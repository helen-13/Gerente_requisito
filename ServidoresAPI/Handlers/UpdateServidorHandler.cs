using MediatR;
using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Commands;
using ServidoresAPI.Data;

namespace ServidoresAPI.Handlers;

public class UpdateServidorHandler : IRequestHandler<UpdateServidorCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public UpdateServidorHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateServidorCommand request, CancellationToken cancellationToken)
    {
        var servidor = await _context.Servidores
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.Ativo, cancellationToken);

        if (servidor == null)
            return false;

        servidor.Nome = request.Nome;
        servidor.Telefone = request.Telefone;
        servidor.Email = request.Email;
        servidor.OrgaoId = request.OrgaoId;
        servidor.LotacaoId = request.LotacaoId;
        servidor.Sala = request.Sala;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 