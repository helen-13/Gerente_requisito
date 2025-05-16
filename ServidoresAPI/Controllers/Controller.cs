using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configurar EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ServidoresDb"));

// Configurar MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Configurar FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Add Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

#region MODELS

public class Servidor
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public int OrgaoId { get; set; }
    public int LotacaoId { get; set; }
    public string? Sala { get; set; }
    public bool Ativo { get; set; } = true;
}

#endregion

#region DTOs

public class ServidorDto
{
    public string Nome { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public int OrgaoId { get; set; }
    public int LotacaoId { get; set; }
    public string? Sala { get; set; }
}

#endregion

#region DB CONTEXT

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Servidor> Servidores => Set<Servidor>();
}

#endregion

#region VALIDATORS

public class ServidorDtoValidator : AbstractValidator<ServidorDto>
{
    public ServidorDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(x => x.OrgaoId)
            .GreaterThan(0).WithMessage("OrgaoId deve ser maior que zero.");
        RuleFor(x => x.LotacaoId)
            .GreaterThan(0).WithMessage("LotacaoId deve ser maior que zero.");
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email inválido.");
    }
}

#endregion

#region QUERIES

public record GetServidoresQuery(string? Nome, int? OrgaoId, int? LotacaoId) : IRequest<List<Servidor>>;

public class GetServidoresQueryHandler : IRequestHandler<GetServidoresQuery, List<Servidor>>
{
    private readonly AppDbContext _context;

    public GetServidoresQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Servidor>> Handle(GetServidoresQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Servidores.AsQueryable().Where(s => s.Ativo);

        if (!string.IsNullOrEmpty(request.Nome))
        {
            query = query.Where(s => s.Nome.Contains(request.Nome));
        }
        if (request.OrgaoId.HasValue)
        {
            query = query.Where(s => s.OrgaoId == request.OrgaoId.Value);
        }
        if (request.LotacaoId.HasValue)
        {
            query = query.Where(s => s.LotacaoId == request.LotacaoId.Value);
        }
        return await query.ToListAsync(cancellationToken);
    }
}

#endregion

#region COMMANDS

public record CreateServidorCommand(ServidorDto Servidor) : IRequest<int>;

public class CreateServidorCommandHandler : IRequestHandler<CreateServidorCommand, int>
{
    private readonly AppDbContext _context;

    public CreateServidorCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateServidorCommand request, CancellationToken cancellationToken)
    {
        var novoServidor = new Servidor
        {
            Nome = request.Servidor.Nome,
            Telefone = request.Servidor.Telefone,
            Email = request.Servidor.Email,
            OrgaoId = request.Servidor.OrgaoId,
            LotacaoId = request.Servidor.LotacaoId,
            Sala = request.Servidor.Sala,
            Ativo = true
        };

        _context.Servidores.Add(novoServidor);
        await _context.SaveChangesAsync(cancellationToken);

        return novoServidor.Id;
    }
}

public record UpdateServidorCommand(int Id, ServidorDto Servidor) : IRequest<bool>;

public class UpdateServidorCommandHandler : IRequestHandler<UpdateServidorCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateServidorCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateServidorCommand request, CancellationToken cancellationToken)
    {
        var servidor = await _context.Servidores.FindAsync(new object[] { request.Id }, cancellationToken);
        if (servidor == null || !servidor.Ativo) return false;

        servidor.Nome = request.Servidor.Nome;
        servidor.Telefone = request.Servidor.Telefone;
        servidor.Email = request.Servidor.Email;
        servidor.OrgaoId = request.Servidor.OrgaoId;
        servidor.LotacaoId = request.Servidor.LotacaoId;
        servidor.Sala = request.Servidor.Sala;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public record InactivateServidorCommand(int Id) : IRequest<bool>;

public class InactivateServidorCommandHandler : IRequestHandler<InactivateServidorCommand, bool>
{
    private readonly AppDbContext _context;

    public InactivateServidorCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(InactivateServidorCommand request, CancellationToken cancellationToken)
    {
        var servidor = await _context.Servidores.FindAsync(new object[] { request.Id }, cancellationToken);
        if (servidor == null || !servidor.Ativo) return false;

        servidor.Ativo = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

#endregion

#region CONTROLLER

[ApiController]
[Route("api/[controller]")]
public class ServidoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServidoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<Servidor>>> Get([FromQuery] string? nome, [FromQuery] int? orgao, [FromQuery] int? lotacao)
    {
        var query = new GetServidoresQuery(nome, orgao, lotacao);
        var servidores = await _mediator.Send(query);
        return Ok(servidores);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post([FromBody] ServidorDto dto)
    {
        var command = new CreateServidorCommand(dto);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ServidorDto dto)
    {
        var command = new UpdateServidorCommand(id, dto);
        var updated = await _mediator.Send(command);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new InactivateServidorCommand(id);
        var deleted = await _mediator.Send(command);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

#endregion

