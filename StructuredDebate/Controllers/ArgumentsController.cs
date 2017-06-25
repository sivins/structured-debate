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
    public class ArgumentsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Arguments
        public ActionResult Index()
        {
            var arguments = db.Arguments.Include(a => a.Post);
            return View(arguments.ToList());
        }

        // GET: Arguments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Argument argument = db.Arguments.Find(id);
            if (argument == null)
            {
                return HttpNotFound();
            }
            return View(argument);
        }

        // GET: Arguments/Create
        public ActionResult Create()
        {
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim");
            return View();
        }

        // POST: Arguments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArgumentID,PostID,Body,Affirmative,Score")] Argument argument)
        {
            if (ModelState.IsValid)
            {
                db.Arguments.Add(argument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim", argument.PostID);
            return View(argument);
        }

        // GET: Arguments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Argument argument = db.Arguments.Find(id);
            if (argument == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim", argument.PostID);
            return View(argument);
        }

        // POST: Arguments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArgumentID,PostID,Body,Affirmative,Score")] Argument argument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(argument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim", argument.PostID);
            return View(argument);
        }

        // GET: Arguments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Argument argument = db.Arguments.Find(id);
            if (argument == null)
            {
                return HttpNotFound();
            }
            return View(argument);
        }

        // POST: Arguments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Argument argument = db.Arguments.Find(id);
            db.Arguments.Remove(argument);
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
