using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLSinhVien.Models
{
    public class SinhVien
    {
        public int ID { get; set; }
        [Display(Name = "Họ")]
        public string Ho { get; set; }
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        [Display(Name = "Giới Tính")]
        public string GioiTinh { get; set; }
        [Display(Name = "Ngày Sinh"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime NgaySinh { get; set; }
        [Display(Name = "Nơi Sinh")]
        public string NoiSinh { get; set; }
        [Display(Name = "Địa Chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Số Điện Thoại")]
        public string SoDienThoai { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Họ và Tên")]
        public string HoVaTen { get { return Ho + " " + Ten; } }

        public virtual ICollection<DangKyKhoaHoc> DangKyKhoaHoc { get; set; }

    }
}