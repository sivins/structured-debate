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
using Microsoft.AspNet.Identity;

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

        //POST: CrossExaminations/Vote/5
        [HttpPost]
        [Authorize]
        public PartialViewResult Vote(int id, string upOrDown)
        {
            CrossExamination crossExamination = db.CrossExaminations.Find(id);
            var userID = User.Identity.GetUserId();
            var userVote = new UserVote();
            //If an exception is thrown, it means the user has not voted on this thing yet, so go ahead and cast their vote
            try
            {
                userVote = db.UserVotes.Where(i => i.CrossExaminationID == crossExamination.CrossExaminationID).Where(i => i.UserID == userID).First();
            }
            catch
            {
                //Log vote in database
                var newUserVote = new UserVote();
                newUserVote.Vote = upOrDown;
                newUserVote.CrossExaminationID = crossExamination.CrossExaminationID;
                newUserVote.UserID = userID;
                db.UserVotes.Add(newUserVote);
                db.SaveChanges();

                //Update score
                if (upOrDown == "Up") { crossExamination.Score++; } else { crossExamination.Score--; }
            }
            
            if (userVote.CrossExaminationID == crossExamination.CrossExaminationID)
            {
                //They have already voted
                if (userVote.Vote == upOrDown)
                {
                    //They are not allowed to duplicate their vote
                    ViewBag.Score = crossExamination.Score.ToString() + "<br>Only one vote.";
                    return PartialView("_ScorePartial");
                }

                //However, they can change their vote
                userVote.Vote = upOrDown;
                db.UserVotes.Attach(userVote);
                var voteEntry = db.Entry(userVote);
                voteEntry.Property(v => v.Vote).IsModified = true;
                db.SaveChanges();

                //If they have already voted, we need to increment/decrement by two to correct it
                if (upOrDown == "Up") { crossExamination.Score = crossExamination.Score + 2; } else { crossExamination.Score = crossExamination.Score - 2; }
            }

            //Send the changes in
            var entry = db.Entry(crossExamination);
            entry.Property(e => e.Score).IsModified = true;
            db.SaveChanges();

            ViewBag.Score = crossExamination.Score;
            return PartialView("_ScorePartial");
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
