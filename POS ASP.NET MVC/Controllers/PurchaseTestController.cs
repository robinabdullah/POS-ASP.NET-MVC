using POSDataAccess;
using POSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS_ASP.NET_MVC.Controllers
{
    public class PurchaseTestController : Controller
    {
        // GET: PurchaseTest
        public ActionResult Index()
        {
            ViewBag.test = ProductTableData.getAllProducts();
            string[] supplier = SupplierTableData.getAllSupplierName().ToArray();
            string[] types = ProductTableData.getAllProductTypes().ToArray();
            string[] colors = FileManagement.getAllColor().ToArray();
            ViewBag.supplierList = supplier;
            ViewData["supplierList"] = supplier;
            ViewData["typeList"] = types;
            ViewData["colorList"] = colors;

            ViewData["selectedSupplier"] = "Select";
            ViewData["selectedType"] = "Select";
            ViewData["selectedModel"] = "Select";
            ViewData["modelList"] = new string[] { "Select" };
            ViewData["selectedColor"] = "None";

            return View();
        }
        [HttpPost]
        public ActionResult Index(Product product)
        {
            try
            {
                
                ProductTableData.updateProduct(product.ID, (int)product.Quantity_Available, (int)product.Unit_Price, (int)product.Selling_Price, product.Barcodes.ToArray(), product.Unique_Barcode, DateTime.Now);

                Supplier supplier = SupplierTableData.getSupplier(product.Model);//supplier name is saved in product model

                Product_Supplier product_Supplier = new Product_Supplier() { Date = DateTime.Now, Quantity = product.Quantity_Available, Supplier = supplier, Ref_Number = product.Type, Product_ID = product.ID, Unit_Price = product.Unit_Price };

                Journal journal = new Journal();
                journal.Date = product.Date_Updated;
                journal.Sub_Account_ID = 10; ///purchase account
                journal.Type = 0; /// 0 means Debit
                journal.Status = 1; ///posted
                journal.PreparedBy = 1;
                journal.AuthenticatedBy = 1;
                //Product_SupplierTableData.addNewProduct_Supplier()
                return Json(new
                { 
                    msg = "Successfully added " + product.ID + " " +
                    product.Quantity_Available + " " +
                    product.Selling_Price + " " +
                    product.Barcodes.First().Barcode_Serial

                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult FillDDLModel(string selected)
        {
            
            //if (selected != null)
            var models = ProductTableData.getAllProducts().Where(x => x.Type == selected).Select(x => new { ID = x.ID, Model = x.Model}).ToList();
            //, UnitPrice = x.Unit_Price, SellingPrice = x.Selling_Price, UniqueBarcode = x.uni
            return new JsonResult { Data = models, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult FillPrices(string selected)
        {
            //if (selected != null)
            var prices = ProductTableData.getAllProducts().Where(x => x.Model == selected).Select(x => new { ID = x.ID, UnitPrice = x.Unit_Price, SellingPrice = x.Selling_Price, UniqueBarcode = x.Unique_Barcode, Description=x.Des}).FirstOrDefault();
            //, UnitPrice = x.Unit_Price, SellingPrice = x.Selling_Price, UniqueBarcode = x.Unique_Barcode
            return new JsonResult { Data = prices, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult FillProductTable(string selected)
        {
            //if (selected != null)
            var product = ProductTableData.getAllProducts().Where(x => x.Model == selected).Select(x => new { ID = x.ID, Type = x.Type, Model = x.Model, Quantity= x.Quantity_Available, UnitPrice = x.Unit_Price, SellingPrice = x.Selling_Price, UniqueBarcode = x.Unique_Barcode }).FirstOrDefault();
            //, UnitPrice = x.Unit_Price, SellingPrice = x.Selling_Price, UniqueBarcode = x.Unique_Barcode
            return new JsonResult { Data = product, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}