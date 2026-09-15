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
    public class CancionesController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: Canciones
        public ActionResult Index()
        {
            var canciones = db.Canciones.Include(c => c.Albumes).Include(c => c.Artistas).Include(c => c.Generos);
            return View(canciones.ToList());
        }

        // GET: Canciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canciones canciones = db.Canciones.Find(id);
            if (canciones == null)
            {
                return HttpNotFound();
            }
            return View(canciones);
        }

        // GET: Canciones/Create
        public ActionResult Create()
        {
            ViewBag.IdAlbum = new SelectList(db.Albumes, "IdAlbum", "Titulo");
            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico");
            ViewBag.IdGenero = new SelectList(db.Generos, "IdGenero", "NombreGenero");
            return View();
        }

        // POST: Canciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCancion,Titulo,DuracionSegundos,AnioLanzamiento,PreviewUrl,AudioUrl,ImagenUrl,FuenteApi,IdExternoApi,IdArtista,IdAlbum,IdGenero,Reproducciones,EsExplIcita,Activo")] Canciones canciones)
        {
            if (ModelState.IsValid)
            {
                db.Canciones.Add(canciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAlbum = new SelectList(db.Albumes, "IdAlbum", "Titulo", canciones.IdAlbum);
            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico", canciones.IdArtista);
            ViewBag.IdGenero = new SelectList(db.Generos, "IdGenero", "NombreGenero", canciones.IdGenero);
            return View(canciones);
        }

        // GET: Canciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canciones canciones = db.Canciones.Find(id);
            if (canciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAlbum = new SelectList(db.Albumes, "IdAlbum", "Titulo", canciones.IdAlbum);
            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico", canciones.IdArtista);
            ViewBag.IdGenero = new SelectList(db.Generos, "IdGenero", "NombreGenero", canciones.IdGenero);
            return View(canciones);
        }

        // POST: Canciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCancion,Titulo,DuracionSegundos,AnioLanzamiento,PreviewUrl,AudioUrl,ImagenUrl,FuenteApi,IdExternoApi,IdArtista,IdAlbum,IdGenero,Reproducciones,EsExplIcita,Activo")] Canciones canciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(canciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAlbum = new SelectList(db.Albumes, "IdAlbum", "Titulo", canciones.IdAlbum);
            ViewBag.IdArtista = new SelectList(db.Artistas, "IdArtista", "NombreArtistico", canciones.IdArtista);
            ViewBag.IdGenero = new SelectList(db.Generos, "IdGenero", "NombreGenero", canciones.IdGenero);
            return View(canciones);
        }

        // GET: Canciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canciones canciones = db.Canciones.Find(id);
            if (canciones == null)
            {
                return HttpNotFound();
            }
            return View(canciones);
        }

        // POST: Canciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Canciones canciones = db.Canciones.Find(id);
            db.Canciones.Remove(canciones);
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
