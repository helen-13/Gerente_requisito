using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Models;

namespace ServidoresAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Servidor> Servidores { get; set; }
    public DbSet<Orgao> Orgaos { get; set; }
    public DbSet<Lotacao> Lotacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Servidor>()
            .HasOne(s => s.Orgao)
            .WithMany(o => o.Servidores)
            .HasForeignKey(s => s.OrgaoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Servidor>()
            .HasOne(s => s.Lotacao)
            .WithMany(l => l.Servidores)
            .HasForeignKey(s => s.LotacaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 