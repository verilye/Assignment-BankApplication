using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;
public class BillPay{

    [Key, Required]
    public int billPayId;

    [Required] // Foreign Key
    public int accountNumber;

    [Required] // Foreign Key
    public int payeeId;

    [Required,Range(0, int.MaxValue, ErrorMessage ="Must be a positive number")]
    public float amount;

    [Required, DataType(DataType.Date)]
    public string scheduleTimeUtc;

    [Required]
    public Period period;

}