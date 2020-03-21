using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cat_in_the_sack.Models;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace Cat_in_the_sack.Controllers
{
    public class MoviesController : Controller
    {
        private MoviesEntities1 db = new MoviesEntities1();




        // GET: Movies
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WatchResult(string genre, string prevTitle="")
        {
            foreach(Movy movy in db.Movies)
            {
                if(movy.Title == prevTitle)
                {
                    movy.IsPicked = 1;
                }
            }

            db.SaveChanges();

            string title="";
            Random rndMovieIndex = new Random();

            if (db.Movies.All(_movie => _movie.IsPicked == 1))
            {
                foreach (Movy movy in db.Movies)
                {
                    movy.IsPicked = 0;
                }
            }

            if (db.Movies.All(_movie => _movie.IsWatched == 1))
            {
                foreach (Movy movy in db.Movies)
                {
                    movy.IsWatched = 0;
                }
            }

            db.SaveChanges();

            while (String.IsNullOrEmpty(title))
            {
                Movy movie = db.Movies.ToList().ElementAt(rndMovieIndex.Next(1, db.Movies.ToList().Count()));  
                
                if( (movie.Genre==genre || genre=="Random" ) && movie.IsWatched==0 && movie.IsPicked == 0)
                {
                    title = movie.Title;
                }

                ViewBag.Id = movie.Id;
            }

            ViewBag.Genre = genre;
            ViewBag.ResultMovie = title;
            return View();
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Genre,Title,IsWatched")] Movy movy)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movy);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Genre,Title,IsWatched")] Movy movy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movy);
        }

       
        public ActionResult Watched(int id)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Find(id).IsWatched = 1;
                db.SaveChanges();
                return View();
            }
            return View();
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movy movy = db.Movies.Find(id);
            db.Movies.Remove(movy);
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
