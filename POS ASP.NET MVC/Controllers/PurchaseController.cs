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
            string[] array = ProductTableData.getAllProductTypes().ToArray();
            ViewData["supplierList"] = array;
            return View();
        }
    }
}