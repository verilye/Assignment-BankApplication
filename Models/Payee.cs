using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;

public class Payee{

    [Key, Required]
    public int payeeID{get;set;}

    [Required, MaxLength(50)]
    public string name{get;set;}

    [Required, DataType(DataType.Text)]
    [MaxLength(50), MinLength(1)]
    public string address{get;set;}

    [Required,DataType(DataType.Text)]
    [MaxLength(40), MinLength(1)]
    public string city{get;set;}

    [Required,DataType(DataType.Text)]
    [MaxLength(3), MinLength(2)]
    public string state{get;set;}

    [Required,MaxLength(4), MinLength(4)]
    public int postCode{get;set;}

    [Required, RegularExpression(@"\(\d\d\)\s[0-9]+\s[0-9]+", 
        ErrorMessage = "Format needs to be (0X) XXXX XXXX")]
    [MaxLength(12)]
    public int phone{get;set;}

    public Payee(int payeeID, string name, string address, string city, string state, int postCode, int phone){
        this.payeeID = payeeID;
        this.name = name;
        this.address = address;
        this.city = city;
        this.state = state;
        this.postCode = postCode;
        this.phone = phone; 
    }
}