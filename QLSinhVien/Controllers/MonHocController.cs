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
        public ActionResult TimKhoaHoc(string tenKhoaHoc)
        {
            if (!string.IsNullOrEmpty(tenKhoaHoc))
            {
                var dstk = db.KhoaHoc.Where(k => k.TenKhoaHoc.Contains(tenKhoaHoc));
                if (dstk.Count() > 0) // nếu có kết quả
                {
                    TempData["Title"] = "Kết quả tìm kiếm " + tenKhoaHoc + "(" + dstk.Count() + " kết quả)";
                    ViewBag.DSTimKiemKhoaHoc = dstk;
                }
                else
                {
                    TempData["Message_Fa"] = "Không tìm thấy khóa học \"" + tenKhoaHoc + "\"";
                }

            }
            return View();
        }



        //[Authorize]
        //// GET: MonHoc/Create
        //public ActionResult Create(string tenKhoaHoc, string tenMonHoc)
        //{
        //    return View();
        //}

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
                return RedirectToAction("Details", "KhoaHoc", new { id = monHoc.KhoaHocID});
            }

            //ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", monHoc.KhoaHocID);
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
