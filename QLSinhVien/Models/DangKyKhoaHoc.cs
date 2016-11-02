using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLSinhVien.Models
{
    public class DangKyKhoaHoc
    {
        public int ID { get; set; }
        [Display(Name = "Tên Sinh Viên")]
        public int SinhVienID { get; set; }
        [Display(Name = "Tên Khóa Học")]
        public int KhoaHocID { get; set; }

        public virtual SinhVien SinhVien { get; set; }
        public virtual KhoaHoc KhoaHoc { get; set; }
    }
}