using MediatR;
using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Commands;
using ServidoresAPI.Data;

namespace ServidoresAPI.Handlers;

public class DeleteServidorHandler : IRequestHandler<DeleteServidorCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteServidorHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteServidorCommand request, CancellationToken cancellationToken)
    {
        var servidor = await _context.Servidores
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.Ativo, cancellationToken);

        if (servidor == null)
            return false;

        servidor.Ativo = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 