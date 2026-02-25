using BLL.Common.Exceptions;
using BLL.Services;

using INFRA;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MODEL;

namespace AgroManager.WEB.Controllers;

/// <summary>
/// Controller MVC responsável por operações de leitura e escrita de bovinos.
/// </summary>
/// <remarks>
/// A leitura da listagem utiliza <see cref="IDbContextFactory{TContext}"/> para criar um
/// <see cref="AgroManagerDbContext"/> por operação, enquanto os fluxos de escrita passam pela BLL.
/// </remarks>
public sealed class BovinesController : Controller
{
    private readonly IDbContextFactory<AgroManagerDbContext> _dbFactory;
    private readonly IBovineService _bovineService;

    /// <summary>
    /// Inicializa o controller com dependências de acesso a dados e regras de negócio.
    /// </summary>
    /// <param name="dbFactory">Fábrica de contexto usada para consultas diretas de listagem.</param>
    /// <param name="bovineService">Serviço de domínio para operações de escrita/validação.</param>
    public BovinesController(
        IDbContextFactory<AgroManagerDbContext> dbFactory,
        IBovineService bovineService)
    {
        _dbFactory = dbFactory;
        _bovineService = bovineService;
    }

    // /bovines  -> página (view) com tabela
    [HttpGet("/bovines")]
    public IActionResult Index()
    {
        return View();
    }

    // /api/bovines -> dados JSON para preencher a tabela
    // Cria um DbContext dedicado para a operação através da fábrica (escopo local do método).
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
    public async Task<IActionResult> Create(BovineViewModel vm, CancellationToken ct)
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

        try
        {
            await _bovineService.CreateAsync(entity, ct);

            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            return View(vm);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao salvar o bovino.");

            return View(vm);
        }
    }

    [HttpGet("/bovines/edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var entity = await _bovineService.GetByIdAsync(id, ct);

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
    public async Task<IActionResult> Edit(Guid id, BovineViewModel vm, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(vm);

        var entity = new BovineEntity
        {
            Id = id,                      // 🔹 importante
            Name = vm.Name,
            Gender = vm.Gender,
            Origin = vm.Origin,
            BirthDate = vm.BirthDate,
            MaritalStatus = vm.MaritalStatus,
            CattleType = vm.CattleType
        };

        try
        {
            await _bovineService.UpdateAsync(entity, ct);

            return RedirectToAction(nameof(Index));
        }
        catch (BusinessRuleException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(vm);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(vm);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao atualizar o bovino.");
            return View(vm);
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
