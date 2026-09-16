//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Sputiffy.Controllers
//{
//    public class AudiolibrosController
//    {
//    }
//}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using Newtonsoft.Json.Linq;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class AudiolibrosController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // Clave API de YouTube (misma que usa PodcastController)
        private static readonly string YoutubeApiKey =
            System.Configuration.ConfigurationManager.AppSettings["YoutubeApiKey"];

        // ============================================
        // GET: Audiolibros  →  "Mis audiolibros"
        // ============================================
        public ActionResult Index(string searchString = null)
        {
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Acceso");

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            if (usuario == null)
                return RedirectToAction("Login", "Acceso");

            var query = db.AudiolibrosGuardados
                .Where(a => a.IdUsuario == idUsuario && a.Activo);

            // Búsqueda global en "Mis audiolibros"
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var s = searchString.Trim().ToLower();
                query = query.Where(a =>
                    (a.Titulo != null && a.Titulo.ToLower().Contains(s)) ||
                    (a.CanalTitulo != null && a.CanalTitulo.ToLower().Contains(s)));
            }

            var audiolibros = query
                .OrderByDescending(a => a.FechaGuardado)
                .ToList();

            var playlists = db.Playlists
                .Where(p => p.IdUsuario == idUsuario && p.Activo == true)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(8)
                .ToList();

            ViewBag.Usuario = usuario;
            ViewBag.Playlists = playlists;
            ViewBag.SearchString = searchString;

            return View(audiolibros);
        }

        // ============================================
        // POST: Audiolibros/GuardarAjax
        // Guarda un video de YouTube como audiolibro del usuario
        // ============================================
        [HttpPost]
        public JsonResult GuardarAjax(string videoId, string titulo, string canal,
                              string descripcion, string duracionIso,
                              string thumbnailUrl, string thumbnailMediumUrl,
                              string thumbnailHighUrl)
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." });

                if (string.IsNullOrWhiteSpace(videoId))
                    return Json(new { ok = false, mensaje = "VideoId inválido." });

                if (string.IsNullOrWhiteSpace(titulo))
                    return Json(new { ok = false, mensaje = "El video no tiene título." });

                // Convertir duración ISO 8601 (ej: PT1H2M30S) → segundos
                int duracionSeg = 0;
                if (!string.IsNullOrWhiteSpace(duracionIso))
                {
                    try
                    {
                        duracionSeg = (int)XmlConvert.ToTimeSpan(duracionIso).TotalSeconds;
                    }
                    catch { duracionSeg = 0; }
                }

                // Buscamos el registro SIN importar si está activo o no
                var existente = db.AudiolibrosGuardados
                    .FirstOrDefault(a => a.IdUsuario == idUsuario && a.VideoId == videoId);

                if (existente != null)
                {
                    // Ya está activo → no permitimos duplicado
                    if (existente.Activo)
                        return Json(new { ok = false, mensaje = "Este audiolibro ya está guardado." });

                    // Estaba eliminado → lo reactivamos y actualizamos datos
                    existente.Activo = true;
                    existente.Titulo = Truncar(titulo, 200);
                    existente.CanalTitulo = Truncar(canal, 200);
                    existente.Descripcion = Truncar(descripcion, 1000);
                    existente.DuracionSegundos = duracionSeg;
                    existente.ThumbnailUrl = Truncar(thumbnailUrl, 500);
                    existente.ThumbnailMediumUrl = Truncar(thumbnailMediumUrl, 500);
                    existente.ThumbnailHighUrl = Truncar(thumbnailHighUrl, 500);
                    existente.FechaGuardado = DateTime.Now;

                    db.SaveChanges();

                    return Json(new { ok = true, mensaje = "Audiolibro restaurado en tu colección." });
                }

                // No existe → insertamos
                var audiolibro = new AudiolibrosGuardados
                {
                    IdUsuario = idUsuario,
                    VideoId = videoId,
                    Titulo = Truncar(titulo, 200),
                    CanalTitulo = Truncar(canal, 200),
                    Descripcion = Truncar(descripcion, 1000),
                    FechaPublicacion = null,
                    DuracionSegundos = duracionSeg,
                    ThumbnailUrl = Truncar(thumbnailUrl, 500),
                    ThumbnailMediumUrl = Truncar(thumbnailMediumUrl, 500),
                    ThumbnailHighUrl = Truncar(thumbnailHighUrl, 500),
                    TipoContenido = "AUDIOLIBRO",
                    FechaGuardado = DateTime.Now,
                    Activo = true
                };

                db.AudiolibrosGuardados.Add(audiolibro);
                db.SaveChanges();

                return Json(new { ok = true, mensaje = "Audiolibro guardado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
            }
        }

        // ============================================
        // POST: Audiolibros/EliminarAjax
        // Quita un audiolibro guardado (soft delete)
        // ============================================
        [HttpPost]
        public JsonResult EliminarAjax(int idAudiolibro)
        {
            try
            {
                int idUsuario = GetIdUsuarioSesion();
                if (idUsuario <= 0)
                    return Json(new { ok = false, mensaje = "Sesión no iniciada." });

                var audiolibro = db.AudiolibrosGuardados.FirstOrDefault(a =>
                    a.IdAudiolibro == idAudiolibro && a.IdUsuario == idUsuario);

                if (audiolibro == null)
                    return Json(new { ok = false, mensaje = "No se encontró ese audiolibro." });

                audiolibro.Activo = false;
                db.SaveChanges();

                return Json(new { ok = true, mensaje = "Audiolibro eliminado." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
            }
        }

        // ============================================
        // GET: Audiolibros/BuscarYoutubeAjax?q=...
        // Proxy a YouTube Data API v3 (búsqueda + detalles de duración)
        // ============================================
        [HttpGet]
        public JsonResult BuscarYoutubeAjax(string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                    return Json(new { ok = false, mensaje = "Consulta vacía.", items = new object[0] },
                                JsonRequestBehavior.AllowGet);

                if (string.IsNullOrWhiteSpace(YoutubeApiKey))
                    return Json(new { ok = false, mensaje = "Falta configurar YoutubeApiKey en Web.config.", items = new object[0] },
                                JsonRequestBehavior.AllowGet);

                // ---------- Paso 1: search.list ----------
                var searchUrl = "https://www.googleapis.com/youtube/v3/search" +
                                "?part=snippet&type=video&maxResults=20" +
                                "&q=" + Uri.EscapeDataString(q) +
                                "&key=" + YoutubeApiKey;

                string searchJson;
                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    searchJson = wc.DownloadString(searchUrl);
                }

                var searchParsed = JObject.Parse(searchJson);
                var searchItems = searchParsed["items"] as JArray ?? new JArray();

                // Recopilar los videoIds encontrados
                var videoIds = new List<string>();
                foreach (var it in searchItems)
                {
                    var vid = (string)it["id"]?["videoId"];
                    if (!string.IsNullOrWhiteSpace(vid)) videoIds.Add(vid);
                }

                // ---------- Paso 2: videos.list (para obtener contentDetails.duration) ----------
                var duraciones = new Dictionary<string, string>();
                if (videoIds.Count > 0)
                {
                    var detailsUrl = "https://www.googleapis.com/youtube/v3/videos" +
                                     "?part=contentDetails" +
                                     "&id=" + Uri.EscapeDataString(string.Join(",", videoIds)) +
                                     "&key=" + YoutubeApiKey;

                    string detailsJson;
                    using (var wc = new WebClient())
                    {
                        wc.Encoding = System.Text.Encoding.UTF8;
                        detailsJson = wc.DownloadString(detailsUrl);
                    }

                    var detailsParsed = JObject.Parse(detailsJson);
                    foreach (var it in detailsParsed["items"] as JArray ?? new JArray())
                    {
                        var vid = (string)it["id"];
                        var dur = (string)it["contentDetails"]?["duration"];
                        if (!string.IsNullOrWhiteSpace(vid) && !string.IsNullOrWhiteSpace(dur))
                            duraciones[vid] = dur;
                    }
                }

                // ---------- Armar la respuesta ----------
                var items = new List<object>();

                foreach (var it in searchItems)
                {
                    var snippet = it["snippet"];
                    var videoId = (string)it["id"]?["videoId"];
                    if (string.IsNullOrWhiteSpace(videoId)) continue;

                    var thumbs = snippet["thumbnails"];

                    items.Add(new
                    {
                        videoId = videoId,
                        titulo = (string)snippet["title"],
                        canal = (string)snippet["channelTitle"],
                        descripcion = (string)snippet["description"],
                        publicado = (string)snippet["publishedAt"],
                        duracionIso = duraciones.ContainsKey(videoId) ? duraciones[videoId] : "",
                        thumbnailUrl = (string)thumbs?["default"]?["url"],
                        thumbnailMediumUrl = (string)thumbs?["medium"]?["url"],
                        thumbnailHighUrl = (string)thumbs?["high"]?["url"]
                    });
                }

                return Json(new { ok = true, items = items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message, items = new object[0] },
                            JsonRequestBehavior.AllowGet);
            }
        }

        // ============================================
        // Helpers
        // ============================================
        private int GetIdUsuarioSesion()
        {
            if (Session["IdUsuario"] != null)
                return Convert.ToInt32(Session["IdUsuario"]);

            if (Session["IDUSUARIO"] != null)
                return Convert.ToInt32(Session["IDUSUARIO"]);

            return 0;
        }

        private static string Truncar(string texto, int max)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            return texto.Length <= max ? texto : texto.Substring(0, max);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}