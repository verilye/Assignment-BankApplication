using Microsoft.AspNetCore.Mvc;

namespace WebDevAss2.ViewComponents;

[ViewComponentAttribute]
public class ProfileDisplayViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
    
}