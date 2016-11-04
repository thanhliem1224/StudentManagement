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
    public class DangKyKhoaHocController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DangKyKhoaHoc
        [Authorize]
        public ActionResult Index()
        {
            var dangKyKhoaHoc = db.DangKyKhoaHoc.Include(d => d.KhoaHoc).Include(d => d.SinhVien);
            return View(dangKyKhoaHoc.ToList());
        }

        // GET: DangKyKhoaHoc/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyKhoaHoc dangKyKhoaHoc = db.DangKyKhoaHoc.Find(id);
            if (dangKyKhoaHoc == null)
            {
                return HttpNotFound();
            }
            return View(dangKyKhoaHoc);
        }

        // GET: DangKyLop/TimKhoaHoc
        [Authorize]
        public ActionResult TimKhoaHoc(string tenKhoaHoc)
        {
            if (!string.IsNullOrEmpty(tenKhoaHoc))
            {
                var ds = db.KhoaHoc.Where(hs => hs.TenKhoaHoc.Contains(tenKhoaHoc));
                if (ds.Count() > 0) // nếu có kết quả
                {
                    TempData["Title"] = "Kết quả tìm kiếm \"" + tenKhoaHoc + "\" (" + ds.Count() + " kết quả)";
                    ViewBag.DSTimKiem = ds;
                }
                else
                {
                    TempData["Message_Fa"] = "Không tìm thấy khóa học \"" + tenKhoaHoc + "\"";
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult KhoaHoc(int? id, string tenSV)
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
            if (!string.IsNullOrEmpty(tenSV))
            {

                var dskqsv = db.SinhVien.Where(s => s.Ten.Contains(tenSV) || s.Ho.Contains(tenSV));


                if (dskqsv.Count() > 0)// neu có kết quả tìm kiếm
                {
                    TempData["Search_result"] = "Kết quả tìm kiếm \"" + tenSV + "\" (" + dskqsv.Count() + " kết quả)";
                    ViewBag.KQTimKiemSV = dskqsv;
                }
                else
                {
                    TempData["Mess"] = "Không tìm thấy sinh vên \"" + tenSV + "\"";
                }
            }
            #endregion

            #region truy suất danh sách sinh viên kho khóa học
            // lấy danh sách sách đang mượn
            var dssv = db.DangKyKhoaHoc.Where(m => m.KhoaHocID == id);
            // đưa dữ liệu sách đang mượn sang View
            if (dssv.Count() > 0)
            {
                ViewBag.DSSinhVien = dssv;
                ViewBag.DSSinhVienCount = dssv.Count();
            }
            else
            {
                ViewBag.DSSinhVienCount = 0;
            }

            #endregion
            return View(khoaHoc);
        }


        //// GET: DangKyKhoaHoc/Create
        //public ActionResult Create()
        //{
        //    ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc");
        //    ViewBag.SinhVienID = new SelectList(db.SinhVien, "ID", "Ho");
        //    return View();
        //}

        // POST: DangKyKhoaHoc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SinhVienID,KhoaHocID")] DangKyKhoaHoc dangKyKhoaHoc)
        {
            if (ModelState.IsValid)
            {
                string tenSinhVien = db.SinhVien.Find(dangKyKhoaHoc.SinhVienID).HoVaTen;
                // kiem tra exits
                var check = db.DangKyKhoaHoc.Where(d => d.KhoaHocID == dangKyKhoaHoc.KhoaHocID).Where(d => d.SinhVienID == dangKyKhoaHoc.SinhVienID);
                if (check.Count() > 0)
                {
                    TempData["Mess"] = "Sinh viên \"" + tenSinhVien + "\" đã tham gia vào khóa học này";
                }
                else
                {
                    db.DangKyKhoaHoc.Add(dangKyKhoaHoc);
                    db.SaveChanges();
                    TempData["Success"] = "Thêm thành công sinh viên \"" + tenSinhVien + "\"";
                    return RedirectToAction("KhoaHoc", "DangKyKhoaHoc", new { id = dangKyKhoaHoc.KhoaHocID });
                }
            }
            return RedirectToAction("KhoaHoc", "DangKyKhoaHoc", new { id = dangKyKhoaHoc.KhoaHocID });
        }

        // GET: DangKyKhoaHoc/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyKhoaHoc dangKyKhoaHoc = db.DangKyKhoaHoc.Find(id);
            if (dangKyKhoaHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", dangKyKhoaHoc.KhoaHocID);
            ViewBag.SinhVienID = new SelectList(db.SinhVien, "ID", "Ho", dangKyKhoaHoc.SinhVienID);
            return View(dangKyKhoaHoc);
        }

        // POST: DangKyKhoaHoc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SinhVienID,KhoaHocID")] DangKyKhoaHoc dangKyKhoaHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dangKyKhoaHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoaHocID = new SelectList(db.KhoaHoc, "ID", "TenKhoaHoc", dangKyKhoaHoc.KhoaHocID);
            ViewBag.SinhVienID = new SelectList(db.SinhVien, "ID", "Ho", dangKyKhoaHoc.SinhVienID);
            return View(dangKyKhoaHoc);
        }

        // GET: DangKyKhoaHoc/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyKhoaHoc dangKyKhoaHoc = db.DangKyKhoaHoc.Find(id);
            if (dangKyKhoaHoc == null)
            {
                return HttpNotFound();
            }
            return View(dangKyKhoaHoc);
        }

        // POST: DangKyKhoaHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DangKyKhoaHoc dangKyKhoaHoc = db.DangKyKhoaHoc.Find(id);
            db.DangKyKhoaHoc.Remove(dangKyKhoaHoc);
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
