using HolaBebe.Domain;

namespace HolaBebe.Domain.Entities;

public sealed class Pregnancy : AuditableEntity
{
    public bool Current { get; set; }
    public int GestationalAgeDays { get; set; }
    public DateTime? ConceptionDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? LastMenstruationDate { get; set; }
    public PregnancyEstimationMethod EstimationMethod { get; set; }

    public int WeekNumber => Math.Clamp((int)Math.Floor(GestationalAgeDays / 7d) + 1, 1, 42);
}
