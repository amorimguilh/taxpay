using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taxpay.payment.store.models;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Account SourceAccount { get; set; } = null !;
    public Account DestinationAccount { get; set; } = null !;
    public Accountant Accountant { get; set; } = null !;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
}
