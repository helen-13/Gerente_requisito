using MediatR;
using ServidoresAPI.DTOs;

namespace ServidoresAPI.Commands;

public class CreateServidorCommand : IRequest<int>
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int OrgaoId { get; set; }
    public int LotacaoId { get; set; }
    public string Sala { get; set; }
}
