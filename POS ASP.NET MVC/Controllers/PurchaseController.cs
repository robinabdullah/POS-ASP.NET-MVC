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
            ViewData["modelList"] = new string[] { "Select" };
            ViewData["selectedType"] = "Select";
            ViewData["selectedSupplier"] = "Select";

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string[] supplier = SupplierTableData.getAllSupplierName().ToArray();
            string[] types = ProductTableData.getAllProductTypes().ToArray();
            ViewData["supplierList"] = supplier;
            ViewData["typeList"] = types;

            string selected = collection["type"];
            ViewData["selectedType"] = selected;
            ViewData["selectedSupplier"] = collection["supplier"].ToString();
            ViewData["selectedModel"] = "Select";

            ViewData["modelList"] = ProductTableData.getAllTypeMachedModels(selected).ToArray();
            return View();
        }

        
    }
}