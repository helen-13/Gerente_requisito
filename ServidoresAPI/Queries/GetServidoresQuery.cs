using MediatR;
using ServidoresAPI.DTOs;

namespace ServidoresAPI.Queries;

public record GetServidoresQuery : IRequest<IEnumerable<ServidorResponseDto>>
{
    public string Nome { get; init; }
    public string Orgao { get; init; }
    public string Lotacao { get; init; }
} 