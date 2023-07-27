using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebDevAss2.Models;
public class BillPay{

    [Key, Required]
    public int BillPayId{get;set;}

    [Required] // Foreign Key
    public int AccountNumber{get;set;}

    [Required] // Foreign Key
    public int PayeeId{get;set;}

    [Column(TypeName = "money")]
    [Required,Range(0, int.MaxValue, ErrorMessage ="Must be a positive number")]
    public float Amount{get;set;}

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime ScheduleTimeUtc{get;set;}

    [Required]
    [Column(TypeName = "char(1)")]
    public char Period{get;set;}

    public Payee? Payee{get;set;}

    public Account? Account{get;set;}

    public BillPay(int billPayId, int accountNumber, int payeeId, float amount, DateTime scheduleTimeUtc, char period){
        this.BillPayId = billPayId;
        this.AccountNumber = accountNumber;
        this.PayeeId = payeeId;
        this.Amount = amount;
        this.ScheduleTimeUtc = scheduleTimeUtc;
        this.Period = period;
    }

}