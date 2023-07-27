using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebDevAss2.Models;
public class BillPay{

    [Key, Required]
    public int billPayId{get;set;}

    [Required] // Foreign Key
    public int accountNumber{get;set;}

    [Required] // Foreign Key
    public int payeeId{get;set;}

    [Column(TypeName = "money")]
    [Required,Range(0, int.MaxValue, ErrorMessage ="Must be a positive number")]
    public float amount{get;set;}

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime scheduleTimeUtc{get;set;}

    [Required]
    [Column(TypeName = "char(1)")]
    public char period{get;set;}

    public BillPay(int billPayId, int accountNumber, int payeeId, float amount, DateTime scheduleTimeUtc, char period){
        this.billPayId = billPayId;
        this.accountNumber = accountNumber;
        this.payeeId = payeeId;
        this.amount = amount;
        this.scheduleTimeUtc = scheduleTimeUtc;
        this.period = period;
    }

}