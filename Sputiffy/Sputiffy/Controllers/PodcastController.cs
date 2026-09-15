using Newtonsoft.Json.Linq;
using Sputiffy.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sputiffy.Controllers
{
    // Módulo de Podcast: busca episodios/podcasts en YouTube, igual que ApiMusicaController
    // pero enfocado a contenido hablado (podcasts) en vez de música.
    public class PodcastController : Controller
    {
        // La clave se lee de Web.config (appSettings -> YoutubeApiKey).
        // Así evitamos repetir el problema de dejar la key escrita en el código.
        private readonly string youtubeApiKey = ConfigurationManager.AppSettings["YoutubeApiKey"];
        private readonly sputiffyEntities1 db = new sputiffyEntities1();

        // GET: Podcast
        public ActionResult Index()
        {
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Login", "Acceso");

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            var usuario = db.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            if (usuario == null)
                return RedirectToAction("Login", "Acceso");

            var playlists = db.Playlists
                .Where(p => p.IdUsuario == idUsuario && p.Activo == true)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(8)
                .ToList();

            ViewBag.Usuario = usuario;
            ViewBag.Playlists = playlists;

            return View();
        }

        // GET: Podcast/Buscar?term=...
        [HttpGet]
        public async Task<JsonResult> Buscar(string term)
        {
            if (Session["IdUsuario"] == null)
                return Json(new { error = true, mensaje = "Sesión no iniciada." }, JsonRequestBehavior.AllowGet);

            try
            {
                if (string.IsNullOrWhiteSpace(term))
                    return Json(new object[] { }, JsonRequestBehavior.AllowGet);

                if (string.IsNullOrWhiteSpace(youtubeApiKey) || youtubeApiKey == "TU_YOUTUBE_API_KEY")
                {
                    return Json(new
                    {
                        error = true,
                        mensaje = "Falta configurar YoutubeApiKey en Web.config (appSettings)."
                    }, JsonRequestBehavior.AllowGet);
                }

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(15);

                    // topicId de Google para "Podcast" ayuda a filtrar resultados relacionados
                    var url =
                        "https://www.googleapis.com/youtube/v3/search" +
                        "?part=snippet" +
                        "&type=video" +
                        "&maxResults=12" +
                        "&q=" + Uri.EscapeDataString(term + " podcast") +
                        "&key=" + youtubeApiKey;

                    var response = await client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return Json(new
                        {
                            error = true,
                            mensaje = json
                        }, JsonRequestBehavior.AllowGet);
                    }

                    var data = JObject.Parse(json);

                    var resultados = data["items"]
                        .Select(x => new
                        {
                            id = x["id"]?["videoId"]?.ToString(),
                            titulo = x["snippet"]?["title"]?.ToString() ?? "",
                            autor = x["snippet"]?["channelTitle"]?.ToString() ?? "",
                            descripcion = x["snippet"]?["description"]?.ToString() ?? "",
                            portada =
                                x["snippet"]?["thumbnails"]?["high"]?["url"]?.ToString() ??
                                x["snippet"]?["thumbnails"]?["medium"]?["url"]?.ToString() ??
                                x["snippet"]?["thumbnails"]?["default"]?["url"]?.ToString() ?? ""
                        })
                        .Where(x => !string.IsNullOrWhiteSpace(x.id))
                        .ToList();

                    return Json(resultados, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    mensaje = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
