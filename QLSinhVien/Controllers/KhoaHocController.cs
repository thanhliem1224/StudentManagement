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
    public class KhoaHocController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: KhoaHoc
        [Authorize]
        public ActionResult Index()
        {
            return View(db.KhoaHoc.ToList());
        }

        // GET: KhoaHoc/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaHoc khoaHoc = db.KhoaHoc.Find(id);
            if (khoaHoc == null)
            {
                return HttpNotFound();
            }

            #region truy suất danh sách môn học trong khóa học
            // lấy danh sách môn học
            var dsmh = db.KhoaHocMonHoc.Where(m => m.KhoaHocID == id);
            // đưa dữ liệu danh sách môn học sang View
            if (dsmh.Count() > 0)
            {
                ViewBag.DSMonHoc = dsmh;
                @TempData["SLMonHoc"] = dsmh.Count();
            }
            else
            {
                @TempData["SLMonHoc"] = 0;
            }

            #endregion

            #region truy suất danh sách sinh viên trong khóa học
            // lấy danh sách môn học
            var dssv = db.DangKyKhoaHoc.Where(m => m.KhoaHocID == id);
            // đưa dữ liệu danh sách môn học sang View
            if (dssv.Count() > 0)
            {
                ViewBag.DSSinhVien = dssv;
                @TempData["SLSinhVien"] = dssv.Count();
            }
            else
            {
                @TempData["SLSinhVien"] = 0;
            }

            #endregion

            return View(khoaHoc);
        }

        // GET: KhoaHoc/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhoaHoc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenKhoaHoc,ThoiGianBatDau,ThoiGianKetThuc")] KhoaHoc khoaHoc)
        {
            if (ModelState.IsValid)
            {
                db.KhoaHoc.Add(khoaHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khoaHoc);
        }

        // GET: KhoaHoc/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaHoc khoaHoc = db.KhoaHoc.Find(id);
            if (khoaHoc == null)
            {
                return HttpNotFound();
            }
            return View(khoaHoc);
        }

        // POST: KhoaHoc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenKhoaHoc,ThoiGianBatDau,ThoiGianKetThuc")] KhoaHoc khoaHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khoaHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khoaHoc);
        }

        // GET: KhoaHoc/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaHoc khoaHoc = db.KhoaHoc.Find(id);
            if (khoaHoc == null)
            {
                return HttpNotFound();
            }
            return View(khoaHoc);
        }

        // POST: KhoaHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhoaHoc khoaHoc = db.KhoaHoc.Find(id);
            db.KhoaHoc.Remove(khoaHoc);
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
