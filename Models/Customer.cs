using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDevAss2.Models
{
    public class Customer
    {   
        public int CustomerIdentity {get;set;}

        [Key]
        [Required]
        public int CustomerId { get; set; }

        [DataType(DataType.Text)]
        [Required, MaxLength(50), MinLength(1)] 
        public string Name { get; set; } = default!;

        [DataType(DataType.Text)]
        [MinLength(11), MaxLength(11)]
        [RegularExpression(@"\d{3} \d{3} \d{3}",
            ErrorMessage = "Format needs to be XXX XXX XXX")]
        public string? Tfn { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(50), MinLength(1)]
        public string? Address { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(40), MinLength(1)]
        public string? City { get; set; }

        public State? State { get; set; }

        [MaxLength(4), MinLength(4)]
        public string? PostCode { get; set; }

        [RegularExpression(@"04\d\d\s\d\d\d\s\d\d\d",
            ErrorMessage = "Format needs to be 04XX XXX XXX")]
        [MaxLength(12)]
        public string? Mobile { get; set; }

        public ICollection<Account>? Accounts { get; set; }
        public Login? Login { get; set; }

    }
}