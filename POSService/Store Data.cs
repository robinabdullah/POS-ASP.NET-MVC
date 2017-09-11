using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Security.Principal;
using POSDataAccess;
using System.Data;

namespace POSService
{
    
    class DB
    {
        public static POSDBContext db = new POSDBContext();
        public static void resetConnString()
        {
            db.Dispose();
            db = new POSDBContext();
        }
        public static void DBSubmitChanges()
        {
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }

        public static bool TestDBConnection()
        {
            bool result = true;
            db = new POSDBContext();

            try
            {
                /// Hangs if connectionString is invalid rather than throw an exception
                db.Connection.Open();

                /// Initially, I was just trying to call DatabaseExists but, this hangs as well if the conn string is invalid
                if (!db.DatabaseExists())
                {
                    result = false;
                    throw new Exception("Database doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception("Error:" + ex.Message + "\n\nDetailed Error: " + ex.StackTrace);
            }
            finally
            {
                DB.resetConnString();
            }

            return result;
        }
    }
    class JurnalTable_Data: DB
    {

        public static bool PostPurchaseJournal(Journal j, List<Product_Supplier> listProduct_Supplier, List<Journal_Details> listJournal_Details, int amount, double paymentDue)
        {
            int serial = 1;
            try
            {

                if (listJournal_Details.Count > 0)
                {
                    foreach (var item in listJournal_Details)
                    {
                        item.SNO = serial++;
                        item.Journal = j;
                    }
                }
                else
                {
                    Journal_Details jd = new Journal_Details();
                    jd.SNO = 1;
                    jd.Sub_Account_ID = 1; ///cash account
                    jd.Amount = amount;
                    jd.Narration = "";
                    jd.Journal = j;
                }

                foreach (var ps in listProduct_Supplier)
                {
                    ps.Journal = j;
                }

                //var ps = db.Product_Suppliers.OrderByDescending(ep => ep.ID).FirstOrDefault();
                //ps.Journal = j; ///adding journal id in product supplier 

                db.Journals.Add(j);
                //db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Journal PostSalesJournal(Sale sale, Journal j, List<Journal_Details> listJournal_Details, int amount, double paymentDue)///DateTime datetime, int sub_accountID1, int sub_accountID2, int jurnalType, int status, int prepBy, int authBy, int amount
        {
            try
            {
                int serial = 1;

                if (listJournal_Details.Count > 0)
                {
                    foreach (var item in listJournal_Details)
                    {
                        item.SNO = serial++;
                        item.Journal = j;
                    }
                }
                else
                {
                    Journal_Details jd = new Journal_Details();
                    jd.SNO = 1;
                    jd.Sub_Account_ID = 1; ///cash account
                    jd.Amount = amount;
                    jd.Narration = "";
                    jd.Journal = j;
                }

                sale.Journal = j; /// assigning journal id in sales invoice

                //var sale = db.Sales.OrderByDescending(ep => ep.Invoice_ID).FirstOrDefault();
                //sale.Journal = j;

                db.Journals.Add(j);
                //db.SubmitChanges();

                return j;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    class ProductTableData : DB
    {
        public static IQueryable<Product> getAllProducts()
        {
            try
            {
                var te = from e in db.Products
                         orderby e.Unique_Barcode.StartsWith("Y")
                         select e;

                return te;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int getProductQuantity(string barcode)
        {
            try
            {
                var query =
                       (from pro in db.Products
                       join bar in db.Barcodes on pro.ID equals bar.Product_ID
                       where bar.Barcode_Serial.Equals(barcode)
                        select pro.Quantity_Available).First();

                return (int)query;
            }
            catch (Exception ex)
            {
                return 0;
                //throw new Exception(ex.Message);
            }
        }
        public static bool updateModel(int productID, string model)
        {
            try
            {
                var te = (from e in db.Products
                          where e.ID == productID
                          select e).First();

                te.Model = model;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Product Model Update unsuccessful.\n\nDetailed Error: " + ex.Message);
            }
        }
        //return total quantity of a product type
        public static int getTotalQ(string typ)
        {
            try
            {
                var allQuantity = from ee in db.Products
                        where ee.Type == typ
                        select ee.Quantity_Available;
                int sumofQuantity = 0;
                foreach (var quantity in allQuantity)
                {
                    sumofQuantity += (int)quantity;
                }

                return sumofQuantity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error calculating the sum of all product types quantity.\n\nDetailed Error:" + ex.Message);
            }
        }
        public static bool addNewProduct(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                //throw new Exception("Error adding new Product.\n\nDetailed Error:" + ex.Message);
                //Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
        public static bool updateProductQuantity(int newQuantity, int productID)
        {
            try
            {
                Product pro = (from u in db.Products
                               where u.ID == productID
                               select u).FirstOrDefault();

                pro.Quantity_Available = newQuantity;
                
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public static bool updateProduct(int productID, int quantity, int unitP,int sellingP, Barcode[] barcode, string uniqueBarcode, DateTime date)
        {
            try
            {                
                Product pro = (from u in db.Products
                               where u.ID == productID
                               select u).FirstOrDefault();
                
                pro.Quantity_Available += quantity;
                pro.Unit_Price = unitP;
                pro.Selling_Price = sellingP;
                pro.Date_Updated = date;
                if(barcode.Count() > 0)
                {
                    if(uniqueBarcode.StartsWith("NY") == false)                    
                        pro.Barcodes.AddRange(barcode);
                    else if (uniqueBarcode.StartsWith("NY") == true)
                    {
                        if (pro.Barcodes.Count == 0) pro.Barcodes.Add(barcode.First());
                    }
                }
                //db.SubmitChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
                //return false;
            }
        }
        public static IQueryable<string> getAllProductTypes()
        {
            try
            {
                //var txts = File.ReadAllLines(ProductTypePath).ToList();
                var txts = DB.db.Products
                           .Select(c => new { c.Type, c.Unique_Barcode })
                           .Distinct()
                           .OrderByDescending(x => x.Unique_Barcode)
                           .Select(x => x.Type);


                return txts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Product getProductByModel(string model)
        {
            //POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);
            try
            {
                
                var aa = (from u in db.Products
                          where u.Model == model
                          select u).First();
                
                return aa;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        //public static Product getProductByModel(POSDataContext dbtemp, string model)
        //{
        //    db = dbtemp;
        //    var aa = (from u in db.Products
        //              where u.Model == model
        //              select u).FirstOrDefault();

        //    return aa;
        //}
        public static Product getProductByID(int id)
        {
            try
            {
                var aa = (from u in db.Products
                          where u.ID == id
                          select u).FirstOrDefault();
                
                return aa;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        //public static Product getProductByID(POSDataContext dbtemp, int id)
        //{
        //    db = dbtemp;
        //    var aa = (from u in db.Products
        //              where u.ID == id
        //              select u).FirstOrDefault();

        //    return aa;
        //}
        public static List<string> getAllTypeMachedModels(string type)
        {
            //POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);

            try
            {
                List<string> list = new List<string>();

                var models = from ee in db.Products where ee.Type == type select ee.Model;
                foreach (string obj in models)
                {
                    list.Add(obj);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("No models found in DB.\n\nDetailed Error: " + ex.Message);
            }
        }
        public static List<string> getTypeMachedModelsQNonZero(string type)
        {
            //POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);

            try
            {
                List<string> list = new List<string>();

                var models = from ee in db.Products where ee.Type == type && ee.Quantity_Available != 0 orderby ee.Model select ee.Model ;
                foreach (string obj in models)
                {
                    list.Add(obj);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<string> getAllProductModels()
        {
            try
            {
                var temp = (from ee in db.Products select ee.Model).ToList();

                return temp;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static bool IsProductAlreadyExists(string typ, string mod)
        {            
            List<string> type = ProductTableData.getAllProductTypes().ToList();
            List<string> model = ProductTableData.getAllProductModels();

            if (typ != "")
            {
                foreach (string obj in type)
                    if (obj == typ)
                        return true;
            }
            foreach (string obj in model)
                if (obj == mod)
                    return true;
                        
            return false;
        }
        public static bool deleteProduct(int productID)
        {
            try
            {
                Product pro = (from ee in db.Products where ee.ID == productID select ee).First();

                if (pro.Quantity_Sold == 0)
                {
                    
                    db.Barcodes.DeleteAllOnSubmit(pro.Barcodes);
                    db.Products.DeleteOnSubmit(pro);
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    throw new Exception("some of the products are sold already. Please delete the sold products.");
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting product. \n\nDetailed Error:" + ex.Message);
            }
        }
        public static bool updatePrice(int productID, int unitPrice, int sellingPrice)
        {
            try
            {
                Product pro = (from ee in db.Products where ee.ID == productID select ee).First();

                pro.Unit_Price = unitPrice;
                pro.Selling_Price = sellingPrice;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting barcode. \n\nDetailed Error:" + ex.Message);
            }
        }
        public static bool delete1Barcode(string bar, int productID)
        {
            try
            {
                Product pro = (from ee in db.Products where ee.ID == productID select ee).First();
                Barcode barcodes = (from ee in db.Barcodes where ee.Barcode_Serial == bar select ee).First();

                pro.Quantity_Available -= 1; // 1 barcode deleted then quantity is decresed by -1

                db.Barcodes.DeleteOnSubmit(barcodes);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting barcode. \n\nDetailed Error:" + ex.Message);
            }

        }
        public static Barcode[] getBarcodesByPID(int productID)
        {
            try
            {
                Barcode[] barcodes = (from ee in db.Barcodes
                                      where ee.Product_ID == productID
                                      select ee).ToArray();

                return barcodes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while listing barcodes by product id.\n\nDetailed Error: " + ex.Message);
            }
        }
        public static List<Barcode> getBarcodesByPIDnDate(int productID, DateTime date)
        {
            try
            {
                var barcodes = (from ee in db.Barcodes
                                      where ee.Product_ID == productID 
                                        && ee.Date.Value.Date == date
                                      select ee).ToList();

                return barcodes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while listing barcodes by product id and date.\n\nDetailed Error: " + ex.Message);
            }
        }
        public static Barcode getBarcode(string barco)
        {
            try
            {
                //POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);
                Barcode barcode = (from ee in db.Barcodes where ee.Barcode_Serial == barco select ee).First();

                return barcode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static bool hasBarcodeinDB(string barcode)
        { 
            try
            {
                Barcode b = (from ee in db.Barcodes where ee.Barcode_Serial == barcode select ee).First();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //public static int getProductID()
        //{
        //    POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);
        //    try
        //    {
        //        var id = from ee in db.Sequences select ee.Product;

        //        return id.First();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.StackTrace);
        //        throw new Exception(ex.StackTrace);
        //    }
        //}

        //public static bool increaseProductID(int productID)
        //{
        //    POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);
        //    Sequence seq = db.Sequences.FirstOrDefault(e => e.Product == productID-1);
        //    //int temp = db.Sequences.Select(a => a.Product).First();
        //    Console.WriteLine(seq.Product);
        //    seq.Product = productID ;

        //    try
        //    {
        //        //db.Sequences.InsertOnSubmit(seq);
        //        db.SubmitChanges();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.StackTrace);
        //        return false;
        //    }
        //}
        
    }
    class Product_SupplierTableData: DB
    {
        public static bool addNewProduct_Supplier(Product_Supplier pro_supplier)
        {
            try
            {
                db.Product_Supplier.Add(pro_supplier);
                //db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                ///throw new Exception(ex.Message + "Detailed Error: " + ex.StackTrace);
                CustomMessage.Message = ex.Message;
                CustomMessage.StackTrace = ex.StackTrace;

                return false;
            }
        }
    }
    class SupplierTableData: DB
    {
        public static bool addNewSupplier(Supplier supplier)
        {
            try
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                ///throw new Exception(ex.Message + "Detailed Error: " + ex.StackTrace);
                CustomMessage.Message = ex.Message;
                CustomMessage.StackTrace = ex.StackTrace;

                return false;
            }
        }
        public static int getIDBySupplier(string supplier)
        {
            try
            {
                string contactName = supplier.Split('-')[0].Trim();
                string companyName = supplier.Split('-')[1].Trim();

                var v = (from ee in db.Suppliers
                         where ee.Company_Name == companyName && ee.Contact_Name == contactName
                         select ee.ID).Single();

                return v;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "Detailed Error: " + ex.StackTrace);
            }
        }
        public static Supplier getSupplier(string supplier)
        {
            try
            {
                string companyName = supplier.Split('-')[0].Trim();
                string contactName = supplier.Split('-')[1].Trim();

                var v = (from ee in db.Suppliers
                         where ee.Company_Name == companyName && ee.Contact_Name == contactName
                         select ee);

                if (v.Count() > 0)
                    return v.First();
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "Detailed Error: " + ex.StackTrace);
            }
        }
        public static List<string> getAllSupplierName()
        {
            try
            {
                var v = (from ee in db.Suppliers
                         where ee.Company_Name != "none" && ee.Contact_Name != "none"
                         select new { ee.Company_Name , ee.Contact_Name});
                List<string> list = new List<string>();
                foreach (var item in v)
                {
                    list.Add(item.Contact_Name + " - " + item.Company_Name);
                }
                return list;
            }
            catch (Exception ex)
            {
                ///throw new Exception(ex.Message + "Detailed Error: " + ex.StackTrace);
                CustomMessage.Message = ex.Message;
                CustomMessage.StackTrace = ex.StackTrace;
                return null;
            }
        }
    }
    class Free_ProductTableData : DB
    {
        public static Free_Product getFreeProductbyIDs(int invoiceID, int productID)
        {
            try
            {
                var v = (from ee in db.Free_Product 
                        where ee.Invoice_ID == invoiceID && ee.Product_ID == productID
                        select ee).First();

                return v;
            }
            catch
            {
                return null;
            }
        }
        
        public static bool getFreeProductby(int invoiceID, int productID)
        {
            try
            {
                var v = (from ee in db.Free_Product
                         where ee.Invoice_ID == invoiceID && ee.Product_ID == productID
                         select ee).First();

                if (v != null)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
    class Bill : DB
    {
        public static int getInvoiceNumber()
        {
            try
            {
                var v = db.Sales.Select(x => x.Invoice_ID).Max();

                return v;
            }
            catch 
            {
                return 0;
            }
            
        }
        public static Sale createBill(List<Customer_Sale> listCustomer_sale, Customer customer, List<Free_Product> listFree_Product,DateTime date)
        {
            
            try
            {
                //db = dbtemp;
                //Console.WriteLine(listCustomer_sale.Count);
                Sale sale = new Sale { Date = date };
                sale.Customer = customer;

                foreach (Free_Product obj in listFree_Product)//saving the invoice id to free product
                {
                    obj.Sale = sale;
                    db.Free_Product.Add(obj);
                }

                foreach (Customer_Sale obj in listCustomer_sale)
                {
                    obj.Sale = sale;
                    db.Customer_Sale.Add(obj);
                }

                //db.SubmitChanges();
                return sale;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
        }
    }
    class CustomerTableData : DB
    {
        public static List<Customer> getCustomersbyMatchingID(string id)
        {
            //POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);
            try
            {
                var temp1 = (from xx in db.Customers
                             where xx.ID.ToString() == id
                             select xx).ToList();
                return temp1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
   
    class FileManagement
    { 
        public static string filePath= Directory.GetCurrentDirectory() + @"\Files";

        public static string ProductTypePath = filePath + @"\Product Type.dat";
        public static string ProductModelPath = filePath + @"\Product Model.dat";
        public static string ColorPath = filePath + @"\Color.dat";
        public static string ProductRegPath = filePath + @"\Product Reg.dat";
        public static string ReceiptSavingLocation_datFile = filePath + @"\Receipt File Path.dat";

        public static string ReceiptSavingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
        public static string receipt = filePath + @"\receipt form field.pdf";
        public static string generatedReceipt = ReceiptSavingPath + @"\Fillied Receipt.pdf";
        private static void GrantAccess(string fullPath)
        {
            var directoryInfo = new DirectoryInfo(fullPath);
            var directorySecurity = directoryInfo.GetAccessControl();
            var currentUserIdentity = WindowsIdentity.GetCurrent();
            var fileSystemRule = new FileSystemAccessRule(currentUserIdentity.Name, FileSystemRights.Read, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow); directorySecurity.AddAccessRule(fileSystemRule); directoryInfo.SetAccessControl(directorySecurity);
        }
        public static List<string> getProductReg()
        {
            try
            {
                var txts = File.ReadAllLines(ProductRegPath).ToList();
                return txts;
            }
            catch(Exception ex)
            {
                throw new Exception("Error while reading Registration file. \n\nDetailed Error:" + ex.Message + ex.StackTrace);
            }
        }
        public static bool saveProductReg(string org, string newMac, string date)
        {
            try
            {
                //GrantAccess(ProductRegPath);
                string temp = String.Format("{0}\n{1}\n{2}", org, newMac, date);
                //File.SetAttributes(ProductRegPath, FileAttributes.Normal);
                File.WriteAllText(ProductRegPath, temp);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static bool checkReceiptSavingLocation()
        {
            string txt = "";
            try
            {
                txt = File.ReadLines(ReceiptSavingLocation_datFile).First();
            }
            catch{ }

            if (txt == "")
            {
                ReceiptSavingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                throw new Exception("No custom location is set yet. Receipt pdf will be saved on your Desktop.");
            }
            else if (Directory.Exists(txt) == false)
            {
                ReceiptSavingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                throw new Exception("The custom location for saving invoice receipt is invalid. Please change the location. Otherwise pdf will be saved on your Desktop.");
            }
            ReceiptSavingPath = txt;// if the saving path is valid

            return true;
        }
        public static void saveNewSavingPathtoDatFile(string savingPath)
        {
            try
            {
                File.WriteAllText(ReceiptSavingLocation_datFile, savingPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
        
        public static List<string> getAllProductModel()
        {
            //setPath();
            
            try
            {
                var txts = File.ReadAllLines(ProductModelPath).ToList();

                return txts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<string> getAllColor()
        {
            try
            {
                var txts = File.ReadAllLines(ColorPath).ToList();

                return txts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        
        public static void setNewProductType(string type)
        {
            //setPath();
            File.AppendAllText(ProductTypePath,"\n" +  type);
        }
        public static void setNewProductModel(string model)
        {
            //setPath();
            File.AppendAllText(ProductModelPath,"\n" +  model);
        }
        public static void setNewColor(string color)
        {
            //setPath();
            File.AppendAllText(ColorPath,"\n" + color);
        }
        public static bool IsColorAlreadyExists(string color)
        {
            //setPath();
            List<string> col = FileManagement.getAllColor();

            foreach (string obj in col)
                if (obj == color)
                    return true;

            return false;
        }
        public static bool setDefaultInvoiceFilePath()
        {
            return true;
        }
        
    }

    class Login_TableData : DB
    {
        public static bool verifyLogin(string username, string password)
        {
            try
            {
                var abc = db.Logins.Where(x => x.Username == username && x.Password == password).First();
                POSService.Login.ID = abc.ID;
                POSService.Login.UserType = abc.User_Type;
                POSService.Login.Username = abc.Username;
                if (!abc.Manager_ID.Equals(null))
                    POSService.Login.ManagerID = (int)abc.Manager_ID;
                POSService.Login.LastLogin = DateTime.Now.ToString();
                if (abc != null)
                {
                    abc.Last_Login = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }

            }
            catch
            {
                return false;
            }
            return false;
        }
        public static bool changePassword(string username, string newPassword)
        {
            try
            {
                var abc = (from ee in db.Logins
                           where ee.Username == username
                           select ee).First();

                abc.Password = newPassword;
                db.SaveChanges();

                return true;

            }
            catch
            {
                return false;
            }
        }
        public static List<POSDataAccess.Login> getAllUsers()
        {
            try
            {
                List<POSDataAccess.Login> abc = (from ee in db.Logins select ee).ToList();

                return abc;

            }
            catch (Exception ex)
            {
                throw new Exception("No user found in DB.\n\nDetailed Error:" + ex.Message);
            }
            
        }
    }

    class Gift_TableData : DB
    {
        public static Gift getGiftRowbtBarcode(string barcode)
        {
            try
            {
                var abc = (from e in db.Gifts
                           where e.Barcode == barcode
                           select e).FirstOrDefault();

                return abc;

            }
            catch (Exception ex)
            {
                throw new Exception("No data found on gift table.\n\nDetailed Error: " + ex.Message);
            }
        }
        public static Gift getGiftRowbyCust_SaleID(int custID)
        {
            try
            {
                var abc = (from e in db.Gifts
                            where e.Customer_Sale_ID == custID
                            select e).FirstOrDefault();

                return abc;

            }
            catch (Exception ex)
            {
                //return null;
                throw new Exception("No data found on gift table.\n\nDetailed Error: " + ex.Message);
                //return false;
            }
        }
        // check whether the product is sold out or not
        public static bool hasBarcodeinGiftTable(string barcode)
        {
            try
            {
                var abc = (from e in db.Gifts
                          where e.Barcode == barcode
                          select e).First();
                
                if (abc == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                //throw new Exception(ex.Message);
                return false;
            }
        }
    }

    class Customer_SaleTableData :DB
    {
        
        public static List<Customer_Sale> getCustomer_SalesbyInvoiceID(int invoiceID)
        {
            try
            {
                var v = (from e in db.Customer_Sales
                        where e.Invoice_ID == invoiceID 
                        select e).ToList();

                return v;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);                
            }
        }
        public static bool deleteCustomer_Sale(int customer_Sale_ID, int quan)
        {
            try
            {
                var cus_sale = Customer_SaleTableData.getCustomer_SalebyID(customer_Sale_ID);

                var product = (from e in db.Products
                          where e.ID == cus_sale.Product_ID
                          select e).First();

                var count = (from ee in db.Customer_Sale
                             where ee.Invoice_ID == cus_sale.Invoice_ID
                             select ee).Count();

                var freeProducts = (from ee in db.Free_Product
                                  where ee.Product_ID == cus_sale.Product_ID 
                                  && ee.Invoice_ID == cus_sale.Invoice_ID
                                  select ee).ToList();

                if (freeProducts.Count != 0)
                    db.Free_Product.DeleteOnSubmit(freeProducts.First());

                product.Quantity_Available += quan;
                product.Quantity_Sold -= quan;

                if (cus_sale.Gifts.Count != 0)
                {
                    db.Gifts.DeleteAllOnSubmit(cus_sale.Gifts); // delete gift if product has same or unq barcode
                }

                if( count == 1 )
                {
                    var sale = (from ee in db.Sales
                                where ee.Invoice_ID == cus_sale.Invoice_ID
                                select ee).First();
                    db.Sales.DeleteOnSubmit(sale);
                }

                db.Customer_Sales.DeleteOnSubmit(cus_sale);

                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting Customer_Sale.\n\nDetailed Error:" + ex.Message);
            }
        }
        public static Customer_Sale getCustomer_SalebyID(int customer_Sale_ID)
        {
            try
            {
                var v = (from e in db.Customer_Sale
                         where e.ID == customer_Sale_ID
                         select e).First();

                return v;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Customer_Sale> getCustomer_SalebyMatchingProduct(Product product)
        {
            //POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);
            try
            {
                var temp1 = (from xx in db.Customer_Sales
                             where xx.Product == product orderby xx.ID descending
                             select xx).ToList();
                return temp1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
    class Sale_TableData : DB
    {
        public static List<Sale> getSalesbyCustomerID(int cus_id)
        {
            try
            {
                //POSDataContext db = new POSDataContext(ConnectionString.connectionStringLinq);

                var v = (from e in db.Sales
                         where e.Customer_ID == cus_id
                         select e).ToList();

                return v;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<Sale> getSalesbyDate(DateTime date)
        {
            try
            {
                var v = (from e in db.Sales
                         where e.Date.Value.Date == date
                         orderby e.Invoice_ID ascending
                         select e).ToList();
                return v;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Sale getSalesbyInvoiceID(int invoiceID)
        {
            try
            {
                var v = (from e in db.Sales
                         where e.Invoice_ID == invoiceID
                         select e).First();

                return v;
            }
            catch (Exception ex)
            {
                throw new Exception("Invoice id not found in DB.\n\nDetailed Error: " + ex.Message);
            }
        }
    }
}