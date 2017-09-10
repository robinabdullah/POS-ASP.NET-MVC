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
        IProductService service = ServiceFactory.GetProductService();
        // GET: Purchase
        public ActionResult Index()
        {
            List<string> array = service.GetAll().Select(x => x.Type).ToList();
            ViewData["supplierList"] = new[] { "Select"};
            return View();
        }
    }
}