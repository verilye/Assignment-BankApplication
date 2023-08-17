using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDevAss2.Models
{
    public class Login
    {
        [Key]
        [Required]
        [Column(TypeName = "char")]
        [MaxLength(8), MinLength(8)]
        public string LoginId { get; set; } = default!;

        // Foreign Key
        public int CustomerId { get; set; }

        [Required]
        [Column(TypeName = "char")]
        [MaxLength(94), MinLength(3)]
        public string PasswordHash { get; set; } = default!;

        public int Locked {get;set;} = 0!;

        public Customer? Customer {get;set;}

    }
}