using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BanThu.Models
{
    public class GioHang
    {
        //Tạo đối tượng data chứa liệu từ model dbBanThu đã tạo
        dbBanThuDataContext data = new dbBanThuDataContext();
        public int iMaSanPham { set; get; }
        public string sTenSanPham { set; get; }
        public string sAnhBia { set; get; }
        public Double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        //khoi tao gio hang truyen vao 1
        public GioHang(int MaSanPham)
        {
            iMaSanPham = MaSanPham;
            Thu thu = data.Thus.Single(n => n.MaSanPham == iMaSanPham);
            sTenSanPham = thu.TenSanPham;
            sAnhBia = thu.AnhBia;
            dDonGia = double.Parse(thu.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}

