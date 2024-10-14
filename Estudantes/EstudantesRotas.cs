using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app){
            
            // Cria um agrupamento de rotas
            var rotasEstudantes = app.MapGroup("estudantes");
            
            // Criar estudante
            rotasEstudantes.MapPost("", async (AddEstudanteRequest request, AppDbContext context, CancellationToken ct) => {

                var jaExiste = await context.Estudantes.AnyAsync(e => e.Nome == request.Nome, ct);

                if (jaExiste) return Results.Conflict("Esse nome de estudante já existe :/");

                var novoEstudante = new Estudante(request.Nome);

                await context.Estudantes.AddAsync(novoEstudante ,ct);
                await context.SaveChangesAsync(ct);

                return Results.Created();
            });

            rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) => {
                var estudantes = await context.Estudantes
                .Where(e => e.Ativo)
                .Select(e => new EstudanteDTO(e.Id, e.Nome))
                .ToListAsync(ct);

                if (estudantes is null) return Results.NotFound();

                return Results.Ok(estudantes);
            });

            rotasEstudantes.MapPut("{id:guid}", async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) => {
                
                var jaExiste = await context.Estudantes.AnyAsync(e => e.Nome == request.Nome, ct);

                if (jaExiste) return Results.Conflict("Esse nome de estudante já existe :/");

                var estudante = await context.Estudantes
                                .SingleOrDefaultAsync(e => e.Id == id, ct);
                
                if (estudante is null) return Results.NotFound();

                estudante.AtualizarNome(request.Nome);
                await context.SaveChangesAsync(ct);

                var estudanteAtualizado = new EstudanteDTO(estudante.Id, estudante.Nome);

                return Results.Ok(new EstudanteDTO(estudante.Id, estudante.Nome));

                
            });

            rotasEstudantes.MapDelete("{id:guid}", async (Guid id, AppDbContext context, CancellationToken ct) => {

                var estudante = await context.Estudantes
                                .SingleOrDefaultAsync(e => e.Id == id, ct);

                if (estudante is null) return Results.NotFound();

                estudante.Desativar();
                await context.SaveChangesAsync(ct);

                return Results.NoContent();

            });
        
        }
    }
}