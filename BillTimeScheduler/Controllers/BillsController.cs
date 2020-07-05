using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BillTimeScheduler.Models;
using Microsoft.AspNet.Identity;

namespace BillTimeScheduler.Controllers
{
    [Authorize]
    public class BillsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // update the account information based on paycheck and bills due today
        public void UpdateDatabase()
        {
            List<decimal> BillsToday = new List<decimal>();

            //Update bill dates and populate BillsToday
            foreach (var bill in db.Bills)
            {
                bill.DueDate = VerifyMonth(bill.Day);
                if (bill.DueDate.Date == DateTime.Now.Date)
                {
                    BillsToday.Add(bill.Amount);
                }
            }

            // Update current balance
            foreach (var income in db.PayCheck)
            {
                // Add income if needed and update PayDate
                if (income.PayDate.Date == DateTime.Now.Date)
                {
                    income.Balance += income.Amount;
                    income.PayDate = income.PayDate.AddDays(income.Frequency);
                }

                // Subtract BillsToday from Current Balance
                foreach (var bill in BillsToday)
                {
                    income.Balance -= bill;
                }
            }
        }

        // Updates the month based on what day it is
        public DateTime VerifyMonth(int day)
        {
            if (day < DateTime.Now.Day)
            {
                return new DateTime(DateTime.Now.Year, (DateTime.Now.Month + 1), day);
            }
            else
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
            }
        }

        // GET: Bills
        public ActionResult Index()
        {
            UpdateDatabase();
            var model = db.Users.Find(User.Identity.GetUserId());
            return View(model);
        }

        // GET: Bills/BillsList
        public ActionResult BillsList()
        {
            UpdateDatabase();
            var UserId = User.Identity.GetUserId();
            var Bills = (from bill in db.Bills
                         where bill.ApplicationUserId == UserId
                         orderby bill.Day
                         select bill).ToList();
            return View(Bills);
        }

        //Get: Bills/SetIncome
        public ActionResult SetIncome()
        {
            return View();
        }

        // POST: Bills/SetIncome
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetIncome([Bind(Include = "Id,Balance,Amount,PayDate,Frequency,ApplicationUserId")] Income PayCheck)
        {
            string UserId = User.Identity.GetUserId();
            var CurrentUser = db.Users.Find(UserId);
            PayCheck.ApplicationUserId = UserId;

            if (ModelState.IsValid)
            {
                CurrentUser.PayCheck = PayCheck;
                db.Entry(CurrentUser).State = EntityState.Modified;
                db.SaveChanges();
                // Modify date if necessary
                UpdateDatabase();
                return RedirectToAction("Index");
            }

            return View(PayCheck);
        }

        // GET: Bills/Create
        public ActionResult Create(string List)
        {
            var model = db.Users.Find(User.Identity.GetUserId()).Bills;
            ViewBag.List = List;
            return View(model);
        }

        // POST: Bills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Amount,DueDate,Day,ApplicationUserId")] Bill bill, string List)
        {
            bill.ApplicationUserId = User.Identity.GetUserId();
            bill.DueDate = VerifyMonth(bill.Day);
            db.Bills.Add(bill);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction(List);
            }

            return View(bill);
        }

        // GET: Bills/Edit/5
        public ActionResult Edit(int? id, string List)
        {
            ViewBag.List = List;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Amount,Day,ApplicationUserId")] Bill bill, string List)
        {
            bill.ApplicationUserId = User.Identity.GetUserId();
            bill.DueDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: bill.Day);
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction(List);
            }
            return View(bill);
        }

        // GET: Bills/Delete/5
        public ActionResult Delete(int? id, string List)
        {
            ViewBag.List = List;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string List)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
            db.SaveChanges();
            return RedirectToAction(List);
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
