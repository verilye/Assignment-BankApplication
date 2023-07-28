using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDevAss2.Models
{
    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [Required]
        [Column(TypeName = "char")]
        public char TransactionType { get; set; }

        [Required] // Foreign Key
        public int AccountNumber { get; set; }

        // Foreign Key
        public int? DestinationAccountNumber { get; set; }

        [Required, Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        [Column(TypeName = "money")]
        public float Amount { get; set; }

        [MaxLength(30)]
        public string? Comment { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime TransactionTimeUtc { get; set; }

        public Account? Account { get; set; }
    }
}
