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
    public Period Period{get;set;}

    [Required]
    public bool Failed{get;set;}

    public int Blocked{get;set;} = 0;

    public Payee? Payee{get;set;}

    public Account? Account{get;set;}

}