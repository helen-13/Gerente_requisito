using MediatR;
using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Commands;
using ServidoresAPI.Data;
using ServidoresAPI.Models;

namespace ServidoresAPI.Handlers;

public class CreateServidorHandler : IRequestHandler<CreateServidorCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateServidorHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateServidorCommand request, CancellationToken cancellationToken)
    {
        var servidor = new Servidor
        {
            Nome = request.Nome,
            Telefone = request.Telefone,
            Email = request.Email,
            OrgaoId = request.OrgaoId,
            LotacaoId = request.LotacaoId,
            Sala = request.Sala,
            Ativo = true
        };

        _context.Servidores.Add(servidor);
        await _context.SaveChangesAsync(cancellationToken);

        return servidor.Id;
    }
} 