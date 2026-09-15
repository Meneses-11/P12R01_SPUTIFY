using System;
using System.Linq;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class RecomendadorController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        public ActionResult Generar()
        {
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Acceso");

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);
            var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario == null)
                return RedirectToAction("Login", "Acceso");

            int anioNacimiento = usuario.FechaNacimiento.Year;
            int anioBase = anioNacimiento < 1980 ? 1980 : anioNacimiento;
            int decadaBase = (anioBase / 10) * 10;

            var cancionesExactas = db.Canciones
                .Where(c => c.Activo == true && c.AnioLanzamiento == anioBase)
                .OrderByDescending(c => c.Reproducciones)
                .Take(10)
                .ToList();

            var cancionesDecada = db.Canciones
                .Where(c => c.Activo == true &&
                            c.AnioLanzamiento >= decadaBase &&
                            c.AnioLanzamiento <= decadaBase + 9)
                .OrderByDescending(c => c.Reproducciones)
                .Take(20)
                .ToList();

            var cancionesFinales = cancionesExactas
                .Concat(cancionesDecada)
                .GroupBy(c => c.IdCancion)
                .Select(g => g.First())
                .Take(20)
                .ToList();

            var anteriores = db.RecomendacionesUsuario.Where(r => r.IdUsuario == idUsuario).ToList();
            if (anteriores.Any())
            {
                db.RecomendacionesUsuario.RemoveRange(anteriores);
                db.SaveChanges();
            }

            foreach (var cancion in cancionesFinales)
            {
                string motivo = cancion.AnioLanzamiento == anioBase
                    ? "Coincide con tu año base"
                    : "Coincide con tu década base";

                db.RecomendacionesUsuario.Add(new RecomendacionesUsuario
                {
                    IdUsuario = idUsuario,
                    IdCancion = cancion.IdCancion,
                    Motivo = motivo,
                    Puntaje = cancion.AnioLanzamiento == anioBase ? 100 : 80,
                    FechaGenerada = DateTime.Now,
                    Mostrada = false
                });
            }

            db.SaveChanges();

            ViewBag.AnioNacimiento = anioNacimiento;
            ViewBag.AnioBase = anioBase;
            ViewBag.DecadaBase = decadaBase;

            return View(cancionesFinales);
        }

        public ActionResult MisRecomendaciones()
        {
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Acceso");

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            var recomendaciones = db.RecomendacionesUsuario
                .Where(r => r.IdUsuario == idUsuario)
                .OrderByDescending(r => r.Puntaje)
                .ThenByDescending(r => r.FechaGenerada)
                .ToList();

            return View(recomendaciones);
        }

        [HttpPost]
        public ActionResult MarcarMostrada(int id)
        {
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Acceso");

            var recomendacion = db.RecomendacionesUsuario.FirstOrDefault(r => r.IdRecomendacion == id);
            if (recomendacion != null)
            {
                recomendacion.Mostrada = true;
                db.SaveChanges();
            }

            return RedirectToAction("MisRecomendaciones");
        }
    }
}