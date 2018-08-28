using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appRegistroCivil.Models;

namespace appRegistroCivil.Views
{
    public class PaisEditarController : Controller
    {
        private RegistroCivilEntities db = new RegistroCivilEntities();

        // GET: PaisEditar
        public ActionResult Index()
        {
            var pais = db.Pais.Include(p => p.Audios).Include(p => p.Imagenes).Include(p => p.Persona2);
            return View(pais.ToList());
        }

        // GET: PaisEditar/Edit/5
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
            ViewBag.idPresidenteActual = new SelectList(db.Persona.Where(p => p.idPaisNacimiento == id), "idPersona", "nbrPersona", pais.idPresidenteActual);
            return View(pais);
        }

        // POST: PaisEditar/Edit/5
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
            ViewBag.idPresidenteActual = new SelectList(db.Persona, "idPersona", "nbrPersona", pais.idPresidenteActual);
            return View(pais);
        }
    }
}
