using MediatR;
using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Data;
using ServidoresAPI.DTOs;


namespace ServidoresAPI.Queries
{
    public class GetServidoresQuery : IRequest<IEnumerable<ServidorResponseDto>>
    {
        // Propriedades e métodos da classe
    }
}
public string Lotacao { get; set; }
    }
}

namespace ServidoresAPI.Handlers
{
    public class GetServidoresHandler : IRequestHandler<GetServidoresQuery, IEnumerable<ServidorResponseDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetServidoresHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ServidorResponseDto>> Handle(GetServidoresQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Servidores
                    .AsNoTracking()
                    .Include(s => s.Orgao)
                    .Include(s => s.Lotacao)
                    .Where(s => s.Ativo)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Nome))
                    query = query.Where(s => s.Nome.Contains(request.Nome.Trim()));

                if (!string.IsNullOrWhiteSpace(request.Orgao))
                    query = query.Where(s => s.Orgao.Nome.Contains(request.Orgao.Trim()));

                if (!string.IsNullOrWhiteSpace(request.Lotacao))
                    query = query.Where(s => s.Lotacao.Nome.Contains(request.Lotacao.Trim()));

                var result = await query.Select(s => new ServidorResponseDto
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Telefone = s.Telefone,
                    Email = s.Email,
                    OrgaoId = s.OrgaoId,
                    LotacaoId = s.LotacaoId,
                    Sala = s.Sala,
                    Ativo = s.Ativo,
                    OrgaoNome = s.Orgao.Nome,
                    LotacaoNome = s.Lotacao.Nome
                }).ToListAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving servidores.", ex);
            }
        }
    }
}
