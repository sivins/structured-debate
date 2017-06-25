using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StructuredDebate.DAL;
using StructuredDebate.Models;

namespace StructuredDebate.Controllers
{
    public class CrossExaminationsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: CrossExaminations
        public ActionResult Index()
        {
            var crossExaminations = db.CrossExaminations.Include(c => c.Argument);
            return View(crossExaminations.ToList());
        }

        // GET: CrossExaminations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrossExamination crossExamination = db.CrossExaminations.Find(id);
            if (crossExamination == null)
            {
                return HttpNotFound();
            }
            return View(crossExamination);
        }

        // GET: CrossExaminations/Create
        public ActionResult Create()
        {
            ViewBag.ArgumentID = new SelectList(db.Arguments, "ArgumentID", "Body");
            return View();
        }

        // POST: CrossExaminations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CrossExaminationID,ArgumentID,Body,Score")] CrossExamination crossExamination)
        {
            if (ModelState.IsValid)
            {
                db.CrossExaminations.Add(crossExamination);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArgumentID = new SelectList(db.Arguments, "ArgumentID", "Body", crossExamination.ArgumentID);
            return View(crossExamination);
        }

        // GET: CrossExaminations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrossExamination crossExamination = db.CrossExaminations.Find(id);
            if (crossExamination == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArgumentID = new SelectList(db.Arguments, "ArgumentID", "Body", crossExamination.ArgumentID);
            return View(crossExamination);
        }

        // POST: CrossExaminations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CrossExaminationID,ArgumentID,Body,Score")] CrossExamination crossExamination)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crossExamination).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArgumentID = new SelectList(db.Arguments, "ArgumentID", "Body", crossExamination.ArgumentID);
            return View(crossExamination);
        }

        // GET: CrossExaminations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrossExamination crossExamination = db.CrossExaminations.Find(id);
            if (crossExamination == null)
            {
                return HttpNotFound();
            }
            return View(crossExamination);
        }

        // POST: CrossExaminations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CrossExamination crossExamination = db.CrossExaminations.Find(id);
            db.CrossExaminations.Remove(crossExamination);
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
