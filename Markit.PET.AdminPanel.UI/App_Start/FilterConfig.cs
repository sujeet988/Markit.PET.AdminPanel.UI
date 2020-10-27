using System.Web;
using System.Web.Mvc;

namespace Markit.PET.AdminPanel.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
