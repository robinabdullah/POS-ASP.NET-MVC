using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POSDataAccess;


namespace POS_ASP.NET_MVC.Controllers
{
    public class BarcodesController : Controller
    {
        private POSDBContext db = new POSDBContext();

        // GET: Barcodes
        public ActionResult Index()
        {
            //var barcodes = db.Barcodes.Include(b => b.Product).OrderBy(x => x.Product.Type).Take(10);
            var barcodes = db.Barcodes.Include(b => b.Product).OrderByDescending(x => x.Date).Skip(1 * 10).Take(10);
            return View(barcodes.ToList());
        }
        [HttpPost]
        //public ActionResult Index(string num)
        //{
        //    int a = 0;
        //    if (num != "")
        //        a = int.Parse(Request["num"]);

        //    var barcodes = db.Barcodes.Include(b => b.Product).OrderBy(x => x.Product.Type).Skip(a * 10).Take(10);

        //    return View(barcodes.ToList());
        //}
        // GET: Barcodes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barcode barcode = db.Barcodes.Find(id);
            if (barcode == null)
            {
                return HttpNotFound();
            }
            return View(barcode);
        }

        // GET: Barcodes/Create
        public ActionResult Create()
        {
            ViewBag.Product_ID = new SelectList(db.Products, "ID", "Type");
            ViewBag.Barcode_Serial = new SelectList(db.Gifts, "Barcode", "SL");
            return View();
        }

        // POST: Barcodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Barcode_Serial,Product_ID,Color,Date")] Barcode barcode)
        {
            if (ModelState.IsValid)
            {
                db.Barcodes.Add(barcode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Product_ID = new SelectList(db.Products, "ID", "Type", barcode.Product_ID);
            ViewBag.Barcode_Serial = new SelectList(db.Gifts, "Barcode", "SL", barcode.Barcode_Serial);
            return View(barcode);
        }

        // GET: Barcodes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barcode barcode = db.Barcodes.Find(id);
            if (barcode == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_ID = new SelectList(db.Products, "ID", "Type", barcode.Product_ID);
            ViewBag.Barcode_Serial = new SelectList(db.Gifts, "Barcode", "SL", barcode.Barcode_Serial);
            return View(barcode);
        }

        // POST: Barcodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Barcode_Serial,Product_ID,Color,Date")] Barcode barcode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(barcode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product_ID = new SelectList(db.Products, "ID", "Type", barcode.Product_ID);
            ViewBag.Barcode_Serial = new SelectList(db.Gifts, "Barcode", "SL", barcode.Barcode_Serial);
            return View(barcode);
        }

        // GET: Barcodes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barcode barcode = db.Barcodes.Find(id);
            if (barcode == null)
            {
                return HttpNotFound();
            }
            return View(barcode);
        }

        // POST: Barcodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Barcode barcode = db.Barcodes.Find(id);
            db.Barcodes.Remove(barcode);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
