using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Controllers
{
    public class MonHocController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        // GET: MonHoc
        public ActionResult Index()
        {
            var monHoc = db.MonHoc.Include(m => m.KhoaHoc);
            return View(monHoc.ToList());
        }
        [Authorize]
        // GET: MonHoc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }
        [Authorize]
        // GET: MonHoc/Create
        public ActionResult Create()
        {
            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc");
            return View();
        }

        // POST: MonHoc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenMonHoc,KhoaHocID")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                db.MonHoc.Add(monHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", monHoc.KhoaHocID);
            return View(monHoc);
        }

        // GET: MonHoc/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", monHoc.KhoaHocID);
            return View(monHoc);
        }

        // POST: MonHoc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenMonHoc,KhoaHocID")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", monHoc.KhoaHocID);
            return View(monHoc);
        }

        // GET: MonHoc/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHoc.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }

        // POST: MonHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonHoc monHoc = db.MonHoc.Find(id);
            db.MonHoc.Remove(monHoc);
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
