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
    public class KhoaHocMonHocController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult TimKhoaHoc(string tenKhoaHoc)
        {
            if (!string.IsNullOrEmpty(tenKhoaHoc))
            {
                var ds = db.KhoaHoc.Where(hs => hs.TenKhoaHoc.Contains(tenKhoaHoc));
                if (ds.Count() > 0) // nếu có kết quả
                {
                    TempData["Title"] = "Kết quả tìm kiếm " + tenKhoaHoc + " (" + ds.Count() + " kết quả)";
                    ViewBag.DSTimKiem = ds;
                }
                else
                {
                    TempData["Message_Fa"] = "Không tìm thấy lop \"" + tenKhoaHoc + "\"";
                }
            }
            return View();
        }


        [Authorize]
        public ActionResult KhoaHoc(int? id, string tenMonHoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaHoc khoaHoc = db.KhoaHoc.Find(id.Value);
            if (khoaHoc == null)
            {
                return HttpNotFound();
            }

            #region Tìm kiếm
            // tìm kiếm sách
            if (!string.IsNullOrEmpty(tenMonHoc))
            {

                var dskqmh = db.MonHoc.Where(s => s.TenMonHoc.Contains(tenMonHoc));


                if (dskqmh.Count() > 0)// neu có kết quả tìm kiếm
                {
                    TempData["Search_result"] = "Kết quả tìm kiếm \"" + tenMonHoc + "\" (" + dskqmh.Count() + " kết quả)";
                    ViewBag.KQTimKiemMH = dskqmh;
                }
                else
                {
                    TempData["Mess"] = "Không tìm thấy " + tenMonHoc;
                }
            }
            #endregion

            #region truy suất danh sách môn học trong khóa học
            // lấy danh sách môn học
            var dsmh = db.KhoaHocMonHoc.Where(m => m.KhoaHocID == id);
            // đưa dữ liệu danh sách môn học sang View
            if (dsmh.Count() > 0)
            {
                ViewBag.DSMonHoc = dsmh;
                ViewBag.DSMonHocCount = dsmh.Count();
            }
            else
            {
                ViewBag.DSMonHocCount = 0;
            }

            #endregion
            return View(khoaHoc);
        }


        // GET: KhoaHocMonHoc
        public ActionResult Index()
        {
            var khoaHocMonHoc = db.KhoaHocMonHoc.Include(k => k.KhoaHoc).Include(k => k.MonHoc);
            return View(khoaHocMonHoc.ToList());
        }

        // GET: KhoaHocMonHoc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaHoc_MonHoc khoaHoc_MonHoc = db.KhoaHocMonHoc.Find(id);
            if (khoaHoc_MonHoc == null)
            {
                return HttpNotFound();
            }
            return View(khoaHoc_MonHoc);
        }

        //// GET: KhoaHocMonHoc/Create
        //public ActionResult Create()
        //{
        //    ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc");
        //    ViewBag.MonHocID = new SelectList(db.MonHoc, "ID", "TenMonHoc");
        //    return View();
        //}

        // POST: KhoaHocMonHoc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,KhoaHocID,MonHocID")] KhoaHoc_MonHoc khoaHoc_MonHoc)
        {
            if (ModelState.IsValid)
            {
                string tenMonHoc = db.MonHoc.Find(khoaHoc_MonHoc.MonHocID).TenMonHoc;
                // kiểm tra exits
                var check = db.KhoaHocMonHoc.Where(k => k.KhoaHocID == khoaHoc_MonHoc.KhoaHocID).Where(k => k.MonHocID == khoaHoc_MonHoc.MonHocID);
                if (check.Count() > 0)
                {
                    TempData["Mess"] = "Môn học \"" + tenMonHoc + "\" đã có trong khóa học này";
                }
                else
                {
                    db.KhoaHocMonHoc.Add(khoaHoc_MonHoc);
                    db.SaveChanges();
                    TempData["Success"] = "Thêm thành công môn \"" + tenMonHoc + "\"";
                    return RedirectToAction("KhoaHoc", new { id = khoaHoc_MonHoc.KhoaHocID });
                }
            }
            return RedirectToAction("KhoaHoc", new { id = khoaHoc_MonHoc.KhoaHocID });
        }

        // GET: KhoaHocMonHoc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaHoc_MonHoc khoaHoc_MonHoc = db.KhoaHocMonHoc.Find(id);
            if (khoaHoc_MonHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", khoaHoc_MonHoc.KhoaHocID);
            ViewBag.MonHocID = new SelectList(db.MonHoc, "ID", "TenMonHoc", khoaHoc_MonHoc.MonHocID);
            return View(khoaHoc_MonHoc);
        }

        // POST: KhoaHocMonHoc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,KhoaHocID,MonHocID")] KhoaHoc_MonHoc khoaHoc_MonHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khoaHoc_MonHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", khoaHoc_MonHoc.KhoaHocID);
            ViewBag.MonHocID = new SelectList(db.MonHoc, "ID", "TenMonHoc", khoaHoc_MonHoc.MonHocID);
            return View(khoaHoc_MonHoc);
        }

        // GET: KhoaHocMonHoc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaHoc_MonHoc khoaHoc_MonHoc = db.KhoaHocMonHoc.Find(id);
            if (khoaHoc_MonHoc == null)
            {
                return HttpNotFound();
            }
            return View(khoaHoc_MonHoc);
        }

        // POST: KhoaHocMonHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhoaHoc_MonHoc khoaHoc_MonHoc = db.KhoaHocMonHoc.Find(id);
            db.KhoaHocMonHoc.Remove(khoaHoc_MonHoc);
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
