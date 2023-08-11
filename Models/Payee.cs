using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDevAss2.Models
{
    public class Payee
    {
        [Key, Required]
        public int PayeeID { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required, DataType(DataType.Text)]
        [MaxLength(50), MinLength(1)]
        public string Address { get; set; } = default!;

        [Required, DataType(DataType.Text)]
        [MaxLength(40), MinLength(1)]
        public string City { get; set; }= default!;

        [Required, DataType(DataType.Text)]
        [MaxLength(3), MinLength(2)]
        public State State { get; set; }

        [Required, MaxLength(4), MinLength(4)]
        public string PostCode { get; set; }= default!;

        [Required, RegularExpression(@"\(\d\d\)\s[0-9]+\s[0-9]+",
            ErrorMessage = "Format needs to be (0X) XXXX XXXX")]
        [MaxLength(12)]
        public string Phone { get; set; }= default!;

        public ICollection<BillPay>? BillPays { get; set; }

    }
}
