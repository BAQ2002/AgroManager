using INFRA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MODEL;

namespace AgroManager.WEB.Controllers;

public sealed class BovinesController : Controller
{
    private readonly AgroManagerDbContext _db;

    public BovinesController(AgroManagerDbContext db)
    {
        _db = db;
    }

    // /bovines  -> página (view) com tabela
    [HttpGet("/bovines")]
    public IActionResult Index()
    {
        return View();
    }

    // /api/bovines -> dados JSON para preencher a tabela
    [HttpGet("/api/bovines")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var data = await _db.Bovines
            .AsNoTracking()
            .Select(b => new
            {
                b.Id,
                b.Name,
                Gender = b.Gender.ToString(),
                Origin = b.Origin.ToString(),
                b.BirthDate,
                b.Age,
                MaritalStatus = b.MaritalStatus.HasValue ? b.MaritalStatus.Value.ToString() : "",
                CattleType = b.CattleType.HasValue ? b.CattleType.Value.ToString() : ""
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    // /bovines/create -> página de cadastro
    [HttpGet("/bovines/create")]
    public IActionResult Create()
    {
        return View(new CreateBovineVm());
    }

    // POST /bovines/create -> salva no banco
    [HttpPost("/bovines/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBovineVm vm, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(vm);

        var entity = new BovineEntity
        {
            Name = vm.Name,
            Gender = vm.Gender,
            Origin = vm.Origin,
            BirthDate = vm.BirthDate,
            MaritalStatus = vm.MaritalStatus,
            CattleType = vm.CattleType
        };

        _db.Bovines.Add(entity);
        await _db.SaveChangesAsync(ct);

        return RedirectToAction(nameof(Index));
    }
}

// ViewModel simples (pode ir em /ViewModels depois)
public sealed class CreateBovineVm
{
    public string? Name { get; set; }

    public Gender Gender { get; set; }

    public AcquisitionOrigin Origin { get; set; }

    public DateOnly? BirthDate { get; set; }

    public int? Age { get; set; }

    public MaritalStatus? MaritalStatus { get; set; }

    public CattleType? CattleType { get; set; }
}
