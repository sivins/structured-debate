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

        //POST: Arguments/Vote/5
        [HttpPost]
        [Authorize]
        public PartialViewResult Vote(int id, string upOrDown)
        {
            Argument argument = db.Arguments.Find(id);
            var userID = User.Identity.GetUserId();
            var userVote = new UserVote();
            //If an exception is thrown, it means the user has not voted on this thing yet, so go ahead and cast their vote
            try
            {
                userVote = db.UserVotes.Where(i => i.ArgumentID == argument.ArgumentID).Where(i => i.UserID == userID).First();
            }
            catch
            {
                //Log vote in database
                var newUserVote = new UserVote();
                newUserVote.Vote = upOrDown;
                newUserVote.ArgumentID = argument.ArgumentID;
                newUserVote.UserID = userID;
                db.UserVotes.Add(newUserVote);
                db.SaveChanges();

                //Update score
                if (upOrDown == "Up") { argument.Score++; } else { argument.Score--; }
            }
            
            if (userVote.ArgumentID == argument.ArgumentID)
            {
                //They have already voted
                if (userVote.Vote == upOrDown)
                {
                    //They are not allowed to duplicate their vote
                    ViewBag.Score = argument.Score.ToString() + "<br>Only one vote.";
                    return PartialView("_ScorePartial");
                }

                //However, they can change their vote
                userVote.Vote = upOrDown;
                db.UserVotes.Attach(userVote);
                var voteEntry = db.Entry(userVote);
                voteEntry.Property(v => v.Vote).IsModified = true;
                db.SaveChanges();

                //If they have already voted, we need to increment/decrement by two to correct it
                if (upOrDown == "Up") { argument.Score = argument.Score + 2; } else { argument.Score = argument.Score - 2; }
            }

            //Send the changes in
            var entry = db.Entry(argument);
            entry.Property(e => e.Score).IsModified = true;
            db.SaveChanges();

            ViewBag.Score = argument.Score;
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
