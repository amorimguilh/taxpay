using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace taxpay.payment.store.models;

[Index(nameof(Name), IsUnique = true)]
public record Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Length(3, 25)]
    public string Name { get; set; } = null!;
    public decimal Balance { get; set; }
    public DateTime CreatedDate { get; set; }
}