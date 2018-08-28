using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appRegistroCivil.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data.SqlClient;
using System.IO.Pipes;

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
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Include(p => p.Videos).Where(p => p.idPaisResidencia == 2);
            ViewBag.totalPaises = db.Pais.Count();
            Pais pais = db.Pais.First();
            //Pais pais = db.Pais.Where(x=>x.idPais == id).First();
            ViewBag.idPais = pais.idPais;
            ViewBag.nombrePais = pais.nbrPais;
            ViewBag.area = pais.area;
            if (db.Persona.Where(x => x.idPersona == pais.idPresidenteActual).Count() > 0)
            {
                ViewBag.nombrePresi = "" + db.Persona.Where(x => x.idPersona == 1 && x.idPaisResidencia == pais.idPais).First().nbrPersona; 
            }
            else
            {
                ViewBag.nombrePresi = "No hay";
            }
            ViewBag.foto = db.Imagenes.Where(x=> x.id == pais.fotoBandera).First().info_bytes;
            ViewBag.poblacion = db.Persona.Where(x=> x.idPaisResidencia == pais.idPais).Count();
            ViewBag.himno = db.Audios.Where(x => x.id == pais.himnoNacional).First().info_bytes;
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
            Pais pais = new Pais();
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Include(p => p.Videos).Where(p=> p.idPaisResidencia == id);
            
            if (db.Pais.Where(x => x.idPais == id).Count() > 0)
            {
                pais = db.Pais.Where(x => x.idPais == id).First();
            }
            else
            {
                return PaisIndex(id + 1);
            }
            ViewBag.totalPaises = db.Pais.Count();
            ViewBag.nombrePais = pais.nbrPais;
            ViewBag.area = pais.area;
            if (db.Persona.Where(x => x.idPersona == pais.idPresidenteActual).Count() > 0)
            {
                ViewBag.nombrePresi = "" + db.Persona.Where(x => x.idPersona == pais.idPresidenteActual && x.idPaisResidencia == pais.idPais).First().nbrPersona + ""; 
            }
            else
            {
                ViewBag.nombrePresi = "No hay";
            }
            ViewBag.foto = db.Imagenes.Where(x => x.id == pais.fotoBandera).First().info_bytes;
            ViewBag.idPais = pais.idPais;
            ViewBag.poblacion = db.Persona.Where(x => x.idPaisResidencia == pais.idPais).Count();
            ViewBag.himno = db.Audios.Where(x => x.id == pais.himnoNacional).First().info_bytes;
            return View("Index", persona.ToList());
        }

        public ActionResult ListaPersonas()
        {
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Include(p => p.Videos);
            return View(persona.ToList());
        }

        // GET: Personas/Details/5
        public ActionResult Details(decimal id, decimal id2)
        {
            if (id == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id,id2);
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

        // GET: Pais/Create
        public ActionResult CreatePais()
        {
            
            db = TransactionSingletone.db;
            ViewBag.fotoBandera = db.Imagenes.Where(x => x.id == 0).First().info_bytes;
            //ViewBag.himno = new SelectList(db.Audios, "id", "descripcion");
            //ViewBag.fotoBandera = db.Imagenes.Where(x => x.id == 0);
            return View();
        }

        // POST: Pais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePais([Bind(Include = "idPais,nbrPais,area,poblacionActual,fotoBandera,himnoNacional,idPresidenteActual")] Pais pais, HttpPostedFileBase file)
        {
            //Image convert = Image.FromFile("C:\\Users\\Giulliano\\Desktop\\himno.mp4");
            //ImageConverter _imageConverter = new ImageConverter();
            //byte[] xByte = (byte[])_imageConverter.ConvertTo(convert, typeof(byte[]));
            Videos imageI = new Videos();
            byte[] audiobyte = System.IO.File.ReadAllBytes("C:\\Users\\Giulliano\\Desktop\\interview.mp4");
            //imageI.id = db.Imagenes.Count();
            imageI.id = 2;
            //imageI.descripcion = ""+ pais.nbrPais + "";
            imageI.descripcion = "Default Interview";
            imageI.info_bytes = audiobyte;
            pais.fotoBandera = imageI.id;
            try
            {
                db.Videos.Add(imageI);
                //db.Pais.Add(pais);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.himnoNacional = new SelectList(db.Audios, "id", "descripcion", pais.himnoNacional);
                ViewBag.fotoBandera = new SelectList(db.Imagenes, "id", "descripcion", pais.fotoBandera);
                return View(pais);
            }
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(decimal id, decimal id2)
        {
            if (id == null || id2 == null)
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

        // POST: Personas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona,nbrPersona,idPaisNacimiento,idPaisResidencia,fchNacimiento,correo,foto,videoEntrevista")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db = new RegistroCivilEntities();
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
