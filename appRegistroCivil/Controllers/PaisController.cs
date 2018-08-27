using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appRegistroCivil.Models;
using System.Data.SqlClient;

namespace appRegistroCivil.Views
{
    public class PaisController : Controller
    {
        private RegistroCivilEntities db = new RegistroCivilEntities();

        // GET: Pais
        public ActionResult Index()
        {
            var pais = db.Pais.Include(p => p.Audios).Include(p => p.Imagenes);
            return View(pais.ToList());
        }

        // GET: Pais/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = db.Pais.Find(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // GET: Pais/Create
        public ActionResult Create()
        {
            ViewBag.himnoNacional = new SelectList(db.Audios, "id", "descripcion");
            ViewBag.fotoBandera = new SelectList(db.Imagenes, "id", "descripcion");
            return View();
        }

        // POST: Pais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPais,nbrPais,area,poblacionActual,fotoBandera,himnoNacional,idPresidenteActual")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                db.Pais.Add(pais);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.himnoNacional = new SelectList(db.Audios, "id", "descripcion", pais.himnoNacional);
            ViewBag.fotoBandera = new SelectList(db.Imagenes, "id", "descripcion", pais.fotoBandera);
            return View(pais);
        }

        // GET: Pais/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = db.Pais.Find(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            ViewBag.himnoNacional = new SelectList(db.Audios, "id", "descripcion", pais.himnoNacional);
            ViewBag.fotoBandera = new SelectList(db.Imagenes, "id", "descripcion", pais.fotoBandera);
            return View(pais);
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPais,nbrPais,area,poblacionActual,fotoBandera,himnoNacional,idPresidenteActual")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pais).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.himnoNacional = new SelectList(db.Audios, "id", "descripcion", pais.himnoNacional);
            ViewBag.fotoBandera = new SelectList(db.Imagenes, "id", "descripcion", pais.fotoBandera);
            return View(pais);
        }

        // GET: Pais/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = db.Pais.Find(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // POST: Pais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Pais pais = db.Pais.Find(id);
            db.Pais.Remove(pais);
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


namespace appRegistroCivil.Controllers
{
    public class PaisController : Controller
    {
        public ActionResult EdadPromedio()
        {
            RegistroCivilEntities db = TransactionSingletone.db;
            var lista = db.Database.SqlQuery<SP_EdadPromedio1_Result>("exec SP_EdadPromedio");
            return View(lista.ToList());
        }
        public ActionResult EstadisticaNacimiento()
        {
            RegistroCivilEntities db = TransactionSingletone.db;
            var lista = db.Database.SqlQuery<SP_Nacimientos_Result>("exec SP_Nacimientos");
            return View(lista.ToList());
        }
    }
}