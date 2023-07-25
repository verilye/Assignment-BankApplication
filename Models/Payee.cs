using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;

public class Payee{

    [Key, Required]
    public int payeeID;

    [Required, MaxLength(50)]
    public string name;

    [Required, DataType(DataType.Text)]
    [MaxLength(50), MinLength(1)]
    public string address;

    [Required,DataType(DataType.Text)]
    [MaxLength(40), MinLength(1)]
    public string city;

    [Required,DataType(DataType.Text)]
    [MaxLength(3), MinLength(2)]
    public State state;

    [Required,MaxLength(4), MinLength(4)]
    public int postCode;

    [Required, RegularExpression(@"", 
        ErrorMessage = "Format needs to be (0X) XXXX XXXX")]
    [MaxLength(12)]
    public int phone;
}