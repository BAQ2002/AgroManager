using BLL.Services;

using INFRA;

using Microsoft.EntityFrameworkCore;

using MODEL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
