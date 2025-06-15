namespace HolaBebe.Domain.Entities;

public abstract class AuditableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; private set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }

    public void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
