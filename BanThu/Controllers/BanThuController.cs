using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanThu.Models;
using PagedList;
using PagedList.Mvc;


namespace BanThu.Controllers
{
    public class BanThuController : Controller
    {
       
        //Tao 1 doi tuong chua tong bo CSDL tu dbBanThu
        dbBanThuDataContext data = new dbBanThuDataContext();
        private List<Thu> Layspmoi(int count)
        {
            // Sap xep giam dan theo ngay cap nhat, lay count dong dau
            return data.Thus.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: BanThu
        public ActionResult Index(int? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var giaymoi = Layspmoi(12);//lay  loai giay cap nhat moi nhat
            return View(giaymoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Loai()
        {
            var loai = from tl in data.Loais select tl;
            return PartialView(loai);
        }
        public ActionResult NhaCungCap()
        {
            var loai = from tl in data.Loais select tl;
            return PartialView(loai);
        }
        public ActionResult SPTheoLoai(int id)
        {
            var thu = from t in data.Thus where t.MaLoai == id select t;
            return View(thu);

        }
        public ActionResult TimkiembangTen(string searchString)
        {
            var thu = data.Thus.Where(x => x.TenSanPham.Contains(searchString));
            return View(thu.ToList());
        }
        public ActionResult SPTheoNcc(int id)
        {
            var thu = from t in data.Thus where t.MaNcc == id select t;
            return View(thu);
        }
        public ActionResult Details(int id)
        {
            var thu = from t in data.Thus
                      where t.MaSanPham == id
                      select t;
            return View(thu.Single());

        }
        
    }
}