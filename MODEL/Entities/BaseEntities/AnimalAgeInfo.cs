namespace MODEL;

/// <summary>
/// Snapshot de idade do animal em múltiplas unidades para uso em APIs e Views.
/// </summary>
public readonly record struct AnimalAgeInfo(int Days, int Months, int Years);
