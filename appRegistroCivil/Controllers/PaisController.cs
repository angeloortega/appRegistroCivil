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