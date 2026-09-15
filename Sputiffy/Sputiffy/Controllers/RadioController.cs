using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sputiffy.Controllers
{
    public class RadioController : Controller
    {
        // GET: /Radio/
        public ActionResult Index()
        {
            // Aquí podrías pasar datos dinámicos si luego quieres
            return View();
        }
    }
}