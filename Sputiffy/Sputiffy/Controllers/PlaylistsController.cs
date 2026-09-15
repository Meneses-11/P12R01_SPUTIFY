using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class PlaylistsController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: Playlists
        public ActionResult Index()
        {
            var playlists = db.Playlists.Include(p => p.Usuarios);
            return View(playlists.ToList());
        }

        // GET: Playlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var playlist = db.Playlists
                .Include(x => x.Usuarios)
                .FirstOrDefault(x => x.IdPlaylist == id);

            if (playlist == null)
                return HttpNotFound();

            var canciones = db.PlaylistCanciones
                .Where(x => x.IdPlaylist == playlist.IdPlaylist)
                .OrderBy(x => x.FechaAgregado)
                .ToList();

            ViewBag.CancionesPlaylist = canciones;
            ViewBag.TotalCanciones = canciones.Count;

            return View(playlist);
        }

        [HttpPost]
        public JsonResult EliminarCancionAjax(int idPlaylistCancion)
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." });

                var item = db.PlaylistCanciones.FirstOrDefault(x => x.IdPlaylistCancion == idPlaylistCancion);
                if (item == null)
                    return Json(new { ok = false, mensaje = "La canción no existe." });

                var playlist = db.Playlists.FirstOrDefault(x => x.IdPlaylist == item.IdPlaylist && x.IdUsuario == idUsuario);
                if (playlist == null)
                    return Json(new { ok = false, mensaje = "No tienes permiso para modificar esta playlist." });

                db.PlaylistCanciones.Remove(item);
                db.SaveChanges();

                return Json(new { ok = true, mensaje = "Canción eliminada de la playlist." });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    mensaje = ex.Message,
                    detalle = ex.InnerException != null ? ex.InnerException.Message : ""
                });
            }
        }

        // GET: Playlists/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: Playlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPlaylist,NombrePlaylist,Descripcion,FechaCreacion,Publica,IdUsuario,Activo")] Playlists playlists)
        {
            if (ModelState.IsValid)
            {
                db.Playlists.Add(playlists);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", playlists.IdUsuario);
            return View(playlists);
        }

        // GET: Playlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Playlists playlists = db.Playlists.Find(id);
            if (playlists == null)
                return HttpNotFound();

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", playlists.IdUsuario);
            return View(playlists);
        }

        // POST: Playlists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPlaylist,NombrePlaylist,Descripcion,FechaCreacion,Publica,IdUsuario,Activo")] Playlists playlists)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playlists).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", playlists.IdUsuario);
            return View(playlists);
        }

        // GET: Playlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Playlists playlists = db.Playlists.Find(id);
            if (playlists == null)
                return HttpNotFound();

            return View(playlists);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Playlists playlists = db.Playlists.Find(id);
            db.Playlists.Remove(playlists);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ============================
        // JSON PARA SPUTIFFY
        // ============================

        [HttpPost]
        public JsonResult CrearPlaylistAjax(string nombre)
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." });

                if (string.IsNullOrWhiteSpace(nombre))
                    return Json(new { ok = false, mensaje = "Escribe un nombre para la playlist." });

                var nueva = new Playlists
                {
                    NombrePlaylist = nombre.Trim(),
                    Descripcion = "",
                    FechaCreacion = DateTime.Now,
                    Publica = false,
                    IdUsuario = idUsuario,
                    Activo = true
                };

                db.Playlists.Add(nueva);
                db.SaveChanges();

                return Json(new
                {
                    ok = true,
                    mensaje = "Playlist creada correctamente.",
                    idPlaylist = nueva.IdPlaylist,
                    nombre = nueva.NombrePlaylist
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    mensaje = ex.Message,
                    detalle = ex.InnerException != null ? ex.InnerException.Message : ""
                });
            }
        }

        [HttpGet]
        public JsonResult MisPlaylists()
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." }, JsonRequestBehavior.AllowGet);

                var listas = db.Playlists
                    .Where(x => x.IdUsuario == idUsuario && (x.Activo == null || x.Activo == true))
                    .OrderBy(x => x.NombrePlaylist)
                    .Select(x => new
                    {
                        id = x.IdPlaylist,
                        nombre = x.NombrePlaylist
                    })
                    .ToList();

                return Json(new { ok = true, playlists = listas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    mensaje = ex.Message,
                    detalle = ex.InnerException != null ? ex.InnerException.Message : ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AgregarCancionAjax(int idPlaylist, string videoId, string titulo, string artista, string portada)
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." });

                var playlist = db.Playlists.FirstOrDefault(x => x.IdPlaylist == idPlaylist && x.IdUsuario == idUsuario);
                if (playlist == null)
                    return Json(new { ok = false, mensaje = "La playlist no existe o no te pertenece." });

                if (string.IsNullOrWhiteSpace(videoId) || string.IsNullOrWhiteSpace(titulo))
                    return Json(new { ok = false, mensaje = "Datos de canción incompletos." });

                bool existe = db.PlaylistCanciones.Any(x => x.IdPlaylist == idPlaylist && x.VideoId == videoId);
                if (existe)
                    return Json(new { ok = false, mensaje = "La canción ya está en esa playlist." });

                var item = new PlaylistCanciones
                {
                    IdPlaylist = idPlaylist,
                    VideoId = videoId,
                    Titulo = titulo,
                    Artista = artista,
                    Portada = portada,
                    FechaAgregado = DateTime.Now
                };

                db.PlaylistCanciones.Add(item);
                db.SaveChanges();

                return Json(new { ok = true, mensaje = "Canción agregada a la playlist." });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    mensaje = ex.Message,
                    detalle = ex.InnerException != null ? ex.InnerException.Message : ""
                });
            }
        }

        private int GetIdUsuarioSesion()
        {
            if (Session["IdUsuario"] != null)
                return Convert.ToInt32(Session["IdUsuario"]);

            if (Session["IDUSUARIO"] != null)
                return Convert.ToInt32(Session["IDUSUARIO"]);

            return 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}