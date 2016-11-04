using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLSinhVien.Models
{
    public class KhoaHoc
    {
        public int ID { get; set; }
        [Display(Name = "Tên Khóa Học") , Required(ErrorMessage ="Vui lòng điền tên khóa học")]
        public string TenKhoaHoc { get; set; }
        [Display(Name = "Thời Gian Bắt Đầu"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ThoiGianBatDau { get; set; }
        [Display(Name = "Thời Gian Kết Thúc"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ThoiGianKetThuc { get; set; }

        public virtual ICollection<KhoaHoc_MonHoc> KhoaHocMonHoc { get; set; }
        public virtual ICollection<DangKyKhoaHoc> DangKyKhoaHoc { get; set; }
    }
}