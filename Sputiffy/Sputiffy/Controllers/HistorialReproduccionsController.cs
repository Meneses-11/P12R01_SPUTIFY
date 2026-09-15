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
    public class HistorialReproduccionsController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: HistorialReproduccions
        public ActionResult Index()
        {
            var historialReproduccion = db.HistorialReproduccion.Include(h => h.Canciones).Include(h => h.Usuarios);
            return View(historialReproduccion.ToList());
        }

        // GET: HistorialReproduccions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialReproduccion historialReproduccion = db.HistorialReproduccion.Find(id);
            if (historialReproduccion == null)
            {
                return HttpNotFound();
            }
            return View(historialReproduccion);
        }

        // GET: HistorialReproduccions/Create
        public ActionResult Create()
        {
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: HistorialReproduccions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdHistorial,IdUsuario,IdCancion,FechaReproduccion,SegundosEscuchados,Completa")] HistorialReproduccion historialReproduccion)
        {
            if (ModelState.IsValid)
            {
                db.HistorialReproduccion.Add(historialReproduccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", historialReproduccion.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", historialReproduccion.IdUsuario);
            return View(historialReproduccion);
        }

        // GET: HistorialReproduccions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialReproduccion historialReproduccion = db.HistorialReproduccion.Find(id);
            if (historialReproduccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", historialReproduccion.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", historialReproduccion.IdUsuario);
            return View(historialReproduccion);
        }

        // POST: HistorialReproduccions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdHistorial,IdUsuario,IdCancion,FechaReproduccion,SegundosEscuchados,Completa")] HistorialReproduccion historialReproduccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historialReproduccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", historialReproduccion.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", historialReproduccion.IdUsuario);
            return View(historialReproduccion);
        }

        // GET: HistorialReproduccions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialReproduccion historialReproduccion = db.HistorialReproduccion.Find(id);
            if (historialReproduccion == null)
            {
                return HttpNotFound();
            }
            return View(historialReproduccion);
        }

        // POST: HistorialReproduccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistorialReproduccion historialReproduccion = db.HistorialReproduccion.Find(id);
            db.HistorialReproduccion.Remove(historialReproduccion);
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
