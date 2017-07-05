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
using Vereyon.Web;

namespace StructuredDebate.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Claim,OpeningStatement,Score,Date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,Claim,OpeningStatement,Score,Date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected UserManager<ApplicationUser> UserManager { get; set; }

        //POST: Posts/Upvote/5
        [HttpPost]
        [Authorize]
        public PartialViewResult Vote(int id, string upOrDown)
        {
            Post post = db.Posts.Find(id);
            var userID = User.Identity.GetUserId();
            var userVote = new UserVote();
            //If an exception is thrown, it means the user has not voted on this thing yet, so go ahead and cast their vote
            try
            {
                userVote = db.UserVotes.Where(i => i.PostID == post.PostID).Where(i => i.UserID == userID).First();
            }
            catch
            {
                //Log vote in database
                var newUserVote = new UserVote();
                newUserVote.Vote = upOrDown;
                newUserVote.PostID = post.PostID;
                newUserVote.UserID = userID;
                db.UserVotes.Add(newUserVote);
                db.SaveChanges();

                //Update score
                if (upOrDown == "Up") { post.Score++; } else { post.Score--; }
            }
            
            if (userVote.PostID == post.PostID)
            {
                //They have already voted
                if (userVote.Vote == upOrDown)
                {
                    //They are not allowed to duplicate their vote
                    ViewBag.Score = post.Score.ToString() + "<br>Only one vote.";
                    return PartialView("_ScorePartial");
                }

                //However, they can change their vote
                userVote.Vote = upOrDown;
                db.UserVotes.Attach(userVote);
                var voteEntry = db.Entry(userVote);
                voteEntry.Property(v => v.Vote).IsModified = true;
                db.SaveChanges();

                //If they have already voted, we need to increment/decrement by two to correct it
                if (upOrDown == "Up") { post.Score = post.Score + 2; } else { post.Score = post.Score - 2; }
            }

            //Send the changes in
            var entry = db.Entry(post);
            entry.Property(e => e.Score).IsModified = true;
            db.SaveChanges();

            ViewBag.Score = post.Score;
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
