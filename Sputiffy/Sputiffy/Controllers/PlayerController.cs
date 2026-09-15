using System;
using System.Linq;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class PlayerController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: Player
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegistrarReproduccion(int idCancion, int segundosEscuchados = 0, bool completa = false)
        {
            try
            {
                if (Session["IdUsuario"] == null)
                {
                    return Json(new
                    {
                        ok = false,
                        mensaje = "La sesión ha expirado. Inicia sesión nuevamente."
                    }, JsonRequestBehavior.AllowGet);
                }

                int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

                var cancion = db.Canciones.FirstOrDefault(c => c.IdCancion == idCancion && c.Activo == true);

                if (cancion == null)
                {
                    return Json(new
                    {
                        ok = false,
                        mensaje = "La canción no existe o no está disponible."
                    }, JsonRequestBehavior.AllowGet);
                }

                var historial = new HistorialReproduccion
                {
                    IdUsuario = idUsuario,
                    IdCancion = idCancion,
                    FechaReproduccion = DateTime.Now,
                    SegundosEscuchados = segundosEscuchados,
                    Completa = completa
                };

                db.HistorialReproduccion.Add(historial);

                if (cancion.Reproducciones == null)
                    cancion.Reproducciones = 0;

                cancion.Reproducciones += 1;

                db.SaveChanges();

                return Json(new
                {
                    ok = true,
                    mensaje = "Reproducción registrada correctamente."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    mensaje = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}