using HolaBebe.Domain;

namespace HolaBebe.Application.Dtos;

public sealed class PregnancyDto
{
    public Guid Id { get; init; }
    public int WeekNumber { get; init; }
    public DateTime? DueDate { get; init; }
    public bool Current { get; init; }
}
