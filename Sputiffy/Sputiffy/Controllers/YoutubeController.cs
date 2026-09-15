using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sputiffy.Controllers
{
    public class YoutubeController : Controller
    {
        private readonly string apiKey = "TU_YOUTUBE_API_KEY";

        [HttpGet]
        public async Task<JsonResult> Buscar(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                    return Json(new object[] { }, JsonRequestBehavior.AllowGet);

                using (var client = new HttpClient())
                {
                    var url =
                        "https://www.googleapis.com/youtube/v3/search" +
                        "?part=snippet" +
                        "&type=video" +
                        "&maxResults=10" +
                        "&q=" + Uri.EscapeDataString(term) +
                        "&key=" + apiKey;

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
                            titulo = x["snippet"]?["title"]?.ToString(),
                            artista = x["snippet"]?["channelTitle"]?.ToString(),
                            portada = x["snippet"]?["thumbnails"]?["high"]?["url"]?.ToString()
                                      ?? x["snippet"]?["thumbnails"]?["medium"]?["url"]?.ToString()
                                      ?? x["snippet"]?["thumbnails"]?["default"]?["url"]?.ToString()
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

        [HttpGet]
        public async Task<JsonResult> BuscarPorEpoca(string termino)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termino))
                    return Json(new object[] { }, JsonRequestBehavior.AllowGet);

                using (var client = new HttpClient())
                {
                    var url =
                        "https://www.googleapis.com/youtube/v3/search" +
                        "?part=snippet" +
                        "&type=video" +
                        "&videoCategoryId=10" +
                        "&maxResults=12" +
                        "&q=" + Uri.EscapeDataString(termino) +
                        "&key=" + apiKey;

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
                            titulo = x["snippet"]?["title"]?.ToString(),
                            artista = x["snippet"]?["channelTitle"]?.ToString(),
                            portada = x["snippet"]?["thumbnails"]?["high"]?["url"]?.ToString()
                                      ?? x["snippet"]?["thumbnails"]?["medium"]?["url"]?.ToString()
                                      ?? x["snippet"]?["thumbnails"]?["default"]?["url"]?.ToString()
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