using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using BanThu.Models;

namespace BanThu.Controllers
{
    public class AdminController : Controller
    {
        dbBanThuDataContext data = new dbBanThuDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //Gan cac gia tri nguoi dung nhap lieu cho cac bien
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = " Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gan gia tri cho doi tuong duoc tao moi(ad)
                Admin ad = data.Admins.SingleOrDefault(n => n.UserName == tendn && n.PassWord== matkhau);
                if (ad != null)
                {
                    //ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                   
                    return RedirectToAction("Index", "Admin");

                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";

            }
            return View();

        }
        public ActionResult Thu(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //  return View(db.Thus.ToList());
            return View(data.Thus.ToList().OrderBy(n => n.MaSanPham).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ThemmoiSanpham()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNcc), "MaNcc", "TenNcc");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSanpham(Thu thu, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNcc= new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNcc), "MaNcc", "TenNcc");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    thu.AnhBia = fileName;
                    thu.NgayCapNhat = DateTime.Now;
                    //Luu vao CSDL
                    data.Thus.InsertOnSubmit(thu);
                    data.SubmitChanges();
                }
                return RedirectToAction("Thu");
            }
        }

        //Hiển thị sản phẩm
        public ActionResult Chitietsanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            Thu thu = data.Thus.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSanPham = thu.MaSanPham;
            if (thu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thu);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            Thu thu = data.Thus.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSanPham = thu.MaSanPham;
            if (thu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thu);
        }

        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            Thu thu = data.Thus.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSanPham = thu.MaSanPham;
            if (thu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.Thus.DeleteOnSubmit(thu);
            data.SubmitChanges();
            return RedirectToAction("Thu");
        }
        //----------------------------------------------------------------------
        public ActionResult KhachHang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //  return View(db.KhachHangs.ToList());
            return View(data.KhachHangs.ToList().OrderBy(n => n.MaKhachHang).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Xoakhachhang(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KhachHang khachhang = data.KhachHangs.SingleOrDefault(n => n.MaKhachHang == id);
            ViewBag.MaKhachHang = khachhang.MaKhachHang;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpPost, ActionName("Xoakhachhang")]
        public ActionResult Xacnhanxoakh(int  id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KhachHang khachhang = data.KhachHangs.SingleOrDefault(n => n.MaKhachHang == id);
            ViewBag.MaKhachHang = khachhang.MaKhachHang;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KhachHangs.DeleteOnSubmit(khachhang);
            data.SubmitChanges();
            return RedirectToAction("KhachHang");
        }
        //---------------------------------------------------------
        public ActionResult DonDatHang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //  return View(db.DonDatHangs.ToList());
            return View(data.DonDatHangs.ToList().OrderBy(n => n.SoHd).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Xoadondathang(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            DonDatHang dondathang = data.DonDatHangs.SingleOrDefault(n => n.SoHd == id);
            ViewBag.SoHd = dondathang.SoHd;
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dondathang);
        }

        [HttpPost, ActionName("Xoakhachhang")]
        public ActionResult Xacnhanxoadh(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            DonDatHang dondathang = data.DonDatHangs.SingleOrDefault(n => n.SoHd == id);
            ViewBag.SoHd = dondathang.SoHd;
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DonDatHangs.DeleteOnSubmit(dondathang);
            data.SubmitChanges();
            return RedirectToAction("DonDatHang");
        }
        //Chinh sửa sản phẩm
        [HttpGet]
        public ActionResult Suasanpham(int maSP)
        {
            //if (Session["UserAdmin"] == null)
            //    return RedirectToAction("Login", "Admin");
            Thu sanPham = data.Thus.SingleOrDefault(n => n.MaSanPham == maSP);
            if (sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaNcc = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNcc), "MaNcc", "TenNcc", sanPham.MaNcc);
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanPham.MaLoai);

            return View(sanPham);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(Thu sanPham, HttpPostedFileBase fileUpload, string anhBia)
        {
            //if (Session["UserAdmin"] == null)
            //    return RedirectToAction("Login", "Admin");
            ViewBag.MaNSX = new SelectList(data.NhaCungCaps.ToList().OrderBy(n => n.TenNcc), "MaNcc", "TenNcc");
            ViewBag.MaLoai = new SelectList(data.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (1 > 3)
            {
                return null;

            }
            else
            {
                if (ModelState.IsValid)
                {

                    var sp = data.Thus.FirstOrDefault(s => s.MaSanPham == sanPham.MaSanPham);
                    if (fileUpload != null)
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        sp.AnhBia = fileName;
                    }
                    sp.TenSanPham = sanPham.TenSanPham;
                    sp.GiaBan = sanPham.GiaBan;
                    sp.MoTa = sanPham.MoTa;
                    sp.NgayCapNhat = sanPham.NgayCapNhat;
                    sp.SoLuongTon = sanPham.SoLuongTon;
                    
                    sp.MaNcc = sanPham.MaNcc;
                    sp.MaLoai = sanPham.MaLoai;

                    data.SubmitChanges();
                }
                return RedirectToAction("Thu");
            }
        }

    }
}