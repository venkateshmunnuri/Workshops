using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkShopsBucket.Attributes;

namespace WorkShopsBucket.Controllers
{
    public class WorkShop1Controller : Controller
    {
        // GET: WorkShop1
        [WorkshopAuthorize()]
        public ActionResult Index()
        {
            return View();
        }
    }
}