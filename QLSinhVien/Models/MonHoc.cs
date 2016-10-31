using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLSinhVien.Models
{
    public class MonHoc
    {
        public int ID { get; set; }
        [Display(Name = "Tên Môn Học")]
        public string TenMonHoc { get; set; }

        public int KhoaHocID { get; set; }

        public virtual KhoaHoc KhoaHoc { get; set; }
    }
}