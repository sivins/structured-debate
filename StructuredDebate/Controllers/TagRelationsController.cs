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
    public class TagRelationsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: TagRelations
        public ActionResult Index()
        {
            var tagRelations = db.TagRelations.Include(t => t.Post).Include(t => t.Tag);
            return View(tagRelations.ToList());
        }

        // GET: TagRelations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagRelation tagRelation = db.TagRelations.Find(id);
            if (tagRelation == null)
            {
                return HttpNotFound();
            }
            return View(tagRelation);
        }

        // GET: TagRelations/Create
        public ActionResult Create()
        {
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim");
            ViewBag.TagID = new SelectList(db.Tags, "TagID", "Name");
            return View();
        }

        // POST: TagRelations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TagRelationID,TagID,PostID")] TagRelation tagRelation)
        {
            if (ModelState.IsValid)
            {
                db.TagRelations.Add(tagRelation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim", tagRelation.PostID);
            ViewBag.TagID = new SelectList(db.Tags, "TagID", "Name", tagRelation.TagID);
            return View(tagRelation);
        }

        // GET: TagRelations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagRelation tagRelation = db.TagRelations.Find(id);
            if (tagRelation == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim", tagRelation.PostID);
            ViewBag.TagID = new SelectList(db.Tags, "TagID", "Name", tagRelation.TagID);
            return View(tagRelation);
        }

        // POST: TagRelations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TagRelationID,TagID,PostID")] TagRelation tagRelation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tagRelation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Claim", tagRelation.PostID);
            ViewBag.TagID = new SelectList(db.Tags, "TagID", "Name", tagRelation.TagID);
            return View(tagRelation);
        }

        // GET: TagRelations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagRelation tagRelation = db.TagRelations.Find(id);
            if (tagRelation == null)
            {
                return HttpNotFound();
            }
            return View(tagRelation);
        }

        // POST: TagRelations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TagRelation tagRelation = db.TagRelations.Find(id);
            db.TagRelations.Remove(tagRelation);
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
