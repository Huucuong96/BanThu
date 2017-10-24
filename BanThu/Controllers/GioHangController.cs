using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanThu.Models;

namespace BanThu.Controllers
{
    namespace WebApplication1.Controllers
    {
        public class GioHangController : Controller
        {
            dbBanThuDataContext data = new dbBanThuDataContext();
            public List<GioHang> LayGioHang()
            {
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang == null)
                {
                    lstGioHang = new List<GioHang>();
                    Session["GioHang"] = lstGioHang;

                }
                return lstGioHang;
            }
            //them hang gio
            public ActionResult ThemGioHang(int iMaSanPham, string strURL)
            {
                List<GioHang> lstGioHang = LayGioHang();
                GioHang SanPham = lstGioHang.Find(n => n.iMaSanPham == iMaSanPham);

                if (SanPham == null)
                {
                    SanPham = new GioHang(iMaSanPham);
                    lstGioHang.Add(SanPham);
                    return Redirect(strURL);

                }
                else
                {
                    SanPham.iSoLuong++;
                    return Redirect(strURL);
                }
            }
            private int TongSL()
            {
                int iTongSL = 0;
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang != null)
                {
                    iTongSL = lstGioHang.Sum(n => n.iSoLuong);


                }
                return iTongSL;
            }
            private Double TongTien()
            {
                Double iTongTien = 0;
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang != null)
                {
                    iTongTien = lstGioHang.Sum(n => n.dThanhTien);


                }
                return iTongTien;
            }
            // GET: Giohang
            public ActionResult GioHang()
            {
                List<GioHang> lstGioHang = LayGioHang();
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "BanThu");

                }
                ViewBag.TongSL = TongSL();
                ViewBag.TongTien = TongTien();



                return View(lstGioHang);
            }
            public ActionResult GioHangPartial()
            {
                ViewBag.TongSL = TongSL();
                ViewBag.TongTien = TongTien();
                return PartialView();

            }
            public ActionResult XoaGioHang(int iMaSP)
            {
                List<GioHang> lstGioHang = LayGioHang();
                GioHang SanPham = lstGioHang.SingleOrDefault(n => n.iMaSanPham == iMaSP);
                if (SanPham != null)
                {
                    lstGioHang.RemoveAll(n => n.iMaSanPham == iMaSP);
                    return RedirectToAction("GioHang");
                }
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "BanThu");
                }
                return RedirectToAction("GioHang");
            }
            public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
            {
                List<GioHang> lstGioHang = LayGioHang();
                GioHang SanPham = lstGioHang.SingleOrDefault(n => n.iMaSanPham == iMaSP);
                if (SanPham != null)
                {
                    SanPham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
                }

                return RedirectToAction("GioHang");
            }
            public ActionResult XoaHetGioHang()
            {
                List<GioHang> lstGioHang = LayGioHang();
                lstGioHang.Clear();

                return RedirectToAction("Index", "BanThu");


            }
            [HttpGet]
            public ActionResult DatHang()
            {
                if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
                {
                    return RedirectToAction("DangNhap", "NguoiDung");


                }
                if (Session["GioHang"] == null)
                {
                    return RedirectToAction("Index", "BanThu");

                }
                List<GioHang> lstGioHang = LayGioHang();
                ViewBag.TongSL = TongSL();
                ViewBag.TongTien = TongTien();



                return View(lstGioHang);
            }

            public ActionResult DatHang(FormCollection collection)
            {
                DonDatHang ddh = new DonDatHang();
                KhachHang kh = (KhachHang)Session["TaiKhoan"];
                List<GioHang> gh = LayGioHang();
                ddh.MaKhachHang = kh.MaKhachHang;
                ddh.NgayDh = DateTime.Now;



                var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);
                ddh.NgayGiao = DateTime.Parse(ngaygiao);

                ddh.TinhTrangGiaoHang = false;
                ddh.DaThanhToan = false;
                data.DonDatHangs.InsertOnSubmit(ddh);
                data.SubmitChanges();
                foreach (var item in gh)
                {
                    ChiTietDonHang ctdh = new ChiTietDonHang();
                    ctdh.SoHd = ddh.SoHd;
                    ctdh.MaSanPham = item.iMaSanPham;
                    ctdh.SoLuong = item.iSoLuong;
                    ctdh.DonGia = (decimal)item.dDonGia;
                    data.ChiTietDonHangs.InsertOnSubmit(ctdh);
                }
                data.SubmitChanges();
                Session["GioHang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            public ActionResult XacNhanDonHang()
            {
                return View();
            }

        }
    }
}