using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLSinhVien.Models
{
    public class KhoaHoc_MonHoc
    {
        public int ID { get; set; }
        public int KhoaHocID { get; set; }
        public int MonHocID { get; set; }

        public virtual KhoaHoc KhoaHoc { get; set; }
        public virtual MonHoc MonHoc { get; set; }
    }
}