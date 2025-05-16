using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ServidoresAPI.Data;
using ServidoresAPI.Middleware;
using ServidoresAPI.Validators;
using System.Reflection;

namespace ServidoresAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add MediatR
        builder.Services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Add FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateServidorCommandValidator>();

        // Add DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ServidoresDb"));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Add error handling middleware
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        // Seed initial data
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Add sample data
            if (!context.Orgaos.Any())
            {
                context.Orgaos.Add(new ServidoresAPI.Models.Orgao { Nome = "Secretaria de Administração" });
                context.Orgaos.Add(new ServidoresAPI.Models.Orgao { Nome = "Secretaria de Educação" });
                context.SaveChanges();
            }

            if (!context.Lotacoes.Any())
            {
                context.Lotacoes.Add(new ServidoresAPI.Models.Lotacao { Nome = "Departamento de RH" });
                context.Lotacoes.Add(new ServidoresAPI.Models.Lotacao { Nome = "Departamento Financeiro" });
                context.SaveChanges();
            }
        }

        app.Run();
    }
}
