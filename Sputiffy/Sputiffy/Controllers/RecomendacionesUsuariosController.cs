using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class RecomendacionesUsuariosController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: RecomendacionesUsuarios
        public ActionResult Index()
        {
            var recomendacionesUsuario = db.RecomendacionesUsuario.Include(r => r.Canciones).Include(r => r.Usuarios);
            return View(recomendacionesUsuario.ToList());
        }

        // GET: RecomendacionesUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecomendacionesUsuario recomendacionesUsuario = db.RecomendacionesUsuario.Find(id);
            if (recomendacionesUsuario == null)
            {
                return HttpNotFound();
            }
            return View(recomendacionesUsuario);
        }

        // GET: RecomendacionesUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: RecomendacionesUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRecomendacion,IdUsuario,IdCancion,Motivo,Puntaje,FechaGenerada,Mostrada")] RecomendacionesUsuario recomendacionesUsuario)
        {
            if (ModelState.IsValid)
            {
                db.RecomendacionesUsuario.Add(recomendacionesUsuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", recomendacionesUsuario.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", recomendacionesUsuario.IdUsuario);
            return View(recomendacionesUsuario);
        }

        // GET: RecomendacionesUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecomendacionesUsuario recomendacionesUsuario = db.RecomendacionesUsuario.Find(id);
            if (recomendacionesUsuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", recomendacionesUsuario.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", recomendacionesUsuario.IdUsuario);
            return View(recomendacionesUsuario);
        }

        // POST: RecomendacionesUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRecomendacion,IdUsuario,IdCancion,Motivo,Puntaje,FechaGenerada,Mostrada")] RecomendacionesUsuario recomendacionesUsuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recomendacionesUsuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", recomendacionesUsuario.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", recomendacionesUsuario.IdUsuario);
            return View(recomendacionesUsuario);
        }

        // GET: RecomendacionesUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecomendacionesUsuario recomendacionesUsuario = db.RecomendacionesUsuario.Find(id);
            if (recomendacionesUsuario == null)
            {
                return HttpNotFound();
            }
            return View(recomendacionesUsuario);
        }

        // POST: RecomendacionesUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecomendacionesUsuario recomendacionesUsuario = db.RecomendacionesUsuario.Find(id);
            db.RecomendacionesUsuario.Remove(recomendacionesUsuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
