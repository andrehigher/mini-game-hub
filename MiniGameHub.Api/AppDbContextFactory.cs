using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniGameHub.Api.Data;

// Implementa a interface IDesignTimeDbContextFactory para dizer ao EF Core como criar o DbContext
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 1. Configuração do DbContextOptionsBuilder
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // 2. Definir o Provedor e a String de Conexão.
        // Se você estiver usando SQLite, a string de conexão é tipicamente o nome do arquivo.
        // ATENÇÃO: Substitua "Data Source=MiniGameHub.db" pela sua string de conexão real, se diferente.
        optionsBuilder.UseSqlite("Data Source=MiniGameHub.db"); 

        return new AppDbContext(optionsBuilder.Options);
    }
}