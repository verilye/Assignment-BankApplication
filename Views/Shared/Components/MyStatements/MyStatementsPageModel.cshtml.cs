using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WebDevAss2.Pages
{
    public class MyStatmentsPageModel : PageModel
    {

        public IActionResult OnPostSetSelectedID([FromBody]JsonElement requestData)
        {
            if(requestData.TryGetProperty("selectedOption", out JsonElement selectedOption))
            {
                TempData["SelectedID"] = selectedOption.GetInt32();
                return new JsonResult(new{success=true});
            }

            return new JsonResult(new{success=false});
        }
    }
}