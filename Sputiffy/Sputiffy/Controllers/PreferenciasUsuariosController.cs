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
    public class PreferenciasUsuariosController : Controller
    {
        private sputiffyEntities1 db = new sputiffyEntities1();

        // GET: PreferenciasUsuarios
        public ActionResult Index()
        {
            var preferenciasUsuario = db.PreferenciasUsuario.Include(p => p.Generos).Include(p => p.Usuarios);
            return View(preferenciasUsuario.ToList());
        }

        // GET: PreferenciasUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreferenciasUsuario preferenciasUsuario = db.PreferenciasUsuario.Find(id);
            if (preferenciasUsuario == null)
            {
                return HttpNotFound();
            }
            return View(preferenciasUsuario);
        }

        // GET: PreferenciasUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.IdGeneroFavorito = new SelectList(db.Generos, "IdGenero", "NombreGenero");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: PreferenciasUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPreferencia,IdUsuario,DecadaPreferida,IdGeneroFavorito,ModoRecomendacion")] PreferenciasUsuario preferenciasUsuario)
        {
            if (ModelState.IsValid)
            {
                db.PreferenciasUsuario.Add(preferenciasUsuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdGeneroFavorito = new SelectList(db.Generos, "IdGenero", "NombreGenero", preferenciasUsuario.IdGeneroFavorito);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", preferenciasUsuario.IdUsuario);
            return View(preferenciasUsuario);
        }

        // GET: PreferenciasUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreferenciasUsuario preferenciasUsuario = db.PreferenciasUsuario.Find(id);
            if (preferenciasUsuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdGeneroFavorito = new SelectList(db.Generos, "IdGenero", "NombreGenero", preferenciasUsuario.IdGeneroFavorito);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", preferenciasUsuario.IdUsuario);
            return View(preferenciasUsuario);
        }

        // POST: PreferenciasUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPreferencia,IdUsuario,DecadaPreferida,IdGeneroFavorito,ModoRecomendacion")] PreferenciasUsuario preferenciasUsuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preferenciasUsuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdGeneroFavorito = new SelectList(db.Generos, "IdGenero", "NombreGenero", preferenciasUsuario.IdGeneroFavorito);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", preferenciasUsuario.IdUsuario);
            return View(preferenciasUsuario);
        }

        // GET: PreferenciasUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreferenciasUsuario preferenciasUsuario = db.PreferenciasUsuario.Find(id);
            if (preferenciasUsuario == null)
            {
                return HttpNotFound();
            }
            return View(preferenciasUsuario);
        }

        // POST: PreferenciasUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PreferenciasUsuario preferenciasUsuario = db.PreferenciasUsuario.Find(id);
            db.PreferenciasUsuario.Remove(preferenciasUsuario);
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
