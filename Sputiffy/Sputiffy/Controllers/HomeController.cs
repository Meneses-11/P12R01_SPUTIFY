using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class HomeController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        public ActionResult Index()
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

            string terminoEpoca = decadaBase + " hits";

            if (decadaBase == 1980) terminoEpoca = "80s hits";
            else if (decadaBase == 1990) terminoEpoca = "90s hits";
            else if (decadaBase == 2000) terminoEpoca = "2000s hits";
            else if (decadaBase == 2010) terminoEpoca = "2010s hits";
            else if (decadaBase == 2020) terminoEpoca = "2020 hits";

            var playlists = db.Playlists
                .Where(p => p.IdUsuario == idUsuario && p.Activo == true)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(8)
                .ToList();

            var favoritos = db.Favoritos
                .Where(f => f.IdUsuario == idUsuario)
                .Include(f => f.Canciones.Artistas)
                .OrderByDescending(f => f.FechaAgregado)
                .Take(8)
                .ToList();

            ViewBag.Usuario = usuario;
            ViewBag.AnioBase = anioBase;
            ViewBag.DecadaBase = decadaBase;
            ViewBag.TerminoEpoca = terminoEpoca;
            ViewBag.Playlists = playlists;
            ViewBag.Favoritos = favoritos;

            return View();
        }
    }
}