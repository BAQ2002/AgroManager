using BLL.Services;

using INFRA;

using Microsoft.EntityFrameworkCore;

using MODEL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Registra AgroManagerDbContext como serviço scoped.
//
// Efeito prático:
// - Permite injetar AgroManagerDbContext diretamente em serviços/repositórios web;
// - O ciclo de vida é "um contexto por request HTTP";
// - A instância é criada sob demanda pelo contêiner de DI.
builder.Services.AddDbContext<AgroManagerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Registra fábrica para criação manual de contextos EF Core.
//
// Efeito prático:
// - Permite injetar IDbContextFactory<AgroManagerDbContext>;
// - Cada CreateDbContext/CreateDbContextAsync gera uma NOVA instância;
// - Útil para fluxos fora do request padrão (background jobs, múltiplas unidades de trabalho).
builder.Services.AddDbContextFactory<AgroManagerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Service (BLL)
builder.Services.AddScoped<IBovineService, BovineService>();

// Repositório EF (INFRA) -> contrato do MODEL
builder.Services.AddScoped<IAnimalRepository<BovineEntity>, BovineRepositoryEF>();



var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
