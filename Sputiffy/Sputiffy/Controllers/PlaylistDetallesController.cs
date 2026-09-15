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
    public class PlaylistDetallesController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: PlaylistDetalles
        public ActionResult Index()
        {
            var playlistDetalle = db.PlaylistDetalle.Include(p => p.Canciones).Include(p => p.Playlists);
            return View(playlistDetalle.ToList());
        }

        // GET: PlaylistDetalles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaylistDetalle playlistDetalle = db.PlaylistDetalle.Find(id);
            if (playlistDetalle == null)
            {
                return HttpNotFound();
            }
            return View(playlistDetalle);
        }

        // GET: PlaylistDetalles/Create
        public ActionResult Create()
        {
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo");
            ViewBag.IdPlaylist = new SelectList(db.Playlists, "IdPlaylist", "NombrePlaylist");
            return View();
        }

        // POST: PlaylistDetalles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPlaylistDetalle,IdPlaylist,IdCancion,FechaAgregado,OrdenCancion")] PlaylistDetalle playlistDetalle)
        {
            if (ModelState.IsValid)
            {
                db.PlaylistDetalle.Add(playlistDetalle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", playlistDetalle.IdCancion);
            ViewBag.IdPlaylist = new SelectList(db.Playlists, "IdPlaylist", "NombrePlaylist", playlistDetalle.IdPlaylist);
            return View(playlistDetalle);
        }

        // GET: PlaylistDetalles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaylistDetalle playlistDetalle = db.PlaylistDetalle.Find(id);
            if (playlistDetalle == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", playlistDetalle.IdCancion);
            ViewBag.IdPlaylist = new SelectList(db.Playlists, "IdPlaylist", "NombrePlaylist", playlistDetalle.IdPlaylist);
            return View(playlistDetalle);
        }

        // POST: PlaylistDetalles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPlaylistDetalle,IdPlaylist,IdCancion,FechaAgregado,OrdenCancion")] PlaylistDetalle playlistDetalle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playlistDetalle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", playlistDetalle.IdCancion);
            ViewBag.IdPlaylist = new SelectList(db.Playlists, "IdPlaylist", "NombrePlaylist", playlistDetalle.IdPlaylist);
            return View(playlistDetalle);
        }

        // GET: PlaylistDetalles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaylistDetalle playlistDetalle = db.PlaylistDetalle.Find(id);
            if (playlistDetalle == null)
            {
                return HttpNotFound();
            }
            return View(playlistDetalle);
        }

        // POST: PlaylistDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaylistDetalle playlistDetalle = db.PlaylistDetalle.Find(id);
            db.PlaylistDetalle.Remove(playlistDetalle);
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
