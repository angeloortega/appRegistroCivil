using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Services.Protocols;


namespace appRegistroCivil.Models
{
    public class PaisController : Controller
    {
        private RegistroCivilEntities db = new RegistroCivilEntities();

        // GET: Pais
        //public ActionResult Index()
        //{
        //    var pais = db.Pais.Include(p => p.Audios).Include(p => p.Imagenes);
        //    return View(pais.ToList());
        //}

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

        public class PagedData<T> where T : class
        {
            public IEnumerable<T> Data { get; set; }
            public int NumberOfPages { get; set; }
            public int CurrentPage { get; set; }
        }

        public const int PageSize = 5;

        public ActionResult Index()
        {
            var pais = new PagedData<Pais>();

            using (var ctx = new RegistroCivilEntities())
            {
                pais.Data = ctx.Pais.OrderBy(p => p.nbrPais).Take(PageSize).ToList();
                pais.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)ctx.Pais.Count() / PageSize));
            }

            return View(pais);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaisList(int page) { 
        
            var pais = new PagedData<Pais>();
            Console.WriteLine("El numero de pagina es: " + page);
            using (var ctx = new RegistroCivilEntities())
            {
                pais.Data = ctx.Pais.OrderBy(p => p.nbrPais).Skip(PageSize * (page - 1)).Skip(PageSize).ToList();
                pais.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)ctx.Pais.Count() / PageSize));
                pais.CurrentPage = page;
            }

            return PartialView(pais);
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
