using BLL.Common.Exceptions;
using BLL.Services;

using INFRA;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MODEL;

namespace AgroManager.WEB.Controllers;

public sealed class BovinesController : Controller
{
    private readonly IDbContextFactory<AgroManagerDbContext> _dbFactory;    // leitura (List) temporario enquanto nao ha serviço para isso (BLL) - pode ser injetado diretamente no controller, sem depender de AddDbContext
    private readonly IBovineService _bovineService; // escrita (Create)

    public BovinesController(
        IDbContextFactory<AgroManagerDbContext> dbFactory,
        IBovineService bovineService)
    {
        _dbFactory = dbFactory;
        _bovineService = bovineService;
    }
    /// <summary>
    /// /bovines -> página (view) da tabela Db.Bovines.
    /// Utiliza <see cref="Task"/> <see cref="List"/>.
    /// </summary>
    /// <returns>
    /// <see cref="ViewResult"/> utilizando o PL/Views/Bovines/Index.cshtml
    /// como layout Front-End e os dados via Json's preenchidos por <see cref="List"/>.
    /// </returns>
    [HttpGet("/bovines")]
    public IActionResult Index()
    {
        return View();
    }

    // /api/bovines -> dados JSON para preencher a tabela
    // Leitura via DbContextFactory (sem depender de AddDbContext).
    [HttpGet("/api/bovines")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        await using AgroManagerDbContext db = await _dbFactory.CreateDbContextAsync(ct);

        var data = await db.Bovines
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
        return View(new BovineViewModel());
    }

    // POST /bovines/create -> salva no banco via BLL 
    [HttpPost("/bovines/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BovineViewModel bovineViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(bovineViewModel);

        var entity = new BovineEntity
        {
            Name = bovineViewModel.Name,
            Gender = bovineViewModel.Gender,
            Origin = bovineViewModel.Origin,
            BirthDate = bovineViewModel.BirthDate,
            MaritalStatus = bovineViewModel.MaritalStatus,
            CattleType = bovineViewModel.CattleType
        };

        try
        {
            await _bovineService.CreateAsync(entity, cancellationToken);

            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            return View(bovineViewModel);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao salvar o bovino.");

            return View(bovineViewModel);
        }
    }

    [HttpGet("/bovines/edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken CancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var entity = await _bovineService.GetByIdAsync(id, CancellationToken);

        if (entity is null)
            return NotFound();

        var vm = new BovineViewModel
        {
            Name = entity.Name,
            Gender = entity.Gender,
            Origin = entity.Origin,
            BirthDate = entity.BirthDate,
            Age = entity.Age,
            MaritalStatus = entity.MaritalStatus,
            CattleType = entity.CattleType
        };

        return View(vm);
    }

    [HttpPost("/bovines/edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, BovineViewModel bovineViewModel, CancellationToken CancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(bovineViewModel);

        var entity = new BovineEntity
        {
            Id = id,                      // 🔹 importante
            Name = bovineViewModel.Name,
            Gender = bovineViewModel.Gender,
            Origin = bovineViewModel.Origin,
            BirthDate = bovineViewModel.BirthDate,
            MaritalStatus = bovineViewModel.MaritalStatus,
            CattleType = bovineViewModel.CattleType
        };

        try
        {
            await _bovineService.UpdateAsync(entity, CancellationToken);

            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(bovineViewModel);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(bovineViewModel);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao atualizar o bovino.");
            return View(bovineViewModel);
        }
    }
}

/// <summary>
/// Representa um modelo de apresentação (ViewModel) utilizado para transportar
/// dados projetados a partir das entidades de domínio.
/// <para>
/// Não possui <see cref="DbSet{TEntity}"/> registrado no <see cref="AgroManagerDbContext"/>.
/// </para>
/// <para>
/// Não é uma entidade mapeada pelo Entity Framework Core, portanto, suas instâncias não
/// são rastreadas pelo Change Tracker.
/// </para>
/// </summary>
public sealed class BovineViewModel
{
    public string? Name { get; set; }

    public Gender Gender { get; set; }

    public AcquisitionOrigin Origin { get; set; }

    public DateOnly? BirthDate { get; set; }

    public int? Age { get; set; }

    public MaritalStatus? MaritalStatus { get; set; }

    public CattleType? CattleType { get; set; }
}
