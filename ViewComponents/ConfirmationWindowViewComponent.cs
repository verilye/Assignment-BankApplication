using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;

namespace WebDevAss2.ViewComponents;

[ViewComponentAttribute]
public class ConfirmationWindowViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Transaction transaction)
    {
        return View(transaction);
    }
}