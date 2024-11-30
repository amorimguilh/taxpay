namespace taxpay.payment.domain.entities;

public record AccountEntity
{
    public decimal Balance { get; set; }
    public string Name { get; set; } = null!;
}
