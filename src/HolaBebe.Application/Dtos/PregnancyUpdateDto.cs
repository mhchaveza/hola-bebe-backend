namespace HolaBebe.Application.Dtos;

public sealed class PregnancyUpdateDto
{
    public int? GestationalAgeDays { get; init; }
    public DateTime? ConceptionDate { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? LastMenstruationDate { get; init; }
    public bool? Current { get; init; }
}
