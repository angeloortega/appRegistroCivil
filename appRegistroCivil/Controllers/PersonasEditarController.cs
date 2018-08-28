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
    public class PersonasEditarController : Controller
    {
        private RegistroCivilEntities db = new RegistroCivilEntities();

        // GET: PersonasEditar
        public ActionResult Index()
        {
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Include(p => p.Videos);
            return View(persona.ToList());
        }

        // GET: PersonasEditar/Edit/5
        public ActionResult Edit(decimal id, decimal id2)
        {
            if (id == null | id2==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id,id2);
            if (persona == null)
            {
                return HttpNotFound();
            }
            ViewBag.foto = new SelectList(db.Imagenes, "id", "descripcion", persona.foto);
            ViewBag.idPaisNacimiento = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisNacimiento);
            ViewBag.idPaisResidencia = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisResidencia);
            ViewBag.videoEntrevista = new SelectList(db.Videos, "id", "descripcion", persona.videoEntrevista);
            return View(persona);
        }

        // POST: PersonasEditar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona,nbrPersona,idPaisNacimiento,idPaisResidencia,fchNacimiento,correo,foto,videoEntrevista")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.foto = new SelectList(db.Imagenes, "id", "descripcion", persona.foto);
            ViewBag.idPaisNacimiento = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisNacimiento);
            ViewBag.idPaisResidencia = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisResidencia);
            ViewBag.videoEntrevista = new SelectList(db.Videos, "id", "descripcion", persona.videoEntrevista);
            return View(persona);
        }
    }
}
