using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;
public class BillPay{

    [Key, Required]
    public int billPayId{get;set;}

    [Required] // Foreign Key
    public int accountNumber{get;set;}

    [Required] // Foreign Key
    public int payeeId{get;set;}

    [Required,Range(0, int.MaxValue, ErrorMessage ="Must be a positive number")]
    public float amount{get;set;}

    [Required, DataType(DataType.Date)]
    public string scheduleTimeUtc{get;set;}

    [Required]
    public char period{get;set;}

    public BillPay(int billPayId, int accountNumber, int payeeId, float amount, string scheduleTimeUtc, char period){
        this.billPayId = billPayId;
        this.accountNumber = accountNumber;
        this.payeeId = payeeId;
        this.amount = amount;
        this.scheduleTimeUtc = scheduleTimeUtc;
        this.period = period;
    }

}