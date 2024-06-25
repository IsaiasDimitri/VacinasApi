using Microsoft.EntityFrameworkCore;
using VacinasApi.Data;

namespace VacinasApi.Vacinas
{
    public static class VacinasEdpoints
    {
        public static void AddEndpointsVacinas(this WebApplication app)
        {
            var endpointVacinas = app.MapGroup(prefix: "vacinas");

            endpointVacinas.MapGet("", async (AppDbContext context, CancellationToken ct) =>
            {
                var vacinas = await context.Vacinas
                    .Select(vacina => new VacinaDto(vacina.Id, vacina.Nome))
                    .ToListAsync(ct);
                return vacinas;
            });

            endpointVacinas.MapPost("", async (AddVacinaRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var nameExists = await context.Vacinas.AnyAsync(vacina => vacina.Nome == request.Nome, ct);
                if (nameExists)
                    return Results.Conflict("Nome já cadastrado!");

                var newVacina= new Vacina(request.Nome);
                await context.Vacinas.AddAsync(newVacina, ct);
                await context.SaveChangesAsync(ct);

                var vacinaRetorno = new VacinaDto(newVacina.Id, newVacina.Nome);

                return Results.Ok(vacinaRetorno);
            });

            endpointVacinas.MapPut("{id:guid}", async (Guid id, UpdateVacinaRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var vacina = await context.Vacinas
                    .SingleOrDefaultAsync(posto => posto.Id == id, ct);

                if (vacina == null)
                    return Results.NotFound();

                vacina.UpdateVacina(request.Nome);

                await context.SaveChangesAsync(ct);

                return Results.Ok(new VacinaDto(vacina.Id, vacina.Nome));
            });

            endpointVacinas.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
            {
                var vacina = await context.Vacinas
                    .SingleOrDefaultAsync(posto => posto.Id == id, ct);

                if (vacina == null)
                    return Results.NotFound();

                vacina.Desativar();

                await context.SaveChangesAsync(ct);
                return Results.Ok();
            });
        }
    }
}
