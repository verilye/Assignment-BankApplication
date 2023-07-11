using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;

namespace WebDevAss2.ViewComponents;

[ViewComponentAttribute]
public class ProfileDisplayViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }


}