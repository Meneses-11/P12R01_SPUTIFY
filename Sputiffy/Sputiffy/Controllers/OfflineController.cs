using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Sputiffy.Models;

namespace Sputiffy.Controllers
{
    public class OfflineController : Controller
    {
        private SputiffyContext db = new SputiffyContext();

        // Carpeta raíz por defecto donde buscas tu música.
        // Cámbiala si usas otra ubicación.
        private static readonly string CarpetaMusica = @"C:\Musica";

        // GET: Offline  (con búsqueda opcional)
        public ActionResult Index(string busqueda)
        {
            var query = db.Canciones.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                query = query.Where(c =>
                    c.Titulo.Contains(busqueda) ||
                    (c.Artista != null && c.Artista.Contains(busqueda)) ||
                    (c.Album != null && c.Album.Contains(busqueda)));
            }

            ViewBag.Busqueda = busqueda;
            return View(query.OrderBy(c => c.Titulo).ToList());
        }

        // GET: Offline/Reproducir/5
        public ActionResult Reproducir(int id)
        {
            var cancion = db.Canciones.FirstOrDefault(c => c.Id == id);

            if (cancion == null)
                return HttpNotFound("La canción no existe en la base de datos.");

            if (string.IsNullOrWhiteSpace(cancion.RutaArchivo) ||
                !System.IO.File.Exists(cancion.RutaArchivo))
            {
                return new HttpStatusCodeResult(410,
                    "El archivo MP3 no se encuentra en la ruta registrada: " + cancion.RutaArchivo);
            }

            return new RangeFileResult(cancion.RutaArchivo, "audio/mpeg");
        }

        // ==================== AGREGAR ====================

        // GET: Offline/Agregar - muestra el explorador de carpetas
        public ActionResult Agregar()
        {
            ViewBag.CarpetaInicial = CarpetaMusica;
            return View();
        }

        // GET: Offline/ExplorarCarpeta?ruta=C:\Musica
        // Devuelve JSON con subcarpetas y archivos .mp3 de esa ruta
        public JsonResult ExplorarCarpeta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                ruta = CarpetaMusica;

            if (!Directory.Exists(ruta))
            {
                return Json(new { error = "La carpeta no existe: " + ruta }, JsonRequestBehavior.AllowGet);
            }

            var carpetas = Directory.GetDirectories(ruta)
                .Select(d => new { nombre = Path.GetFileName(d), ruta = d })
                .OrderBy(c => c.nombre)
                .ToList();

            var archivos = Directory.GetFiles(ruta, "*.mp3")
                .Select(f => new { nombre = Path.GetFileName(f), ruta = f })
                .OrderBy(a => a.nombre)
                .ToList();

            var padre = Directory.GetParent(ruta);

            return Json(new
            {
                rutaActual = ruta,
                carpetaPadre = padre != null ? padre.FullName : null,
                carpetas,
                archivos
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Offline/AgregarSeleccionadas
        [HttpPost]
        public ActionResult AgregarSeleccionadas(List<string> rutasSeleccionadas)
        {
            if (rutasSeleccionadas == null || !rutasSeleccionadas.Any())
            {
                TempData["Mensaje"] = "No seleccionaste ningún archivo.";
                return RedirectToAction("Agregar");
            }

            int agregadas = 0;
            int yaExistian = 0;

            foreach (var ruta in rutasSeleccionadas)
            {
                if (!System.IO.File.Exists(ruta)) continue;

                if (db.Canciones.Any(c => c.RutaArchivo == ruta))
                {
                    yaExistian++;
                    continue;
                }

                string titulo = Path.GetFileNameWithoutExtension(ruta);
                string artista = null;
                string album = null;
                int? duracion = null;

                try
                {
                    var tagFile = TagLib.File.Create(ruta);
                    if (!string.IsNullOrWhiteSpace(tagFile.Tag.Title))
                        titulo = tagFile.Tag.Title;

                    if (tagFile.Tag.Performers != null && tagFile.Tag.Performers.Length > 0)
                        artista = string.Join(", ", tagFile.Tag.Performers);

                    album = tagFile.Tag.Album;
                    duracion = (int)tagFile.Properties.Duration.TotalSeconds;
                }
                catch
                {
                    // Si el mp3 no tiene tags legibles, se guarda solo con el nombre del archivo
                }

                db.Canciones.Add(new Cancion
                {
                    Titulo = titulo,
                    Artista = artista,
                    Album = album,
                    RutaArchivo = ruta,
                    Duracion = duracion,
                    FechaRegistro = DateTime.Now
                });

                agregadas++;
            }

            db.SaveChanges();

            TempData["Mensaje"] = $"{agregadas} canción(es) agregada(s). {yaExistian} ya existían en la base.";
            return RedirectToAction("Index");
        }

        // ==================== EDITAR ====================

        // GET: Offline/Editar/5
        public ActionResult Editar(int id)
        {
            var cancion = db.Canciones.Find(id);
            if (cancion == null) return HttpNotFound();
            return View(cancion);
        }

        // POST: Offline/Editar
        [HttpPost]
        public ActionResult Editar(Cancion model)
        {
            var cancion = db.Canciones.Find(model.Id);
            if (cancion == null) return HttpNotFound();

            cancion.Titulo = model.Titulo;
            cancion.Artista = model.Artista;
            cancion.Album = model.Album;

            db.SaveChanges();

            // Además de la base de datos, actualiza las etiquetas ID3 del archivo real
            if (System.IO.File.Exists(cancion.RutaArchivo))
            {
                try
                {
                    var tagFile = TagLib.File.Create(cancion.RutaArchivo);
                    tagFile.Tag.Title = cancion.Titulo;
                    tagFile.Tag.Performers = string.IsNullOrWhiteSpace(cancion.Artista)
                        ? new string[0]
                        : new[] { cancion.Artista };
                    tagFile.Tag.Album = cancion.Album;
                    tagFile.Save();
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = "Se guardó en la base, pero no se pudo escribir en el archivo: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }

            TempData["Mensaje"] = "Canción actualizada (base de datos y archivo MP3).";
            return RedirectToAction("Index");
        }

        // ==================== ELIMINAR ====================

        // POST: Offline/Eliminar/5 (solo borra el registro, el MP3 sigue en el disco)
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var cancion = db.Canciones.Find(id);
            if (cancion != null)
            {
                db.Canciones.Remove(cancion);
                db.SaveChanges();
                TempData["Mensaje"] = "Canción eliminada de la base de datos. El archivo sigue en tu disco.";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
    public class RangeFileResult : ActionResult
    {
        private readonly string _rutaArchivo;
        private readonly string _contentType;

        public RangeFileResult(string rutaArchivo, string contentType)
        {
            _rutaArchivo = rutaArchivo;
            _contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;

            var fileInfo = new FileInfo(_rutaArchivo);
            long tamanoTotal = fileInfo.Length;

            long inicio = 0;
            long fin = tamanoTotal - 1;

            string rangeHeader = request.Headers["Range"];
            bool esParcial = false;

            if (!string.IsNullOrEmpty(rangeHeader) && rangeHeader.StartsWith("bytes="))
            {
                esParcial = true;
                var rango = rangeHeader.Replace("bytes=", "").Split('-');

                if (long.TryParse(rango[0], out long r0))
                    inicio = r0;

                if (rango.Length > 1 && long.TryParse(rango[1], out long r1))
                    fin = r1;
                else
                    fin = tamanoTotal - 1;

                if (fin >= tamanoTotal) fin = tamanoTotal - 1;
                if (inicio > fin) inicio = fin;
            }

            long longitud = fin - inicio + 1;

            response.Clear();
            response.Buffer = false;
            response.BufferOutput = false;
            response.ContentType = _contentType;
            response.AddHeader("Accept-Ranges", "bytes");
            response.AddHeader("Content-Length", longitud.ToString());

            if (esParcial)
            {
                response.StatusCode = 206; // Partial Content
                response.AddHeader("Content-Range", $"bytes {inicio}-{fin}/{tamanoTotal}");
            }
            else
            {
                response.StatusCode = 200;
            }

            const int tamanoBuffer = 64 * 1024; // 64 KB por bloque
            byte[] buffer = new byte[tamanoBuffer];

            using (var stream = new FileStream(_rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stream.Seek(inicio, SeekOrigin.Begin);
                long restante = longitud;

                while (restante > 0)
                {
                    if (!response.IsClientConnected) break;

                    int aLeer = (int)Math.Min(tamanoBuffer, restante);
                    int leidos = stream.Read(buffer, 0, aLeer);

                    if (leidos <= 0) break;

                    response.OutputStream.Write(buffer, 0, leidos);
                    response.Flush();

                    restante -= leidos;
                }
            }
        }
    }
}