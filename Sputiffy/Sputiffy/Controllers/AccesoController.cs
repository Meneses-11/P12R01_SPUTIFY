using System;
using System.Linq;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class AccesoController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        public ActionResult Login()
        {
            if (Session["IdUsuario"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
            {
                ViewBag.Error = "Debes capturar correo y contraseña.";
                return View();
            }

            var usuario = db.Usuarios
                .FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena && u.Activo == true);

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            Session["IdUsuario"] = usuario.IdUsuario;
            Session["NombreUsuario"] = usuario.Nombre;
            Session["Correo"] = usuario.Correo;
            Session["FechaNacimiento"] = usuario.FechaNacimiento;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Registro()
        {
            ViewBag.IdRol = new SelectList(db.Roles.Where(r => r.Activo == true).ToList(), "IdRol", "NombreRol");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(Usuarios model)
        {
            ViewBag.IdRol = new SelectList(db.Roles.Where(r => r.Activo == true).ToList(), "IdRol", "NombreRol", model.IdRol);

            if (!ModelState.IsValid)
                return View(model);

            var existeCorreo = db.Usuarios.Any(u => u.Correo == model.Correo);
            if (existeCorreo)
            {
                ViewBag.Error = "Ese correo ya está registrado.";
                return View(model);
            }

            var existeUsuario = db.Usuarios.Any(u => u.NombreUsuario == model.NombreUsuario);
            if (existeUsuario)
            {
                ViewBag.Error = "Ese nombre de usuario ya existe.";
                return View(model);
            }

            model.FechaRegistro = DateTime.Now;
            model.Activo = true;

            if (model.IdRol <= 0)
            {
                var rolUsuario = db.Roles.FirstOrDefault(r => r.NombreRol == "Usuario");
                model.IdRol = rolUsuario != null ? rolUsuario.IdRol : 2;
            }

            db.Usuarios.Add(model);
            db.SaveChanges();

            Session["IdUsuario"] = model.IdUsuario;
            Session["NombreUsuario"] = model.Nombre;
            Session["Correo"] = model.Correo;
            Session["FechaNacimiento"] = model.FechaNacimiento;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}