using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VacinasApi.Data;
using VacinasApi.Models;
using VacinasApi.ViewModels;

namespace VacinasApi.Controllers
{
    [ApiController]
    [Route("v1")]
    public class PostoController : ControllerBase
    {
        [HttpGet]
        [Route("postos")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context, CancellationToken ct)
        {
            var postos = await context
                                .Postos
                                .AsNoTracking()
                                .Where(p => p.Ativo)
                                .Select(p => new CreatePostoViewModel
                                 {
                                     Nome = p.Nome,
                                     Vacinas = p.Vacinas.Select(v => new CreateVacinaViewModel
                                     {
                                         Nome = v.Nome,
                                         Fabricante = v.Fabricante,
                                         Lote = v.Lote,
                                         Quantidade = v.Quantidade,
                                         Validade = v.Validade,
                                     }).ToList()
                                 })
                                .ToListAsync(ct);
            return Ok(postos);
        }

        [HttpGet]
        [Route("postos/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] Guid id, CancellationToken ct)
        {
            var posto = await context
                                .Postos
                                .AsNoTracking()
                                .Where(p => p.Ativo)
                                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (posto == null)
            {
                return NotFound();
            }

            var postoDto = new CreatePostoViewModel
            {
                Nome = posto.Nome,
                Vacinas = posto.Vacinas.Select(v => new CreateVacinaViewModel
                {
                    Nome = v.Nome,
                    Fabricante = v.Fabricante,
                    Lote = v.Lote,
                    Quantidade = v.Quantidade,
                    Validade = v.Validade
                }).ToList()
            };

            return Ok(postoDto);
        }

        [HttpPost]
        [Route("posto")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreatePostoViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var posto = new Posto(model.Nome);

            await context.Postos.AddAsync(posto, ct);
            await context.SaveChangesAsync(ct);

            return Created($"v1/posto/{posto.Id}", posto);
        }

        [HttpPut]
        [Route("posto/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] UpdatePostoViewModel model, [FromRoute] Guid id, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var posto = await context
                                .Postos
                                .Where(p => p.Ativo)
                                .Include(p => p.Vacinas)
                                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (posto == null)
                return NotFound();

            posto.Nome = model.Nome;

            var lotesExistentes = new HashSet<string>();

            foreach (var vacinaViewModel in model.Vacinas)
            {
                if (lotesExistentes.Contains(vacinaViewModel.Lote))
                {
                    return BadRequest($"Já existe uma vacina com o lote '{vacinaViewModel.Lote}' neste posto.");
                }

                var vacina = new Vacina {
                    Nome = vacinaViewModel.Nome,
                    Fabricante = vacinaViewModel.Fabricante,
                    Lote = vacinaViewModel.Lote,
                    Quantidade = vacinaViewModel.Quantidade,
                    Validade = vacinaViewModel.Validade
                };

                lotesExistentes.Add(vacina.Lote);

                posto.Vacinas.Add(vacina);
            }

            context.Postos.Update(posto);
            await context.SaveChangesAsync(ct);

            return Created($"v1/posto/{posto.Id}", posto);
        }

        [HttpPut]
        [Route("posto/{postoId}/vacinas")]
        public async Task<IActionResult> AddVacinaAsync(
                                                            [FromServices] AppDbContext context,
                                                            [FromBody] Guid vacinaId,
                                                            [FromRoute] Guid postoId,
                                                            CancellationToken ct)
        {
            var posto = await context.Postos.FindAsync(postoId, ct);
            if (posto == null)
            {
                return NotFound("Posto não encontrado.");
            }

            var vacina = await context.Vacinas.FindAsync(vacinaId, ct);
            if (vacina == null)
            {
                return NotFound("Vacina não encontrada.");
            }

            if (vacina.PostoId != null)
            {
                return BadRequest("A vacina já está associada a um posto.");
            }

            vacina.DefinirPosto(posto);

            try
            {
                await context.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Ocorreu um erro ao salvar a associação da vacina ao posto.");

            }

            return Ok("Vacina associada ao posto com sucesso.");
        }

        [HttpDelete]
        [Route("posto/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] Guid id, CancellationToken ct)
        {
            var posto = await context
                                .Postos
                                .Where(p => p.Ativo)
                                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if(posto == null)
                return NotFound();

            if (posto.Vacinas.Count >= 0)
                return BadRequest("Não se pode excluir postos com vacinas cadastradas!");

            posto.Desativar();
            await context.SaveChangesAsync(ct);
            return Ok();
        }
    }
}
