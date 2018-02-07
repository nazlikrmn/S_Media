using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webApp.Controllers;
using System.Web.Http;


namespace SocialMediaMvc.Controllers
{
    public class HomeController : Controller
    {
        ValuesController valuesController = new ValuesController();
        // GET: Home
        public ActionResult Index()
        {//kullanıcı girişi
            return View();
        }
        [System.Web.Http.HttpPost]
        public ActionResult Giris(string girisBilgisi,string pass)
        {
            if (girisBilgisi.IndexOf('@') > -1)
                if(valuesController.KullaniciGirisiWithMail(girisBilgisi, pass))
                {
                    Session["Kullanici"] = valuesController.getLoginId(girisBilgisi);
                    return RedirectToAction("Home", "HomePage");

                }
                else
                    if (valuesController.KullaniciGirisi(girisBilgisi, pass))
                {
                    Session["Kullanici"] = valuesController.getLoginId(girisBilgisi);
                    return RedirectToAction("Home", "HomePage");//giriş yapıldı--anasayfaya yönlendir
                }
            return RedirectToAction("Home", "Giris");
        }
        public ActionResult Cikis()
        {
            Session["Kullanici"] = null;
            return RedirectToAction("Home", "Index");//çıkış durumu--giriş sayfasına yönlendir
        }
        public ActionResult Homepage()
        {
            int deger =Convert.ToInt32(Session["Kullanici"]);
            return View(valuesController.getDashBoard(deger));
        }
        public ActionResult getTakipciler()
        {
            int deger = Convert.ToInt32(Session["Kullanici"]);
            return View(valuesController.GetTakipciList(deger));//takipci getir
        }
        public ActionResult getTakipEdilenler()
        {
            int deger = Convert.ToInt32(Session["Kullanici"]);
            return View(valuesController.GetTakipEdilenlerList(deger));//takip edienler getir
        }
    }
}
