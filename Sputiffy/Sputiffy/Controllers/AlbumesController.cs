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
    public class AlbumesController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: Albumes
        public ActionResult Index()
        {
            var albumes = db.Albumes.Include(a => a.Artistas);
            return View(albumes.ToList());
        }

        // GET: Albumes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albumes albumes = db.Albumes.Find(id);
            if (albumes == null)
            {
                return HttpNotFound();
            }
            return View(albumes);
        }

        // GET: Albumes/Create
        public ActionResult Create()
        {
            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico");
            return View();
        }

        // POST: Albumes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAlbum,Titulo,FechaLanzamiento,AnioLanzamiento,PortadaUrl,IdArtista,Activo")] Albumes albumes)
        {
            if (ModelState.IsValid)
            {
                db.Albumes.Add(albumes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico", albumes.IdArtista);
            return View(albumes);
        }

        // GET: Albumes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albumes albumes = db.Albumes.Find(id);
            if (albumes == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico", albumes.IdArtista);
            return View(albumes);
        }

        // POST: Albumes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAlbum,Titulo,FechaLanzamiento,AnioLanzamiento,PortadaUrl,IdArtista,Activo")] Albumes albumes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albumes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico", albumes.IdArtista);
            return View(albumes);
        }

        // GET: Albumes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albumes albumes = db.Albumes.Find(id);
            if (albumes == null)
            {
                return HttpNotFound();
            }
            return View(albumes);
        }

        // POST: Albumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Albumes albumes = db.Albumes.Find(id);
            db.Albumes.Remove(albumes);
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
