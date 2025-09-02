// INFRA/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace INFRA
{
    // Classe que implementa a interface exigida pelo EF Core para criar o DbContext em tempo de design
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AgroManagerDbContext>
    {
        // Método obrigatório da interface — é chamado automaticamente pelo 'dotnet ef'
        public AgroManagerDbContext CreateDbContext(string[] args)
        {
            // Cria um construtor de configuração (ConfigurationBuilder) para carregar settings de arquivos/variáveis
            var cfg = new ConfigurationBuilder()
                // Define o diretório base de onde os arquivos de configuração serão procurados
                .SetBasePath(Directory.GetCurrentDirectory())
                // Tenta carregar o appsettings.Development.json da camada PL (pasta "PL/PL")
                // 'optional: true' evita erro se o arquivo não existir
                .AddJsonFile(Path.Combine("..", "PL", "PL", "appsettings.Development.json"), optional: true)
                // Adiciona também as variáveis de ambiente do SO como fonte de configuração
                .AddEnvironmentVariables()
                // Finaliza a construção e gera um objeto IConfigurationRoot com todas as configurações carregadas
                .Build();

            // Obtém a connection string (ordem de prioridade):
            // 1) variáveis de ambiente (AGRO_DB) — útil para CI/CD ou Docker
            // 2) appsettings.Development.json (chave "AgroDb")
            // 3) fallback hardcoded (último recurso para não quebrar)
            var cs = Environment.GetEnvironmentVariable("AGRO_DB")
                     ?? cfg.GetConnectionString("AgroDb")
                     ?? "Host=localhost;Port=5432;Database=agromanager_db;Username=admin;Password=admin123";

            // Cria um objeto DbContextOptionsBuilder configurando o provider do PostgreSQL
            // Usa a connection string definida acima
            var opt = new DbContextOptionsBuilder<AgroManagerDbContext>()
                .UseNpgsql(cs)
                .Options;

            // Retorna uma instância de AgroManagerDbContext configurada
            // Essa instância será usada pelo EF Core Tools (migrations, update, etc.)
            return new AgroManagerDbContext(opt);
        }
    }

}
