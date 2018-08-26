using appRegistroCivil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appRegistroCivil.Models;
using System.Net;

namespace appRegistroCivil.Controllers
{
    public class PersonaController : Controller
    {
        private TransactionSingletone singletone = TransactionSingletone.Instance();
        private RegistroCivilEntities db;

        // GET: Jugadors
        public ViewResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
            db = TransactionSingletone.db;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var person = from m in db.Persona
                         select m;

            if (!String.IsNullOrEmpty(search))
            {
                person = person.Where(s => s.nbrPersona.Contains(search));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    person = person.OrderByDescending(s => s.nbrPersona);
                    break;
                default:
                    person = person.OrderBy(s => s.nbrPersona);
                    break;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(person);
        }

        public ViewResult Paises()
        {
            db = TransactionSingletone.db;
         
            var paises = from m in db.Persona
                         select m;

         
            return View(paises);
        }

        // GET: Jugadors/Details/5
        public ActionResult PersonaDetails(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona jugador = db.Persona.Find(id);
            if (jugador == null)
            {
                return HttpNotFound();
            }
            return View(jugador);
        }

  
        // GET: Jugadors/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Jugadors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona,nbrPersona,idPaisNacimiento,idPaisResidencia,fchNacimiento,correo,foto,video")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                TransactionSingletone.UploadPerson(persona);
                return RedirectToAction("Index");
            }

            return View(persona);
        }


        // GET: Jugadors/Create
        public ActionResult ReloadTable()
        {
            TransactionSingletone.SendCommit();
            db = TransactionSingletone.db;
            var person = from m in db.Persona
                         select m;
            return RedirectToAction("Index");
        }

        /*
        // GET: Jugadors/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador jugador = db.Jugador.Find(id);
            Funcionario funcionario = db.Funcionario.Find(id);
            if (jugador == null)
            {
                return HttpNotFound();
            }
            ViewBag.codigoFuncionario = new SelectList(db.Funcionario, "codigoFuncionario", "nombre", jugador.codigoFuncionario);
            ViewBag.idClub = new SelectList(db.Club, "idClub", "nombre", funcionario.idClub);
            return View(jugador);
        }

        // POST: Jugadors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigoFuncionario,Peso,Altura,nroCamiseta,usuarioModificacion")] Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                Jugador jugadorOut = db.Jugador.Find(jugador.codigoFuncionario);
                jugador.usuarioCreacion = jugadorOut.usuarioCreacion;
                jugador.fchCreacion = jugadorOut.fchCreacion;
                jugador.fchModificacion = DateTime.Now;
                var newContext = new FootballEntities();
                newContext.Entry(jugador).State = EntityState.Modified;
                newContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codigoFuncionario = new SelectList(db.Funcionario, "codigoFuncionario", "nombre", jugador.codigoFuncionario);
            return View(jugador);
        }

        // GET: Jugadors/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jugador jugador = db.Jugador.Find(id);
            if (jugador == null)
            {
                return HttpNotFound();
            }
            return View(jugador);
        }

        // POST: Jugadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Jugador jugador = db.Jugador.Find(id);
            db.Jugador.Remove(jugador);
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

    */
    }
}