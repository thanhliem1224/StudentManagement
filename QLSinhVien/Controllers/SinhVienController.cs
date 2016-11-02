using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLSinhVien.Models;
using System.IO;
using Excel;

namespace QLSinhVien.Controllers
{
    public class SinhVienController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult HienTatCa()
        {
            return RedirectToAction("Index", "SinhVien", new { show_all = true });
        }

        [Authorize]
        // GET: SinhVien
        public ActionResult Index(string ten, bool? show_all)
        {
            if (!show_all.HasValue)
            {


                if (!string.IsNullOrEmpty(ten))
                {
                    var ds = db.SinhVien.Select(hs => hs);
                    if (!string.IsNullOrEmpty(ten))
                    {
                        ds = db.SinhVien.Where(hs => hs.Ten.Contains(ten));
                    }
                    if (ds.Count() > 0) // nếu có kết quả
                    {
                        //TempData["Title"] = "Kết quả tìm kiếm " + tenHS + " - " + lopHS + " (" + ds.Count() + " kết quả)";
                        ViewBag.SinhVien = ds;
                    }
                    else
                    {
                        TempData["Message_Fa"] = "Không tìm thấy sinh viên " + ten;
                    }
                }
            }
            else
            {
                var ds = db.SinhVien.Select(hs => hs);

                ViewBag.SinhVien = ds;
            }
            return View();
        }

        [Authorize]
        // GET: SinhVien/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhVien.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            return View(sinhVien);
        }
        [Authorize]
        // GET: SinhVien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SinhVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ho,Ten,GioiTinh,NgaySinh,NoiSinh,DiaChi,SoDienThoai,Email")] SinhVien sinhVien)
        {
            if (ModelState.IsValid)
            {
                db.SinhVien.Add(sinhVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sinhVien);
        }
        [Authorize]
        public ActionResult ThemTuFile()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ThemTuFile(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.InputStream;

                    // We return the interface, so that
                    IExcelDataReader reader = null;


                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Định dạng file không được hỗ trợ.");
                        return View();
                    }

                    reader.IsFirstRowAsColumnNames = true;

                    DataSet result = reader.AsDataSet();

                    for (int i = 0; i < result.Tables.Count; i++)
                    {
                        foreach (DataRow row in result.Tables[i].Rows)
                        {
                            DateTime ngaySinh;
                            // chuyển date từ số sang date nếu là file excel 97
                            if (upload.FileName.EndsWith(".xls"))
                            {
                                double dateNumber = double.Parse(row[4].ToString());
                                ngaySinh = DateTime.FromOADate(dateNumber);
                            }
                            else // nếu là file xlsx thì giữ nguyên
                            {
                                ngaySinh = DateTime.Parse(row[4].ToString());
                            }
                            SinhVien sv = new SinhVien
                            {
                                Ho = row[1].ToString(),
                                Ten = row[2].ToString(),
                                GioiTinh = row[3].ToString(),
                                NgaySinh = ngaySinh,
                                NoiSinh = row[5].ToString(),
                                DiaChi = row[6].ToString(),
                                SoDienThoai = row[7].ToString(),
                                Email = row[8].ToString(),
                            };

                            if (db.SinhVien.Any(hs => hs.NgaySinh == sv.NgaySinh && hs.Ho == sv.Ho && hs.Ten == sv.Ten))
                            {

                            }
                            else
                            {
                                try
                                {
                                    db.SinhVien.Add(sv);
                                    db.SaveChanges();
                                }
                                catch (Exception)
                                {

                                }

                            }
                        }
                    }
                    reader.Close();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("File", "Vui lòng chọn file!");
                }

            }
            return View();
        }

        [Authorize]
        // GET: SinhVien/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhVien.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            return View(sinhVien);
        }

        // POST: SinhVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ho,Ten,GioiTinh,NgaySinh,NoiSinh,DiaChi,SoDienThoai,Email")] SinhVien sinhVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sinhVien);
        }
        [Authorize]
        // GET: SinhVien/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhVien.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            return View(sinhVien);
        }

        // POST: SinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SinhVien sinhVien = db.SinhVien.Find(id);
            db.SinhVien.Remove(sinhVien);
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
