using System.Globalization;
using Shodypati.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Shodypati.Filters;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;
using Shodypati.DAL;

namespace Shodypati.Controllers
{
    [Authorize]
    [ExceptionHandler]
    public class MedicinesController : BaseController
    {
        [Authorize(Roles = "Admin")]
        // GET: Medicines
        public ActionResult Index()
        {
            var userList = Db.MedicineListTbls.Select(x => new UserIds()
            {
              UserId=x.UserId
            }).Distinct().ToList();

            return View(userList);
        }
        [Authorize(Roles = "Admin")]
        // GET: Medicines/Details/5
        public ActionResult Details(string Id)
        {
            Guid userId = Guid.Parse(Id);

            var entity = new Medicine();

            var medicinelist = Db.MedicineListTbls.Where(x => x.UserId == userId).Select(x => new MedicineList()
            {
                MedicineName = x.MedicineName,
                Price = x.Price,
                Quantity = x.Quantity, 
                FinishTime = x.FinishTime,
            }).ToList();
            entity.UserId = userId;
            entity.medicineList = medicinelist;    
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        // GET: Medicines/Edit/5
        public ActionResult Edit(string Id)
        {
            var userId = Guid.Parse(Id);
            var entity = new Medicine();
            var medicinelist = Db.MedicineListTbls.Where(x => x.UserId == userId).Select(x => new MedicineList()
            {
                MedicineName = x.MedicineName,
                Price = x.Price,
                Quantity = x.Quantity,
                FinishTime = x.FinishTime,
            }).ToList();

            entity.medicineList = medicinelist;
            entity.UserId = userId;

            return View(entity);
        }
        [Authorize(Roles = "Admin")]
        // POST: Medicines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Medicine entity)
        {
            
                ModelState.Clear();
                var medicinelist = new List<MedicineList>();

                DeleteFromMedicineTbl(entity.UserId);

                int i = 0;
                if (entity.MedicineName != null)
                {
                    foreach (var item in entity.MedicineName)
                    {
                        string medicineName = string.Empty;
                        int? price = 0;
                        string quantity = string.Empty;

                    if (!string.IsNullOrEmpty(item))
                        {
                            if (entity.MedicineName[i] != null && entity.MedicineName[i] != string.Empty)
                            {
                                medicineName = entity.MedicineName[i];
                            }
                            if (entity.Quantity[i] != null && entity.Quantity[i] != string.Empty)
                            {
                                quantity = entity.Quantity[i];
                            }

                        if (entity.Price[i] != 0)
                            {
                                price = entity.Price[i];
                            }

                            DateTime? finishTime = entity.FinishTime[i];
                            SaveToMedicineList(medicineName, quantity, price, finishTime, entity.UserId);
                        }
                        i++;
                    }
                }            
            return RedirectToAction("Index");
        }

        public void DeleteFromMedicineTbl(Guid userId)
        {
            var query = from x in Db.MedicineListTbls
                        where x.UserId == userId
                        select x;

            foreach (var item in query.ToList())
            {
                Db.MedicineListTbls.DeleteOnSubmit(item);
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }
        [Authorize(Roles = "Patient")]
        // GET: Medicines/Create
        public ActionResult Create()
        {
            var entity = new Medicine();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var userId = Guid.Parse(user.Id);
            var medicinelist = Db.MedicineListTbls.Where(x => x.UserId == userId).Select(x => new MedicineList()
            {
                MedicineName = x.MedicineName,
                Price = x.Price,
                FinishTime = x.FinishTime,
                Quantity =x.Quantity,
            }).ToList();
            
            entity.medicineList = medicinelist;
            entity.UserId = userId;

            return View(entity);
        }

        // POST: Medicines/Create
        [Authorize(Roles = "Patient")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medicine entity)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();

                DeleteFromMedicineTbl(entity.UserId);
           
                int i = 0;
                if (entity.MedicineName != null)
                {
                    foreach(var item in entity.MedicineName)
                    {
                        string medicineName = string.Empty;
                        int? price = 0;
                        string quantity = string.Empty;
                        if (!string.IsNullOrEmpty(item))
                        {
                            if (entity.MedicineName[i] != null && entity.MedicineName[i] != string.Empty)
                            {
                                medicineName = entity.MedicineName[i];
                            }
                            if (entity.Quantity[i] != null && entity.Quantity[i] != string.Empty)
                            {
                                quantity = entity.Quantity[i];
                            }
                            if (entity.Price[i] != 0)
                            {
                                price = entity.Price[i];
                            }

                            DateTime? finishTime = entity.FinishTime[i];
                            SaveToMedicineList(medicineName, quantity, price, finishTime, entity.UserId);                          
                        }
                        i++;
                    }                   
                }

                var medicinelist = Db.MedicineListTbls.Where(x => x.UserId == entity.UserId).Select(x => new MedicineList()
                {
                    MedicineName = x.MedicineName,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    FinishTime = x.FinishTime,
                }).ToList();
                entity.medicineList = medicinelist;
            }
            return View(entity);
        }

        public void SaveToMedicineList(string medicineName, string quantity, int? price,DateTime? finishTime,Guid userId)
        {
            Db.MedicineListTbls.InsertOnSubmit(new MedicineListTbl
            {
                UserId=userId,
                MedicineName=medicineName,
                Quantity = quantity,
                Price =price,
                FinishTime=finishTime,
            });
            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Medicines/Delete/5
        public ActionResult Delete(string Id)
        {
            var userId = Guid.Parse(Id);

            Medicine entity = new Medicine {UserId = userId};
            var medicinelist = Db.MedicineListTbls.Where(x => x.UserId == userId).Select(x => new MedicineList()
            {
                MedicineName = x.MedicineName,
                Price = x.Price,
                Quantity =x.Quantity,
                FinishTime = x.FinishTime,
            }).ToList();
            entity.UserId = userId;
            entity.medicineList = medicinelist;
            return View(entity);
        }
        [Authorize(Roles = "Admin")]
        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string Id)
        {
            var userId = Guid.Parse(Id);
            var query = from x in Db.MedicineListTbls
                        where x.UserId == userId
                        select x;

            foreach (var item in query.ToList())
            {
                Db.MedicineListTbls.DeleteOnSubmit(item);
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
