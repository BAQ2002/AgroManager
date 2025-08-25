using Microsoft.EntityFrameworkCore;
using INFRA; // onde está AgroManagerDbContext

namespace PL
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // 1. Ler connection string do appsettings.json ou .env
            var connectionString = "Host=localhost;Port=5432;Database=agromanager;Username=agro;Password=agro";

            var optionsBuilder = new DbContextOptionsBuilder<AgroManagerDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            // 2. Criar instância do contexto
            using var dbContext = new AgroManagerDbContext(optionsBuilder.Options);

            // Opcional: garantir que DB está criado/migrado
            dbContext.Database.Migrate();

            // 3. Passar o contexto para seu Form (injeção manual)
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(dbContext));
        }
    }
}
