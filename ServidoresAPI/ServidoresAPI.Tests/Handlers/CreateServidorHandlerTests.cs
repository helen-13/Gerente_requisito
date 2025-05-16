using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Commands;
using ServidoresAPI.Data;
using ServidoresAPI.Handlers;
using ServidoresAPI.Models;

namespace ServidoresAPI.Tests.Handlers;

public class CreateServidorHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly CreateServidorHandler _handler;

    public CreateServidorHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new CreateServidorHandler(_context);

        // Setup test data
        _context.Orgaos.Add(new Orgao { Id = 1, Nome = "Test Orgao" });
        _context.Lotacoes.Add(new Lotacao { Id = 1, Nome = "Test Lotacao" });
        _context.SaveChanges();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateServidor()
    {
        // Arrange
        var command = new CreateServidorCommand
        {
            Nome = "Test Servidor",
            Email = "test@test.com",
            Telefone = "123456789",
            OrgaoId = 1,
            LotacaoId = 1,
            Sala = "A1"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result > 0);
        var servidor = await _context.Servidores.FindAsync(result);
        Assert.NotNull(servidor);
        Assert.Equal(command.Nome, servidor.Nome);
        Assert.Equal(command.Email, servidor.Email);
        Assert.Equal(command.Telefone, servidor.Telefone);
        Assert.Equal(command.OrgaoId, servidor.OrgaoId);
        Assert.Equal(command.LotacaoId, servidor.LotacaoId);
        Assert.Equal(command.Sala, servidor.Sala);
        Assert.True(servidor.Ativo);
    }
}