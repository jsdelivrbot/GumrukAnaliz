using Gumruk.Entity;
using Gumruk.UnitOfWork;
using Gumruk.UnitOfWork.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gumruk.Web.Controllers
{
    public class GumrukOperationsController : Controller
    {
        //
        // GET: /GumrukOperations/
        [AuthenticationAction]
        public ActionResult Index()
        {
            return View();
        }
        [AuthenticationAction]
        public ActionResult OzetBeyan()
        {
            return View(new OzetBeyan());
        }

        [HttpPost]
        public ActionResult GetBeyanTurleri()
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("BeyanTurleri", iGumruk.GetBeyanTurleri());
        }

        [HttpPost]
        public ActionResult GetGumrukIdareleri()
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("GumrukIdareleri", iGumruk.GetGumrukIdareleri());
        }

        [HttpPost]
        public ActionResult GetYuklemeUlkesi()
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("YuklemeUlkesi", iGumruk.GetUlkeler());
        }

        [HttpPost]
        public ActionResult GetBosaltmaUlkesi()
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("BosaltmaUlkesi", iGumruk.GetUlkeler());
        }

        [HttpPost]
        public ActionResult GetYuklemeLimanlar(int ulkeID)
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("YuklemeLimanlar", iGumruk.GetLimanlarByUlkeId(ulkeID));
        }

        [HttpPost]
        public ActionResult GetBosaltmaLimanlar(int ulkeID)
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("BosaltmaLimanlar", iGumruk.GetLimanlarByUlkeId(ulkeID));
        }

        [HttpPost]
        public JsonResult GetOzelTuzelSahis(string vergiNo)
        {
            IGumruk iGumruk = new BSGumruk();
            OzelTuzelSahis sahis = iGumruk.GetOzelTuzelSahisByVergiNo(vergiNo);

            return Json(sahis, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOzelTuzelKisiTanimla()
        {
            OzelTuzelSahis sahis = new OzelTuzelSahis();
            IGumruk iGumruk = new BSGumruk();

            sahis.KimlikTurleri = iGumruk.GetKimlikTurleri();

            ViewData["Ulkeler"] = iGumruk.GetUlkeler();

            return PartialView(sahis);
        }

        [HttpPost]
        public JsonResult SahisKaydet(OzelTuzelSahis sahis)
        {
            IGumruk iGumruk = new BSGumruk();
            sahis = iGumruk.SahisKaydet(sahis);
            sahis.AdiUnvani = sahis.AdiUnvani + " / " + sahis.KimlikNo.ToString();
            return Json(sahis, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OzetBeyanKaydet(OzetBeyan oztBeyan)
        {
            IGumruk iGumruk = new BSGumruk();

            oztBeyan = iGumruk.OzetBeyanKaydet(oztBeyan);

            return Json(oztBeyan, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTasitinUlkesi()
        {
            IGumruk iGumruk = new BSGumruk();

            return PartialView("TasitinUlkesi", iGumruk.GetUlkeler());
        }

        public JsonResult BeyanTurleri()
        {
            IGumruk iGumruk = new BSGumruk();
            List<BeyanTurleri> beyanturleri = iGumruk.GetBeyanTurleri();

            return Json(beyanturleri, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GumrukIdareleri()
        {
            IGumruk iGumruk = new BSGumruk();
            List<GumrukIdareleri> idareler = iGumruk.GetGumrukIdareleri();

            return Json(idareler, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUlkeler()
        {
            IGumruk iGumruk = new BSGumruk();
            List<Ulkeler> ulkeler = iGumruk.GetUlkeler();

            return Json(ulkeler, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OzetBynKayit()
        {
            IGumruk iGumruk = new BSGumruk();

            OzetBeyan oztBeyan = new OzetBeyan();

            return View("OzetBeyan", oztBeyan);
        }

        [HttpGet]
        public ActionResult GetLimanlar(int ulkeID)
        {
            IGumruk iGumruk = new BSGumruk();
            List<Limanlar> limanlar = iGumruk.GetLimanlarByUlkeId(ulkeID);

            return Json(limanlar, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetKimlikTurleri()
        {
            IGumruk iGumruk = new BSGumruk();
            List<KimlikTurleri> kimlikler = iGumruk.GetKimlikTurleri();

            return Json(kimlikler, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchBeyanSahibi(string searchValue)
        {
            IGumruk iGumruk = new BSGumruk();
            OzelTuzelSahis sahis = iGumruk.GetOzelTuzelSahisByVergiNo(searchValue);

            if (sahis != null)
                sahis.AdiUnvani += " / " + sahis.KimlikNo;
            

            return Json(sahis, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TasitSearch(string searchValue)
        {
            IGumruk iGumruk = new BSGumruk();
            TasitBilgileri tasit = iGumruk.SearchTasit(searchValue);

            if (tasit != null)
                tasit.Ad+= " / " + tasit.Numarasi;


            return Json(tasit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TasitKaydet(TasitBilgileri tasit)
        {
            IGumruk iGumruk = new BSGumruk();
            tasit = iGumruk.TasitKaydet(tasit);
            tasit.Ad = tasit.Ad + " / " + tasit.Numarasi.ToString();
            return Json(tasit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTasitBilgileri(int ID)
        {
            IGumruk iGumruk = new BSGumruk();
            TasitBilgileri tst = iGumruk.GetTasitBilgileriByID(ID);

            return Json(tst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTasiyiciFirma(int ID)
        {
            IGumruk iGumruk = new BSGumruk();
            OzelTuzelSahis shs = iGumruk.GetTasiyiciFirma(ID);

            return Json(shs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBeyanSahibi(int ID)
        {
            IGumruk iGumruk = new BSGumruk();
            OzelTuzelSahis shs = iGumruk.GetTasiyiciFirma(ID);

            return Json(shs, JsonRequestBehavior.AllowGet);
        }

      
    }
}