using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSService;


namespace POS_ASP.NET_MVC.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Purchase
        public ActionResult Index()
        {
            ViewBag.test = ProductTableData.getAllProducts();
            string[] supplier = SupplierTableData.getAllSupplierName().ToArray();
            string[] types = ProductTableData.getAllProductTypes().ToArray();
            ViewData["supplierList"] = supplier;
            ViewData["typeList"] = types;
            return View();
        }
        public ActionResult Fill_DDL_Model()
        {
            List<Models> models = ProductTableData.getAllProducts().Select(x => new Models { ID = x.ID, Model = x.Model }).ToList();
            return new JsonResult { Data = models, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}