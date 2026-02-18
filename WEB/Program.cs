using BLL.Animals.Bovines;
using BLL.Animals.Bovines.Adapters;
using BLL.Animals.Bovines.Contracts;
using BLL.Animals.Ports;

using INFRA;

using Microsoft.EntityFrameworkCore;

using MODEL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextFactory<AgroManagerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Repositório EF (INFRA) -> contrato do MODEL
builder.Services.AddScoped<IBovineRepository, BovineRepositoryEF>();

// Adapter (BLL) -> port genérico
builder.Services.AddScoped<IAnimalRepositoryPort<BovineEntity>, BovineRepositoryPort>();

// Service (BLL)
builder.Services.AddScoped<IBovineService, BovineService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
