using INFRA;                                      // Camada INFRA (DbContext / persistência).
using Microsoft.AspNetCore.Mvc;                   // Base de Controllers e resultados IActionResult.
using Microsoft.EntityFrameworkCore;              // Extensões EF Core (AsNoTracking / ToListAsync).
using MODEL;                                      // Entidades e enums do domínio.

namespace AgroManager.WEB.Controllers;            // Namespace da camada WEB (MVC).

/// <summary>
/// Controller MVC responsável por listar e cadastrar Suínos.
/// - Entrega View (HTML) para telas.
/// - Entrega JSON (API) para preencher tabelas via JavaScript.
/// </summary>
public sealed class SwinesController : Controller
{
    private readonly AgroManagerDbContext _db;    // DbContext EF Core para leitura/gravação no banco.

    /// <summary>
    /// Injeta o DbContext para acesso ao banco via EF Core.
    /// </summary>
    /// <param name="db">Contexto do AgroManager.</param>
    public SwinesController(AgroManagerDbContext db)
    {
        _db = db;                                 // Armazena a dependência para uso nos endpoints.
    }

    // /swines -> página (view) com tabela
    [HttpGet("/swines")]
    public IActionResult Index()
    {
        return View();                            // Retorna a View padrão associada a Index.
    }

    // /api/swines -> dados JSON para preencher a tabela
    [HttpGet("/api/swines")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var data = await _db.Swines               // Fonte: tabela/DbSet de suínos.
            .AsNoTracking()                       // Otimiza leitura (não rastreia mudanças).
            .Select(s => new                      // Projeta para um objeto anônimo (JSON enxuto).
            {
                s.Id,                             // Identificador do suíno.
                s.Name,                           // Nome do suíno.
                Gender = s.Gender.ToString(),     // Enum -> string (facilita exibição na tabela).
                Origin = s.Origin.ToString(),     // Enum -> string (facilita exibição na tabela).
                s.BirthDate,                      // Data de nascimento.
                s.Age,                            // Idade (se existir/calculada no modelo).
                AgeInDays = s.GetAge(AgeUnit.Days),
                AgeInMonths = s.GetAge(AgeUnit.Months),
                AgeInYears = s.GetAge(AgeUnit.Years),
                PorcType = s.PorcType.HasValue
                    ? s.PorcType.Value.ToString()
                    : ""                          // Se nulo, retorna string vazia (evita "null" no front).
            })
            .ToListAsync(ct);                     // Executa no banco e materializa em lista (cancelável).

        return Ok(data);                          // 200 OK com JSON (para DataTables/fetch/etc).
    }

    // /swines/create -> página de cadastro
    [HttpGet("/swines/create")]
    public IActionResult Create()
    {
        return View(new SwineViewModel());         // Retorna a View com ViewModel "vazio".
    }

    // POST /swines/create -> salva no banco
    [HttpPost("/swines/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SwineViewModel vm, CancellationToken ct)
    {
        if (!ModelState.IsValid)                  // Validação automática (DataAnnotations, etc).
            return View(vm);                      // Reexibe a tela mantendo os valores digitados.

        var entity = new SwineEntity              // Mapeia ViewModel -> Entidade de domínio.
        {
            Name = vm.Name,                  // Nome informado no formulário.
            Gender = vm.Gender,                // Sexo informado no formulário.
            Origin = vm.Origin,                // Origem/aquisição informada no formulário.
            BirthDate = vm.BirthDate,             // Data de nascimento informada no formulário.
            PorcType = vm.PorcType               // Tipo de suíno (opcional).
        };

        _db.Swines.Add(entity);                   // Marca a entidade para inserção.
        await _db.SaveChangesAsync(ct);           // Persiste no banco (cancelável).

        return RedirectToAction(nameof(Index));   // Redireciona para a listagem (evita repost).
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
public sealed class SwineViewModel
{
    public string? Name { get; set; }             // Nome do suíno (campo livre).

    public Gender Gender { get; set; }            // Sexo (enum).

    public AcquisitionOrigin Origin { get; set; } // Origem/aquisição (enum).

    public DateOnly? BirthDate { get; set; }      // Data de nascimento (opcional).

    public int? Age { get; set; }                 // Idade (opcional; pode ser derivada do BirthDate).

    public int? AgeInDays { get; set; }

    public int? AgeInMonths { get; set; }

    public int? AgeInYears { get; set; }

    public PorcType? PorcType { get; set; }       // Tipo de suíno (opcional).
}
