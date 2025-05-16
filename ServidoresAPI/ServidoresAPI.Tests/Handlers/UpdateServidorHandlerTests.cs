using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Commands;
using ServidoresAPI.Data;
using ServidoresAPI.Handlers;
using ServidoresAPI.Models;

namespace ServidoresAPI.Tests.Handlers;

public class UpdateServidorHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly UpdateServidorHandler _handler;
    private readonly int _existingServidorId;

    public UpdateServidorHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new UpdateServidorHandler(_context);

        // Setup test data
        var orgao = new Orgao { Id = 1, Nome = "Test Orgao" };
        var lotacao = new Lotacao { Id = 1, Nome = "Test Lotacao" };
        var servidor = new Servidor
        {
            Nome = "Original Nome",
            Email = "original@test.com",
            Telefone = "123456789",
            OrgaoId = 1,
            LotacaoId = 1,
            Sala = "A1",
            Ativo = true
        };

        _context.Orgaos.Add(orgao);
        _context.Lotacoes.Add(lotacao);
        _context.Servidores.Add(servidor);
        _context.SaveChanges();

        _existingServidorId = servidor.Id;
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateServidor()
    {
        // Arrange
        var command = new UpdateServidorCommand
        {
            Id = _existingServidorId,
            Nome = "Updated Nome",
            Email = "updated@test.com",
            Telefone = "987654321",
            OrgaoId = 1,
            LotacaoId = 1,
            Sala = "B2"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        var servidor = await _context.Servidores.FindAsync(_existingServidorId);
        Assert.NotNull(servidor);
        Assert.Equal(command.Nome, servidor.Nome);
        Assert.Equal(command.Email, servidor.Email);
        Assert.Equal(command.Telefone, servidor.Telefone);
        Assert.Equal(command.Sala, servidor.Sala);
    }

    [Fact]
    public async Task Handle_InvalidId_ShouldReturnFalse()
    {
        // Arrange
        var command = new UpdateServidorCommand
        {
            Id = -1,
            Nome = "Updated Nome"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
} 