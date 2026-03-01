using System;

namespace MODEL;

/// <summary>
/// Representa filtros genéricos reutilizáveis para consultas de entidades que derivam de <see cref="AnimalEntity"/>.
/// A intenção é concentrar critérios transversais que antes tenderiam a ser duplicados em filtros específicos por espécie.
/// </summary>
public sealed class AnimalFiltersModel
{
    /// <summary>
    /// Nome (ou parte do nome) usado no filtro textual. Quando nulo/vazio, o critério é ignorado.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Origem de aquisição do animal. Quando nulo, o critério é ignorado.
    /// </summary>
    public AcquisitionOrigin? Origin { get; init; }

    /// <summary>
    /// Gênero do animal. Quando nulo, o critério é ignorado.
    /// </summary>
    public Gender? Gender { get; init; }

    /// <summary>
    /// Data de nascimento inicial (inclusive) para intervalo de busca. Quando nulo, sem limite inferior.
    /// </summary>
    public DateOnly? BirthDateFrom { get; init; }

    /// <summary>
    /// Data de nascimento final (inclusive) para intervalo de busca. Quando nulo, sem limite superior.
    /// </summary>
    public DateOnly? BirthDateTo { get; init; }

    /// <summary>
    /// Quantidade máxima de registros retornados.
    /// </summary>
    public int? Take { get; init; }

    /// <summary>
    /// Quantidade de registros a serem ignorados antes de começar a retornar resultados.
    /// </summary>
    public int? Skip { get; init; }
}
