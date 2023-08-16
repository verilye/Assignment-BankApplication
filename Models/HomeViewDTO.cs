namespace WebDevAss2.Models;

public class HomeViewDTO{
    public List<AccountViewModel> AccountViewModels = default!;
    public Customer Customer;
    public List<BillPay> BillPays = default!;
}