using BLL.Services;

using INFRA;

using Microsoft.EntityFrameworkCore;
using Minio;
using MODEL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextFactory<AgroManagerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<PhotoStorageOptions>(
    builder.Configuration.GetSection(PhotoStorageOptions.SectionName));

builder.Services.AddSingleton<IMinioClient>(_ =>
{
    var cfg = builder.Configuration
        .GetSection(PhotoStorageOptions.SectionName)
        .Get<PhotoStorageOptions>()!;

    return new MinioClient()
        .WithEndpoint(cfg.Endpoint)
        .WithCredentials(cfg.AccessKey, cfg.SecretKey)
        .WithSSL(cfg.UseSsl)
        .Build();
});

builder.Services.AddScoped<IPhotoStorage, MinioPhotoStorage>();

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
