using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameShop.Controllers.CRUD
{
    public class CrudController : Controller
    {
        // GET: Crud
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Index()
        {
            return View();
        }
    }
}