using Newtonsoft.Json.Linq;
using Sputiffy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sputiffy.Controllers
{
    public class NarracionesDeportivasController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // =============================================
        // GET: /NarracionesDeportivas
        // =============================================
        public ActionResult Index(string searchString)
        {
            // Validar sesión
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Usuarios");

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            // Usuario actual
            var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            if (usuario == null)
                return RedirectToAction("Login", "Usuarios");

            // Playlists del usuario (para el sidebar)
            var playlists = db.Playlists
                .Where(p => p.IdUsuario == idUsuario && p.Activo)
                .OrderByDescending(p => p.FechaCreacion)
                .ToList();

            // Narraciones del usuario (con filtro de búsqueda)
            var query = db.NarracionesDeportivas
                .Where(n => n.IdUsuario == idUsuario && n.Activo);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(n =>
                    n.Titulo.Contains(searchString) ||
                    n.Deporte.Contains(searchString) ||
                    n.EquipoLocal.Contains(searchString) ||
                    n.EquipoVisitante.Contains(searchString) ||
                    n.Evento.Contains(searchString));
            }

            var narraciones = query
                .OrderByDescending(n => n.FechaRegistro)
                .ToList();

            ViewBag.Usuario = usuario;
            ViewBag.Playlists = playlists;
            ViewBag.SearchString = searchString ?? "";

            return View(narraciones);
        }

        // =============================================
        // GET: /NarracionesDeportivas/BuscarYoutubeAjax?q=...
        // =============================================
        [HttpGet]
        public async Task<JsonResult> BuscarYoutubeAjax(string q)
        {
            if (Session["IdUsuario"] == null)
                return Json(new { ok = false, mensaje = "Sesión expirada." },
                            JsonRequestBehavior.AllowGet);

            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return Json(new { ok = false, mensaje = "Escribe al menos 2 caracteres." },
                            JsonRequestBehavior.AllowGet);

            try
            {
                string apiKey = ConfigurationManager.AppSettings["YouTubeApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    return Json(new { ok = false, mensaje = "Falta la API Key de YouTube en Web.config." },
                                JsonRequestBehavior.AllowGet);

                // Forzamos a que busque narraciones deportivas
                string termino = q.Trim() + " narración deportiva";

                string url = "https://www.googleapis.com/youtube/v3/search" +
                             "?part=snippet&type=video&maxResults=12" +
                             "&q=" + HttpUtility.UrlEncode(termino) +
                             "&key=" + apiKey;

                using (var client = new HttpClient())
                {
                    var resp = await client.GetStringAsync(url);
                    var json = JObject.Parse(resp);
                    var items = new List<object>();

                    foreach (var it in json["items"] ?? new JArray())
                    {
                        var sn = it["snippet"];
                        var id = it["id"]?["videoId"]?.ToString();
                        if (string.IsNullOrEmpty(id)) continue;

                        items.Add(new
                        {
                            videoId = id,
                            titulo = sn["title"]?.ToString(),
                            canal = sn["channelTitle"]?.ToString(),
                            descripcion = sn["description"]?.ToString(),
                            fechaPublicacion = sn["publishedAt"]?.ToString(),
                            thumbnailUrl = sn["thumbnails"]?["default"]?["url"]?.ToString(),
                            thumbnailMediumUrl = sn["thumbnails"]?["medium"]?["url"]?.ToString(),
                            thumbnailHighUrl = sn["thumbnails"]?["high"]?["url"]?.ToString(),
                            urlYoutube = "https://www.youtube.com/watch?v=" + id
                        });
                    }

                    return Json(new { ok = true, items }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = "Error al consultar YouTube: " + ex.Message },
                            JsonRequestBehavior.AllowGet);
            }
        }

        // =============================================
        // POST: /NarracionesDeportivas/GuardarAjax
        // =============================================
        /*[HttpPost]
        public JsonResult GuardarAjax(
            string videoId, string titulo, string canal = null, string descripcion = null,
            string thumbnailUrl = null, string thumbnailMediumUrl = null, string thumbnailHighUrl = null,
            string urlYoutube = null, int? duracionSegundos = null, string fechaPublicacion = null,
            string deporte = null, string equipoLocal = null, string equipoVisitante = null,
            string evento = null, string fechaEvento = null, string marcador = null)
        {
            if (Session["IdUsuario"] == null)
                return Json(new { ok = false, mensaje = "Sesión expirada." });

            if (string.IsNullOrWhiteSpace(videoId) || string.IsNullOrWhiteSpace(titulo))
                return Json(new { ok = false, mensaje = "Datos incompletos." });

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            // Evitar duplicado
            bool yaExiste = db.NarracionesDeportivas.Any(n =>
                n.IdUsuario == idUsuario && n.VideoId == videoId && n.Activo);

            if (yaExiste)
                return Json(new { ok = false, mensaje = "Ya tienes esta narración guardada." });

            try
            {
                DateTime? fPub = null;
                if (!string.IsNullOrWhiteSpace(fechaPublicacion) &&
                    DateTime.TryParse(fechaPublicacion, out DateTime tmpPub))
                    fPub = tmpPub;

                DateTime? fEv = null;
                if (!string.IsNullOrWhiteSpace(fechaEvento) &&
                    DateTime.TryParse(fechaEvento, out DateTime tmpEv))
                    fEv = tmpEv;

                var nueva = new NarracionesDeportivas
                {
                    VideoId = videoId,
                    Titulo = titulo.Length > 300 ? titulo.Substring(0, 300) : titulo,
                    Descripcion = Truncar(descripcion, 1000),
                    CanalYoutube = Truncar(canal, 200),
                    ThumbnailUrl = Truncar(thumbnailMediumUrl ?? thumbnailUrl ?? thumbnailHighUrl, 500),
                    UrlYoutube = Truncar(urlYoutube ?? ("https://www.youtube.com/watch?v=" + videoId), 500),
                    DuracionSegundos = duracionSegundos,
                    FechaPublicacion = fPub,
                    Deporte = Truncar(deporte, 80),
                    EquipoLocal = Truncar(equipoLocal, 150),
                    EquipoVisitante = Truncar(equipoVisitante, 150),
                    Evento = Truncar(evento, 200),
                    FechaEvento = fEv,
                    Marcador = Truncar(marcador, 50),
                    IdUsuario = idUsuario,
                    EsFavorito = false,
                    FechaRegistro = DateTime.Now,
                    Activo = true
                };

                db.NarracionesDeportivas.Add(nueva);
                db.SaveChanges();

                return Json(new
                {
                    ok = true,
                    mensaje = "Narración guardada correctamente.",
                    idNarracion = nueva.IdNarracion
                });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = "Error al guardar: " + ex.Message });
            }
        }
        */

        [HttpPost]
        public JsonResult GuardarAjax(
    string videoId, string titulo, string canal = null, string descripcion = null,
    string thumbnailUrl = null, string thumbnailMediumUrl = null, string thumbnailHighUrl = null,
    string urlYoutube = null, int? duracionSegundos = null, string fechaPublicacion = null,
    string deporte = null, string equipoLocal = null, string equipoVisitante = null,
    string evento = null, string fechaEvento = null, string marcador = null)
        {
            if (Session["IdUsuario"] == null)
                return Json(new { ok = false, mensaje = "Sesión expirada." });

            if (string.IsNullOrWhiteSpace(videoId) || string.IsNullOrWhiteSpace(titulo))
                return Json(new { ok = false, mensaje = "Datos incompletos." });

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            try
            {
                DateTime? fPub = null;
                if (!string.IsNullOrWhiteSpace(fechaPublicacion) &&
                    DateTime.TryParse(fechaPublicacion, out DateTime tmpPub))
                    fPub = tmpPub;

                DateTime? fEv = null;
                if (!string.IsNullOrWhiteSpace(fechaEvento) &&
                    DateTime.TryParse(fechaEvento, out DateTime tmpEv))
                    fEv = tmpEv;

                string thumbFinal = !string.IsNullOrWhiteSpace(thumbnailMediumUrl)
                    ? thumbnailMediumUrl
                    : (!string.IsNullOrWhiteSpace(thumbnailUrl) ? thumbnailUrl : thumbnailHighUrl);

                string urlFinal = !string.IsNullOrWhiteSpace(urlYoutube)
                    ? urlYoutube
                    : "https://www.youtube.com/watch?v=" + videoId;

                using (var conn = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_Narraciones_Guardar", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@VideoId", videoId);
                        cmd.Parameters.AddWithValue("@Titulo", Truncar(titulo, 300));
                        cmd.Parameters.AddWithValue("@Descripcion", (object)Truncar(descripcion, 1000) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CanalYoutube", (object)Truncar(canal, 200) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ThumbnailUrl", (object)Truncar(thumbFinal, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@UrlYoutube", (object)Truncar(urlFinal, 500) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DuracionSegundos", (object)duracionSegundos ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaPublicacion", (object)fPub ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Deporte", (object)Truncar(deporte, 80) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EquipoLocal", (object)Truncar(equipoLocal, 150) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EquipoVisitante", (object)Truncar(equipoVisitante, 150) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Evento", (object)Truncar(evento, 200) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaEvento", (object)fEv ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Marcador", (object)Truncar(marcador, 50) ?? DBNull.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int ok = Convert.ToInt32(reader["ok"]);
                                string mensaje = reader["mensaje"].ToString();
                                int idNarracion = reader["IdNarracion"] != DBNull.Value
                                    ? Convert.ToInt32(reader["IdNarracion"]) : 0;

                                return Json(new { ok = ok == 1, mensaje, idNarracion });
                            }
                        }
                    }
                }

                return Json(new { ok = false, mensaje = "No se pudo guardar." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = "Error al guardar: " + ex.Message });
            }
        }

        // =============================================
        // POST: /NarracionesDeportivas/EliminarAjax
        // =============================================
        [HttpPost]
        public JsonResult EliminarAjax(int idNarracion)
        {
            if (Session["IdUsuario"] == null)
                return Json(new { ok = false, mensaje = "Sesión expirada." });

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            var item = db.NarracionesDeportivas
                .FirstOrDefault(n => n.IdNarracion == idNarracion && n.IdUsuario == idUsuario);

            if (item == null)
                return Json(new { ok = false, mensaje = "No se encontró la narración." });

            item.Activo = false;
            db.SaveChanges();

            return Json(new { ok = true, mensaje = "Narración eliminada." });
        }

        // =============================================
        // POST: /NarracionesDeportivas/ToggleFavoritoAjax
        // =============================================
        [HttpPost]
        public JsonResult ToggleFavoritoAjax(int idNarracion)
        {
            if (Session["IdUsuario"] == null)
                return Json(new { ok = false, mensaje = "Sesión expirada." });

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            var item = db.NarracionesDeportivas
                .FirstOrDefault(n => n.IdNarracion == idNarracion && n.IdUsuario == idUsuario);

            if (item == null)
                return Json(new { ok = false, mensaje = "No se encontró la narración." });

            item.EsFavorito = !item.EsFavorito;
            db.SaveChanges();

            return Json(new
            {
                ok = true,
                favorito = item.EsFavorito,
                mensaje = item.EsFavorito ? "Añadido a favoritos." : "Quitado de favoritos."
            });
        }

        // =============================================
        // POST: /NarracionesDeportivas/ActualizarNotasAjax
        // =============================================
        [HttpPost]
        public JsonResult ActualizarNotasAjax(int idNarracion, string notas)
        {
            if (Session["IdUsuario"] == null)
                return Json(new { ok = false, mensaje = "Sesión expirada." });

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            var item = db.NarracionesDeportivas
                .FirstOrDefault(n => n.IdNarracion == idNarracion && n.IdUsuario == idUsuario);

            if (item == null)
                return Json(new { ok = false, mensaje = "No se encontró la narración." });

            item.Notas = Truncar(notas, 500);
            db.SaveChanges();

            return Json(new { ok = true, mensaje = "Notas guardadas." });
        }

        // =============================================
        // Helpers
        // =============================================
        private static string Truncar(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length > max ? s.Substring(0, max) : s;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}