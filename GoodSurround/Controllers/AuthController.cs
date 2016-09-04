using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoodSurround.Logic;

namespace GoodSurround.Controllers
{
    //test controller
    public class AuthController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Redirect()
        {
            return View();
        }
    }
}