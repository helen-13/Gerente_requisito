using MediatR;

namespace ServidoresAPI.Commands;

public class DeleteServidorCommand : IRequest<bool>
{
    public int Id { get; set; }
} 