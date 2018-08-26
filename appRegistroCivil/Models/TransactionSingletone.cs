using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace appRegistroCivil.Models
{

        public class TransactionSingletone
        {
            private static volatile TransactionSingletone instance = null;
            public static RegistroCivilEntities db;
            private static DbContextTransaction Transaction;


            public static TransactionSingletone Instance()
            {
                if (instance == null)
                {
                    instance = new TransactionSingletone();
                }
                return instance;
            }

            private TransactionSingletone()
            {
                db = new RegistroCivilEntities();
                Transaction = db.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

            }
            public static void UploadPerson(Persona person)
            {
                db.Persona.Add(person);
            }
            public static void SendCommit() {

            try
            {
                db.SaveChanges();
                Transaction.Commit();
            }
            catch
            {
            }
            finally {
                ResetInstance();
            }
            }
            public static void ResetInstance()
            {
                instance = null;
                instance = new TransactionSingletone();
            }
        }
    }
