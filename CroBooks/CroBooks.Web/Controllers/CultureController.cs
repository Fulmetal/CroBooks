using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.Web.Controllers
{
    [Route("[controller]")]
    public class CultureController : Controller
    {
        [HttpGet("set")]
        public IActionResult Set(string culture, string returnUrl)
        {
            if (culture != null)
            {
                var requestCulture = new RequestCulture(culture, culture);
                //override culture currency to use euro symbol
                requestCulture.UICulture.NumberFormat.CurrencySymbol = "€";
                requestCulture.UICulture.NumberFormat.CurrencyPositivePattern = 2;
                requestCulture.UICulture.NumberFormat.CurrencyNegativePattern = 12;
                var cookieName = CookieRequestCultureProvider.DefaultCookieName;
                var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

                Response.Cookies.Append(cookieName, cookieValue);
            }

            return LocalRedirect(returnUrl);
        }
    }
}
