using Microsoft.EntityFrameworkCore;
using VacinasApi.Data;

namespace VacinasApi.Postos
{
    public static class PostosEndpoints
    {
        public static void AddEndpointsPostos(this WebApplication app)
        {
            var endpointPostos = app.MapGroup(prefix: "postos");

            endpointPostos.MapGet("", async(AppDbContext context, CancellationToken ct) =>
            {
                var postos = await context.Postos
                .Select(posto => new PostoDto(posto.Id, posto.Nome))
                .ToListAsync(ct);
                return postos;
            });

            endpointPostos.MapPost("", async (AddPostoRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var nameExists = await context.Postos.AnyAsync(posto => posto.Nome == request.Nome, ct);
                if (nameExists)
                    return Results.Conflict("Nome já cadastrado!");

                var newPosto = new Posto(request.Nome);
                await context.Postos.AddAsync(newPosto, ct); 
                await context.SaveChangesAsync(ct);

                var postoRetorno = new PostoDto(newPosto.Id, newPosto.Nome);

                return Results.Ok(postoRetorno);
            });

            endpointPostos.MapPut("{id:guid}", async (Guid id, UpdatePostoRequest request, AppDbContext context, CancellationToken ct) => 
            {
                var posto = await context.Postos
                    .SingleOrDefaultAsync(posto => posto.Id == id, ct);

                if (posto == null)
                    return Results.NotFound();

                posto.UpdatePosto(request.Nome);

                await context.SaveChangesAsync(ct);

                return Results.Ok(new PostoDto(posto.Id, posto.Nome));
            });

            endpointPostos.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
            {
                var posto = await context.Postos
                    .SingleOrDefaultAsync(posto => posto.Id == id, ct);

                if (posto == null)
                    return Results.NotFound();

                posto.Desativar();

                await context.SaveChangesAsync(ct);
                return Results.Ok();
            });
        }
    }
}
