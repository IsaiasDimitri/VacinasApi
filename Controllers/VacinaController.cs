using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacinasApi.Data;
using VacinasApi.Models;
using VacinasApi.ViewModels;

namespace VacinasApi.Controllers
{
    [ApiController]
    [Route("v1")]
    public class VacinaController : ControllerBase
    {
        [HttpGet]
        [Route("vacinas")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context, CancellationToken ct)
        {
            var postos = await context
                                .Vacinas
                                .Where(v => v.Ativo)
                                .AsNoTracking()
                                .ToListAsync(ct);
            return Ok(postos);
        }

        [HttpGet]
        [Route("vacinas/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] Guid id, CancellationToken ct)
        {
            var posto = await context
                                .Vacinas
                                .AsNoTracking()
                                .Where(v => v.Ativo)
                                .FirstOrDefaultAsync(x => x.Id == id, ct);

            return posto == null ? NotFound() : Ok(posto);
        }

        [HttpPost]
        [Route("vacina")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateVacinaViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (model.Validade <= DateTime.Now)
                return BadRequest("A data de validade precisa ser uma data futura!");

            var vacinaLote = await context
                                .Vacinas
                                .AsNoTracking()
                                .Where(v => v.Lote == model.Lote)
                                .FirstOrDefaultAsync(ct);

            if (vacinaLote != null)
                return BadRequest("Lote já existente!");


            var vacina = new Vacina(model.Nome, model.Fabricante, model.Lote, model.Quantidade, model.Validade);

            await context.Vacinas.AddAsync(vacina, ct);
            await context.SaveChangesAsync(ct);

            return Created($"v1/vacina/{vacina.Id}", vacina);
        }

        [HttpPut]
        [Route("vacina/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] UpdateVacinaViewModel model, [FromRoute] Guid id, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var vacina = await context
                                .Vacinas
                                .Where(v => v.Ativo)
                                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (vacina == null)
                return NotFound();

            vacina.Nome = model.Nome == null ? vacina.Nome : model.Nome;
            vacina.Fabricante = model.Fabricante == null ? vacina.Fabricante : model.Fabricante;
            vacina.Lote = model.Lote == null ? vacina.Lote : model.Lote;
            vacina.Quantidade = model.Quantidade;
            vacina.Validade = model.Validade;

            context.Vacinas.Update(vacina);
            await context.SaveChangesAsync(ct);

            return Created($"v1/vacina/{vacina.Id}", vacina);
        }

        [HttpDelete]
        [Route("vacina/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] Guid id, CancellationToken ct)
        {
            var vacina = await context
                                .Vacinas
                                .Where(v => v.Ativo)
                                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (vacina == null)
                return NotFound();

            vacina.Desativar();
            await context.SaveChangesAsync(ct);
            return Ok();
        }
    }
}
