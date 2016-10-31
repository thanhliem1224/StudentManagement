using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLSinhVien.Models
{
    public class DangKyKhoaHoc
    {
        public int ID { get; set; }
        public int SinhVienID { get; set; }
        public int KhoaHocID { get; set; }

        public virtual SinhVien SinhVien { get; set; }
        public virtual KhoaHoc KhoaHoc { get; set; }
    }
}