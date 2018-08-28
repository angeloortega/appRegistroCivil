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
using System.Web.Hosting;

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
            decimal firstid = db.Pais.First().idPais;
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Where(p => p.idPaisResidencia == firstid);
            ViewBag.totalPaises = db.Pais.Count();
            Pais pais = db.Pais.First();
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
            return RedirectToAction("Index");
        }

        public ActionResult CancelTransaction()
        {
            TransactionSingletone.SendRollback();
            db = TransactionSingletone.db;
            return RedirectToAction("Index");
        }

        public ActionResult CargarMediaDePrueba() {
            TransactionSingletone.stopTransaction();
            db = new RegistroCivilEntities();
            Videos video = db.Videos.Find(1);
            Imagenes imagen1 = db.Imagenes.Find(1);
            Imagenes imagen2 = db.Imagenes.Find(2);
            Audios audio = db.Audios.Find(1);
            byte[] videoByte = System.IO.File.ReadAllBytes(HostingEnvironment.ApplicationPhysicalPath +"\\Resources\\video.mp4");
            byte[] imagen1Byte = System.IO.File.ReadAllBytes(HostingEnvironment.ApplicationPhysicalPath + "\\Resources\\BanderaDefault.jpeg");
            byte[] imagen2Byte = System.IO.File.ReadAllBytes(HostingEnvironment.ApplicationPhysicalPath + "\\Resources\\PersonaDefault.jpeg");
            byte[] audioByte = System.IO.File.ReadAllBytes(HostingEnvironment.ApplicationPhysicalPath + "\\Resources\\himno.mp3");
            video.descripcion = "Default Interview";
            video.info_bytes = videoByte;
            imagen1.descripcion = "Bandera Default Pais";
            imagen1.info_bytes = imagen1Byte;
            imagen2.descripcion = "Foto de Perfil Default";
            imagen2.info_bytes = imagen2Byte;
            audio.descripcion = "Audio Default";
            audio.info_bytes = audioByte;
            try
            {

                
                //db.Videos.Attach(video);
                //db.Imagenes.Attach(imagen1);
                //db.Imagenes.Attach(imagen2);
                //db.Audios.Attach(audio);
                db.Entry(video).State = EntityState.Modified;
                db.Entry(imagen1).State = EntityState.Modified;
                db.Entry(imagen2).State = EntityState.Modified;
                db.Entry(audio).State = EntityState.Modified;
                db.SaveChanges();
                TransactionSingletone.ResetInstance();
                db = TransactionSingletone.db;
            }
            catch(Exception e) {}
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
            if (id > db.Pais.Count()+1)
            {
                id = 1;
            }
            Pais pais = new Pais();
            var persona = db.Persona.Include(p => p.Imagenes).Include(p => p.Pais).Include(p => p.Pais1).Where(p=> p.idPaisResidencia == id);
            
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
            int idPais = ViewBag.idPais;
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
            ViewBag.video = db.Videos.Where(x => x.id == persona.videoEntrevista).First().info_bytes;
            return View(persona);
        }

        // GET: Pais/Details/5
        public ActionResult DetailsPais(decimal id)
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

        // GET: Personas/Create
        public ActionResult Create()
        {
            ViewBag.foto = db.Imagenes.Where(x => x.id == 0).First().info_bytes;
            ViewBag.idPaisNacimiento = new SelectList(db.Pais, "idPais", "nbrPais");
            ViewBag.idPaisResidencia = new SelectList(db.Pais, "idPais", "nbrPais");
            ViewBag.videoEntrevista = db.Videos.Where(x => x.id == 1).First().info_bytes;
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona,nbrPersona,idPaisNacimiento,idPaisResidencia,fchNacimiento,correo,foto,videoEntrevista")] Persona persona, IEnumerable<HttpPostedFileBase> files)
        {
            persona.idPersona = db.Pais.Where(x => x.idPais == persona.idPaisResidencia).First().Persona.Count() + 1;
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
            {
                fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
            }
            byte[] xByte = fileData;
            using (var binaryReader = new BinaryReader(Request.Files[1].InputStream))
            {
                fileData = binaryReader.ReadBytes(Request.Files[1].ContentLength);
            }
            byte[] xByteVid = fileData;
            Imagenes imageI = new Imagenes();
            Videos video = new Videos();
            //byte[] xByteVid = (byte[])_imageConverter.ConvertTo(convert, typeof(byte[]));
            video.descripcion = "Entrevista " + persona.nbrPersona + "";
            video.id = db.Videos.Count()+1;
            video.info_bytes = xByteVid;
            imageI.id = db.Imagenes.Count();
            imageI.descripcion = "" + persona.nbrPersona + "";
            imageI.info_bytes = xByte;
            persona.foto = imageI.id;
            persona.videoEntrevista = video.id;
            if (ModelState.IsValid)
            {
                db.Imagenes.Add(imageI);
                db.Videos.Add(video);
                TransactionSingletone.UploadPerson(persona);
                return RedirectToAction("Index");
            }

            ViewBag.foto = new SelectList(db.Imagenes, "id", "descripcion", persona.foto);
            ViewBag.idPaisNacimiento = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisNacimiento);
            ViewBag.idPaisResidencia = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisResidencia);
            ViewBag.videoEntrevista = db.Videos.Where(x => x.id == 1).First().info_bytes;
            return View(persona);
        }

        // GET: Pais/Create
        public ActionResult CreatePais()
        {
            
            db = TransactionSingletone.db;
            ViewBag.fotoBandera = db.Imagenes.Where(x => x.id == 1).First().info_bytes;
            //ViewBag.himno = new SelectList(db.Audios, "id", "descripcion");
            //ViewBag.fotoBandera = db.Imagenes.Where(x => x.id == 0);
            return View();
        }

        // POST: Pais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePais([Bind(Include = "idPais,nbrPais,area,poblacionActual,fotoBandera,himnoNacional,idPresidenteActual")] Pais pais, HttpPostedFileBase file, HttpPostedFileBase fileAud)
        {
            Image convert = Image.FromStream(file.InputStream);
            var ms = new MemoryStream();
            fileAud.InputStream.CopyTo(ms);
            byte[] fileBytes = ms.ToArray();
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(convert, typeof(byte[]));
            Imagenes imageI = new Imagenes();
            Audios audio = new Audios();
            //byte[] audiobyte = System.IO.File.ReadAllBytes("C:\\Users\\Giulliano\\Desktop\\interview.mp4");
            imageI.id = db.Imagenes.Count();
            imageI.descripcion = "" + pais.nbrPais + "";
            imageI.info_bytes = xByte;
            audio.id = db.Audios.Count() + 1;
            audio.descripcion = "Himno " + pais.nbrPais;
            audio.info_bytes = fileBytes;
            pais.fotoBandera = imageI.id;
            pais.himnoNacional = audio.id;
            pais.idPais = db.Pais.Count() + 2;
            try
            {
                db.Imagenes.Add(imageI);
                db.Audios.Add(audio);
                db.Pais.Add(pais);
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
                TransactionSingletone.stopTransaction();
                db.Persona.Attach(persona);
                var entity = db.Entry(persona);
                entity.State = EntityState.Modified;
                db.SaveChanges();
                TransactionSingletone.ResetInstance();
                return RedirectToAction("Index");
                db = TransactionSingletone.db;
            }
            ViewBag.foto = new SelectList(db.Imagenes, "id", "descripcion", persona.foto);
            ViewBag.idPaisNacimiento = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisNacimiento);
            ViewBag.idPaisResidencia = new SelectList(db.Pais, "idPais", "nbrPais", persona.idPaisResidencia);
            ViewBag.videoEntrevista = new SelectList(db.Videos, "id", "descripcion", persona.videoEntrevista);
            return View(persona);
        }

        // GET: Pais/Edit/5
        public ActionResult EditPais(decimal id)
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

        // POST: Pais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPais([Bind(Include = "idPais,nbrPais,area,poblacionActual,fotoBandera,himnoNacional,idPresidenteActual")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                db = new RegistroCivilEntities();
                TransactionSingletone.stopTransaction();
                db.Pais.Attach(pais);
                var entity = db.Entry(pais);
                entity.State = EntityState.Modified;
                db.SaveChanges();
                TransactionSingletone.ResetInstance();
                return RedirectToAction("Index");
                db = TransactionSingletone.db;
            }
            ViewBag.himnoNacional = new SelectList(db.Audios, "id", "descripcion", pais.himnoNacional);
            ViewBag.fotoBandera = new SelectList(db.Imagenes, "id", "descripcion", pais.fotoBandera);
            ViewBag.idPresidenteActual = new SelectList(db.Persona.Where(p=>p.idPaisNacimiento==pais.idPais), "idPersona", "nbrPersona", pais.idPresidenteActual);
            return View(pais);
        }

        // GET: Personas/Delete/5
        public ActionResult Delete(decimal id, decimal id2)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id, id2);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id, decimal id2)
        {
            Persona persona = db.Persona.Find(id, id2);
            db.Persona.Remove(persona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Pais/Delete/5
        public ActionResult DeletePais(decimal id)
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
        [HttpPost, ActionName("DeletePais")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedPais(decimal id)
        {
            Pais pais = db.Pais.Find(id);
            db.Pais.Remove(pais);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
