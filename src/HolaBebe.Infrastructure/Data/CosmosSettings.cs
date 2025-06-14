namespace HolaBebe.Infrastructure.Data;

public sealed class CosmosSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseId { get; set; } = "HolaBebe";
}
