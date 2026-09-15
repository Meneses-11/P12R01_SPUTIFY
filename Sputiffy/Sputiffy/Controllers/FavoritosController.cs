using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class FavoritosController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: Favoritos
        public ActionResult Index()
        {
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Acceso");

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            if (usuario == null)
                return RedirectToAction("Login", "Acceso");

            var favoritos = db.Favoritos
                .Where(f => f.IdUsuario == idUsuario)
                .Include(f => f.Canciones.Artistas)
                .OrderByDescending(f => f.FechaAgregado)
                .ToList();

            var playlists = db.Playlists
                .Where(p => p.IdUsuario == idUsuario && p.Activo == true)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(8)
                .ToList();

            ViewBag.Usuario = usuario;
            ViewBag.Playlists = playlists;

            return View(favoritos);
        }

        // GET: Favoritos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Favoritos favoritos = db.Favoritos.Find(id);
            if (favoritos == null)
                return HttpNotFound();

            return View(favoritos);
        }

        // GET: Favoritos/Create
        public ActionResult Create()
        {
            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: Favoritos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFavorito,IdUsuario,IdCancion,FechaAgregado")] Favoritos favoritos)
        {
            if (ModelState.IsValid)
            {
                db.Favoritos.Add(favoritos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", favoritos.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", favoritos.IdUsuario);
            return View(favoritos);
        }

        // GET: Favoritos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Favoritos favoritos = db.Favoritos.Find(id);
            if (favoritos == null)
                return HttpNotFound();

            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", favoritos.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", favoritos.IdUsuario);
            return View(favoritos);
        }

        // POST: Favoritos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFavorito,IdUsuario,IdCancion,FechaAgregado")] Favoritos favoritos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favoritos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCancion = new SelectList(db.Canciones, "IdCancion", "Titulo", favoritos.IdCancion);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", favoritos.IdUsuario);
            return View(favoritos);
        }

        // GET: Favoritos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Favoritos favoritos = db.Favoritos.Find(id);
            if (favoritos == null)
                return HttpNotFound();

            return View(favoritos);
        }

        // POST: Favoritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favoritos favoritos = db.Favoritos.Find(id);
            db.Favoritos.Remove(favoritos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ============================
        // JSON PARA SPUTIFFY
        // ============================

        [HttpPost]
        public JsonResult Agregar(string videoId, string titulo, string artista, string portada)
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." });

                if (string.IsNullOrWhiteSpace(titulo))
                    return Json(new { ok = false, mensaje = "La canción no tiene título." });

                var cancion = ObtenerOCrearCancionYoutube(videoId, titulo, artista, portada);
                if (cancion == null)
                    return Json(new { ok = false, mensaje = "No se pudo guardar la canción." });

                bool existe = db.Favoritos.Any(x => x.IdUsuario == idUsuario && x.IdCancion == cancion.IdCancion);
                if (existe)
                    return Json(new { ok = false, mensaje = "La canción ya está en favoritos." });

                var favorito = new Favoritos
                {
                    IdUsuario = idUsuario,
                    IdCancion = cancion.IdCancion,
                    FechaAgregado = DateTime.Now
                };

                db.Favoritos.Add(favorito);
                db.SaveChanges();

                return Json(new { ok = true, mensaje = "Agregado a favoritos." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult QuitarAjax(int idFavorito)
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." });

                var favorito = db.Favoritos.FirstOrDefault(f =>
                    f.IdFavorito == idFavorito && f.IdUsuario == idUsuario);

                if (favorito == null)
                    return Json(new { ok = false, mensaje = "No se encontró ese favorito." });

                db.Favoritos.Remove(favorito);
                db.SaveChanges();

                return Json(new { ok = true, mensaje = "Eliminado de favoritos." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
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

        private Canciones ObtenerOCrearCancionYoutube(string videoId, string titulo, string artista, string portada)
        {
            titulo = (titulo ?? "").Trim();
            artista = (artista ?? "").Trim();
            videoId = (videoId ?? "").Trim();

            // 1) Si ya guardamos este video de YouTube antes, reutilizamos la misma canción
            //    (evita duplicados cada vez que alguien le da "me gusta" al mismo video).
            Canciones cancion = null;

            if (!string.IsNullOrWhiteSpace(videoId))
            {
                cancion = db.Canciones.FirstOrDefault(x =>
                    x.FuenteApi == "YouTube" && x.IdExternoApi == videoId);
            }

            // 2) Si no vino videoId (o no se encontró), buscamos por título + artista.
            if (cancion == null)
            {
                cancion = db.Canciones.FirstOrDefault(x =>
                    x.Titulo == titulo && x.Artistas.NombreArtistico == artista);
            }

            if (cancion != null)
                return cancion;

            // Las propiedades reales del modelo Canciones son:
            // Titulo, DuracionSegundos, AnioLanzamiento, PreviewUrl, AudioUrl,
            // ImagenUrl, FuenteApi, IdExternoApi, IdArtista, IdAlbum, IdGenero,
            // Reproducciones, EsExplIcita, Activo.
            // (No existen "Artista", "Portada", "VideoId", "YoutubeId", etc. -
            // por eso el guardado fallaba: IdArtista quedaba en 0 y violaba la
            // llave foránea hacia Artistas).
            cancion = new Canciones
            {
                Titulo = titulo,
                IdArtista = ObtenerOCrearArtista(artista).IdArtista,
                AnioLanzamiento = DateTime.Now.Year,
                ImagenUrl = portada,
                FuenteApi = "YouTube",
                IdExternoApi = videoId,
                PreviewUrl = "",
                Reproducciones = 0,
                EsExplIcita = false,
                Activo = true
            };

            db.Canciones.Add(cancion);
            db.SaveChanges();

            return cancion;
        }

        private Artistas ObtenerOCrearArtista(string nombreArtistico)
        {
            nombreArtistico = string.IsNullOrWhiteSpace(nombreArtistico) ? "Desconocido" : nombreArtistico.Trim();

            var artista = db.Artistas.FirstOrDefault(x => x.NombreArtistico == nombreArtistico);
            if (artista != null)
                return artista;

            artista = new Artistas
            {
                NombreArtistico = nombreArtistico,
                Activo = true
            };

            db.Artistas.Add(artista);
            db.SaveChanges();

            return artista;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}