using HolaBebe.Domain;

namespace HolaBebe.Application.Dtos;

public sealed class PregnancyDto
{
    public Guid Id { get; init; }
    public bool Current { get; init; }
    public int GestationalAgeDays { get; init; }
    public DateTime? ConceptionDate { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? LastMenstruationDate { get; init; }
    public PregnancyEstimationMethod EstimationMethod { get; init; }
    public int WeekNumber { get; init; }
}
