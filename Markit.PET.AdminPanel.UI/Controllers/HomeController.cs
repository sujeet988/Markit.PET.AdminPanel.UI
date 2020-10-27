using Markit.PET.AdminPanel.CommonLibrary;
using Markit.PET.AdminPanel.UI.RestServiceHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Markit.PET.AdminPanel.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //AdminPanel_AccessEntity _entity = new AdminPanel_AccessEntity() { ApplicationID = 3, EmployeeUEN = "207447" };
            //if (ServiceInfo.InvokePostService<bool>(ServiceInfo.AdminPanel_IsAccessible, JsonConvert.SerializeObject(_entity)))
            //{
                return View();
            //}
            //return View("Error");
        }
    }
}