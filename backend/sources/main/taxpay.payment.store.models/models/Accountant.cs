using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace taxpay.payment.store.models;

[Index(nameof(Email), IsUnique = true)]
public class Accountant
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Length(3, 25)]
    public string FirstName { get; set; } =  null!;
    [Length(3, 25)]
    public string LastName { get; set; } =  null!;
    [Length(6, 25)]
    public string Email { get; set; } =  null!;
}

// Here I added email as the unique constraint in order to avoid duplicated accountants
// Which could be replaced by a social secutiry number or even an accountant register id (if that exists in US)