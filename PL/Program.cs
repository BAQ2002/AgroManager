using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using INFRA;
using MODEL;
using BLL;

namespace PL
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Configura o pipeline do Windows Forms
            ApplicationConfiguration.Initialize();

            // Cria um Host genérico para habilitar DI, IConfiguration e logging
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    // Carrega arquivos de configuração e variáveis de ambiente
                    cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                       .AddEnvironmentVariables();
                })
                .ConfigureServices((ctx, services) =>
                {
                    // Obtém a connection string do appsettings ou da variável de ambiente AGRO_DB
                    var cs = ctx.Configuration.GetConnectionString("AgroDb")
                             ?? Environment.GetEnvironmentVariable("AGRO_DB")
                             ?? "Host=localhost;Port=5432;Database=agromanager;Username=agro;Password=agro";

                    // Registra o DbContextFactory (forma recomendada em WinForms)
                    services.AddDbContextFactory<AgroManagerDbContext>(o => o.UseNpgsql(cs));

                    // Registra os repositórios (implementações de INFRA para contratos do MODEL)
                    services.AddScoped<IBovineRepository, BovineRepositoryEF>();
                    // services.AddScoped<ISwineRepository, SwineRepositoryEF>();
                    // ...outros repositórios

                    // Registra os serviços de negócio da BLL
                    //services.AddScoped<IBovineService, BovineService>();
                    //services.AddScoped<ISwineService, SwineService>();

                    // Registra o form inicial
                    services.AddTransient<Form1>();
                })
                .Build();

            // Resolve o form principal via DI
            var form = host.Services.GetRequiredService<Form1>();

            // Inicia a aplicação
            Application.Run(form);
        }
    }
}

