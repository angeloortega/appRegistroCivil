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
    public class PersonasController : Controller
    {
        private TransactionSingletone singletone = TransactionSingletone.Instance();
        private static RegistroCivilEntities db;
        // GET: Personas
        public ActionResult Index()
        {
            db = TransactionSingletone.db;
            //id = 1;
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Include(p => p.Videos).Where(p => p.idPaisResidencia == 1);
            ViewBag.totalPaises = db.Pais.Count();
            Pais pais = db.Pais.First();
            //Pais pais = db.Pais.Where(x=>x.idPais == id).First();
            ViewBag.idPais = pais.idPais;
            ViewBag.nombrePais = pais.nbrPais;
            ViewBag.area = pais.area;
            ViewBag.pobActual = pais.poblacionActual;
            ViewBag.idPresi = pais.idPresidenteActual;
            ViewBag.audios = pais.himnoNacional;
            ViewBag.foto = pais.fotoBandera;
            return View(persona.ToList());
        }

        public ActionResult ReloadTable()
        {
            TransactionSingletone.SendCommit();
            db = TransactionSingletone.db;
            var person = from m in db.Persona
                         select m;
            return RedirectToAction("Index");
        }

        public ActionResult PaisIndex(int id)
        {
           // TransactionSingletone.reloadDBContext();
            db = TransactionSingletone.db;
            int cantidadPaises = db.Pais.Count();
            if (id < 1)
            {
                id = 1;
            }
            if (id > db.Pais.Count())
            {
                id = 1;
            }
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Include(p => p.Videos).Where(p=> p.idPaisResidencia == id);
            Pais pais = db.Pais.Where(x=>x.idPais == id).First();
            ViewBag.totalPaises = db.Pais.Count();
            ViewBag.nombrePais = pais.nbrPais;
            ViewBag.area = pais.area;
            ViewBag.pobActual = pais.poblacionActual;
            ViewBag.idPresi = pais.idPresidenteActual;
            ViewBag.audios = pais.himnoNacional;
            ViewBag.foto = pais.fotoBandera;
            ViewBag.idPais = pais.idPais;
            return View("Index", persona.ToList());
        }

        public ActionResult ListaPersonas()
        {
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Include(p => p.Videos);
            return View(persona.ToList());
        }

        // GET: Personas/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            ViewBag.foto = new SelectList(db.Imagenes, "id", "descripcion");
            ViewBag.idPaisNacimiento = new SelectList(db.Pais, "idPais", "nbrPais");
            ViewBag.idPaisResidencia = new SelectList(db.Pais, "idPais", "nbrPais");
            ViewBag.videoEntrevista = new SelectList(db.Videos, "id", "descripcion");
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona,nbrPersona,idPaisNacimiento,idPaisResidencia,fchNacimiento,correo,foto,videoEntrevista")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                TransactionSingletone.UploadPerson(persona);
                return RedirectToAction("Index");
            }

            ViewBag.foto = new SelectList(db.Imagenes, "id", "descripcion", persona.foto);
            ViewBag.idPaisNacimiento = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisNacimiento);
            ViewBag.idPaisResidencia = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisResidencia);
            ViewBag.videoEntrevista = new SelectList(db.Videos, "id", "descripcion", persona.videoEntrevista);
            return View(persona);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
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

        // POST: Personas/Edit/5
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

        // GET: Personas/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Persona persona = db.Persona.Find(id);
            db.Persona.Remove(persona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
