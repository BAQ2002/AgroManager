using BLL.Common.Exceptions;
using BLL.Services;
using System;

using Microsoft.AspNetCore.Mvc;
using MODEL;

namespace AgroManager.WEB.Controllers;

/// <summary>
/// Controller responsável por expor os endpoints HTTP de listagem, criação e edição de bovinos.
/// Encaminha as regras de negócio para <see cref="IBovineService"/> e mantém no controller apenas
/// a orquestração de fluxo Web (model binding, validação de <see cref="ModelStateDictionary"/> e retorno de views/resultados HTTP).
/// </summary>
public sealed class BovinesController : Controller
{
    private readonly IBovineService _bovineService;

    /// <summary>
    /// Inicializa o controller com a dependência de serviço de bovinos injetada pelo container DI.
    /// </summary>
    /// <param name="bovineService">Serviço utilizado para executar operações de negócio sobre <see cref="BovineEntity"/>.</param>
    public BovinesController(IBovineService bovineService)
    {
        _bovineService = bovineService;
    }

    [HttpGet("/bovines")]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Renderiza a página principal de bovinos.
    /// A tabela da tela é populada de forma assíncrona pelo endpoint <see cref="List(CancellationToken)"/>.
    /// </summary>
    /// <returns><see cref="ViewResult"/> para a view padrão de index de bovinos.</returns>
    [HttpGet("/bovines/records")]
    public IActionResult IndexBovine()
    {
        return View();
    }

    /// <summary>
    /// Obtém os bovinos via <see cref="IBovineService.ListAsync(CancellationToken)"/>, projeta os dados em formato
    /// pronto para serialização JSON (incluindo conversão de enums para texto) e retorna o resultado com <see cref="Ok(object?)"/>.
    /// </summary>
    /// <param name="ct">Token de cancelamento propagado para a camada de serviço.</param>
    /// <returns><see cref="OkObjectResult"/> com a coleção projetada para consumo da tabela no front-end.</returns>
    [HttpGet("/api/bovines")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var bovines = await _bovineService.ListAsync(ct);

        var data = bovines
            .Select(b => new
            {
                b.Id,
                b.Name,
                Gender = b.Gender.ToString(),
                Origin = b.Origin.ToString(),
                b.BirthDate,
                b.Age,
                AgeInDays = b.GetAge(AgeUnit.Days),
                AgeInMonths = b.GetAge(AgeUnit.Months),
                AgeInYears = b.GetAge(AgeUnit.Years),
                MaritalStatus = b.MaritalStatus.HasValue ? b.MaritalStatus.Value.ToString() : "",
                CattleType = b.CattleType.HasValue ? b.CattleType.Value.ToString() : ""
            });

        return Ok(data);
    }

    /// <summary>
    /// Renderiza a página de cadastro inicializando uma nova instância de <see cref="BovineViewModel"/>
    /// para preencher o formulário da view.
    /// </summary>
    /// <returns><see cref="ViewResult"/> com um model vazio para criação.</returns>
    [HttpGet("/bovines/create")]
    public IActionResult CreateBovine()
    {
        return View(new BovineViewModel());
    }

    /// <summary>
    /// Processa o envio do formulário de criação.
    /// Valida <see cref="ModelState"/>, mapeia <see cref="BovineViewModel"/> para <see cref="BovineEntity"/>,
    /// chama <see cref="IBovineService.CreateAsync(BovineEntity, CancellationToken)"/> e redireciona para <see cref="Index"/> em caso de sucesso.
    /// Em caso de falha de regra de negócio ou erro inesperado, registra a mensagem no <see cref="ModelState"/> e retorna a mesma view.
    /// </summary>
    /// <param name="bovineViewModel">Model recebido do formulário de criação.</param>
    /// <param name="cancellationToken">Token de cancelamento propagado para a camada de serviço.</param>
    /// <returns>
    /// <see cref="RedirectToActionResult"/> quando a criação é concluída; caso contrário,
    /// <see cref="ViewResult"/> com o model original e mensagens de erro.
    /// </returns>
    [HttpPost("/bovines/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBovine(BovineViewModel bovineViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(bovineViewModel);

        ValidateCreateBusinessRules(bovineViewModel);

        if (!ModelState.IsValid) return View(bovineViewModel);

        try
        {
            var entity = new BovineEntity
            {
                Name = bovineViewModel.Name,
                Gender = bovineViewModel.Gender,
                Origin = bovineViewModel.Origin,
                BirthDate = bovineViewModel.BirthDate,
                MaritalStatus = bovineViewModel.MaritalStatus,
                CattleType = bovineViewModel.CattleType
            };

            await _bovineService.CreateAsync(entity, cancellationToken);

            return RedirectToAction(nameof(IndexBovine));
        }
        catch (BusinessRuleException ex)
        {
            AddCreateModelError(ex.Message);

            return View(bovineViewModel);
        }
        catch (Exception)
        {
            AddCreateModelError("Ocorreu um erro inesperado ao salvar o bovino.");

            return View(bovineViewModel);
        }
    }

    /// <summary>
    /// Carrega os dados para edição de um bovino.
    /// Valida o identificador, chama <see cref="IBovineService.GetByIdAsync(Guid, CancellationToken)"/>,
    /// projeta a entidade para <see cref="BovineViewModel"/> e retorna a view de edição.
    /// </summary>
    /// <param name="id">Identificador do bovino que será editado.</param>
    /// <param name="CancellationToken">Token de cancelamento propagado para a camada de serviço.</param>
    /// <returns>
    /// <see cref="BadRequestResult"/> para id inválido, <see cref="NotFoundResult"/> para registro inexistente
    /// ou <see cref="ViewResult"/> com o model preenchido.
    /// </returns>
    [HttpGet("/bovines/edit/{id:guid}")]
    public async Task<IActionResult> EditBovine(Guid id, CancellationToken CancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var entity = await _bovineService.GetByIdAsync(id, CancellationToken);

        if (entity is null)
            return NotFound();

        var vm = new BovineViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Gender = entity.Gender,
            Origin = entity.Origin,
            BirthDate = entity.BirthDate,
            Age = entity.Age,
            AgeInDays = entity.GetAge(AgeUnit.Days),
            AgeInMonths = entity.GetAge(AgeUnit.Months),
            AgeInYears = entity.GetAge(AgeUnit.Years),
            MaritalStatus = entity.MaritalStatus,
            CattleType = entity.CattleType
        };

        return View(vm);
    }

    /// <summary>
    /// Processa o envio do formulário de edição.
    /// Resolve o identificador final (rota ou model), valida o estado do model,
    /// mapeia os dados para <see cref="BovineEntity"/> e chama <see cref="IBovineService.UpdateAsync(BovineEntity, CancellationToken)"/>.
    /// Em caso de erro de regra de negócio, registro inexistente ou exceção inesperada, adiciona mensagem no <see cref="ModelState"/> e retorna a view.
    /// </summary>
    /// <param name="id">Identificador informado na rota.</param>
    /// <param name="bovineViewModel">Model enviado pelo formulário de edição.</param>
    /// <param name="CancellationToken">Token de cancelamento propagado para a camada de serviço.</param>
    /// <returns>
    /// <see cref="RedirectToActionResult"/> quando a atualização é concluída; caso contrário,
    /// resultado de erro de requisição ou <see cref="ViewResult"/> com mensagens de validação.
    /// </returns>
    [HttpPost("/bovines/edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, BovineViewModel bovineViewModel, CancellationToken CancellationToken)
    {
        if (id == Guid.Empty && bovineViewModel.Id != Guid.Empty)
            id = bovineViewModel.Id;

        if (id == Guid.Empty)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(bovineViewModel);

        var entity = new BovineEntity
        {
            Id = id,
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

            return RedirectToAction(nameof(IndexBovine));
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
    private void ValidateCreateBusinessRules(BovineViewModel bovineViewModel)
    {
        if (bovineViewModel.CattleType is null)
            ModelState.AddModelError(nameof(BovineViewModel.CattleType), "O tipo do bovino deve ser informado.");

        if (bovineViewModel.Gender == Gender.Unknown)
            ModelState.AddModelError(nameof(BovineViewModel.Gender), "O gênero do bovino deve ser informado.");

        if (bovineViewModel.Gender == Gender.Female && bovineViewModel.MaritalStatus is null)
            ModelState.AddModelError(nameof(BovineViewModel.MaritalStatus), "O estado marital do bovino deve ser informado para fêmeas.");
    }

    private void AddCreateModelError(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            ModelState.AddModelError(nameof(BovineViewModel.Name), "Valor inválido informado.");

            return;
        }

        var messages = message.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (messages.Length == 0)
        {
            messages = new[] { message };
        }

        foreach (var item in messages)
        {
            var normalizedMessage = item.ToLowerInvariant();
            var fieldName = nameof(BovineViewModel.Name);

            if (normalizedMessage.Contains("estado marital"))
                fieldName = nameof(BovineViewModel.MaritalStatus);
            else if (normalizedMessage.Contains("tipo"))
                fieldName = nameof(BovineViewModel.CattleType);
            else if (normalizedMessage.Contains("gênero") || normalizedMessage.Contains("genero"))
                fieldName = nameof(BovineViewModel.Gender);
            else if (normalizedMessage.Contains("nascimento"))
                fieldName = nameof(BovineViewModel.BirthDate);
            else if (normalizedMessage.Contains("origem"))
                fieldName = nameof(BovineViewModel.Origin);
            else if (normalizedMessage.Contains("idade"))
                fieldName = nameof(BovineViewModel.Age);
            else if (normalizedMessage.Contains("nome"))
                fieldName = nameof(BovineViewModel.Name);

            ModelState.AddModelError(fieldName, item);
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
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Gender Gender { get; set; }

    public AcquisitionOrigin Origin { get; set; }

    public DateOnly? BirthDate { get; set; }

    public int? Age { get; set; }

    public int? AgeInDays { get; set; }

    public int? AgeInMonths { get; set; }

    public int? AgeInYears { get; set; }

    public MaritalStatus? MaritalStatus { get; set; }

    public CattleType? CattleType { get; set; }
}
    [HttpGet("/bovines")]
    public IActionResult Index()
    {
        return View();
    }
