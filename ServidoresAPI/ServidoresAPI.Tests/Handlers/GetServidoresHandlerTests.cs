using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Data;
using ServidoresAPI.Handlers;
using ServidoresAPI.Models;
using ServidoresAPI.Queries;

namespace ServidoresAPI.Tests.Handlers;

public class GetServidoresHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly GetServidoresHandler _handler;

    public GetServidoresHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new GetServidoresHandler(_context);

        // Setup test data
        var orgao = new Orgao { Id = 1, Nome = "Test Orgao" };
        var lotacao = new Lotacao { Id = 1, Nome = "Test Lotacao" };
        
        _context.Orgaos.Add(orgao);
        _context.Lotacoes.Add(lotacao);
        
        _context.Servidores.Add(new Servidor
        {
            Id = 1,
            Nome = "Test Servidor",
            Email = "test@test.com",
            Telefone = "123456789",
            OrgaoId = orgao.Id,
            LotacaoId = lotacao.Id,
            Sala = "A1",
            Ativo = true
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task Handle_NoFilters_ShouldReturnAllActiveServidores()
    {
        // Arrange
        var query = new GetServidoresQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var servidores = result.ToList();
        Assert.Single(servidores);
        Assert.Equal("Test Servidor", servidores[0].Nome);
        Assert.Equal("Test Orgao", servidores[0].OrgaoNome);
        Assert.Equal("Test Lotacao", servidores[0].LotacaoNome);
    }

    [Fact]
    public async Task Handle_WithNomeFilter_ShouldReturnFilteredServidores()
    {
        // Arrange
        var query = new GetServidoresQuery { Nome = "Test" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var servidores = result.ToList();
        Assert.Single(servidores);
        Assert.Equal("Test Servidor", servidores[0].Nome);
    }

    [Fact]
    public async Task Handle_WithNonExistingNome_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetServidoresQuery { Nome = "NonExisting" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
} 