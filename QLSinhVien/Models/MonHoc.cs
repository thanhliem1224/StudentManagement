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
        [Display(Name = "Tên Môn Học"), Required(ErrorMessage = "Vui lòng điền tên môn học")]
        public string TenMonHoc { get; set; }

        public virtual ICollection<KhoaHoc_MonHoc> KhoaHocMonHoc { get; set; }
    }
}