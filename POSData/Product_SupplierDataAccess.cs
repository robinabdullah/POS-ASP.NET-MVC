using POSData.Interfaces;
using POSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSData
{
    class Product_SupplierDataAccess : IProduct_SupplierDataAccess
    {
        private POSDataContext context;
        public Product_SupplierDataAccess(POSDataContext context)
        {
            this.context = context;
        }
        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product_Supplier Get(int id)
        {
            return context.Product_Suppliers.SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Product_Supplier> GetAll()
        {
            return context.Product_Suppliers.ToList();
        }

        public int Insert(Product_Supplier obj)
        {
            this.context.Product_Suppliers.Add(obj);
            return this.context.SaveChanges();
        }

        public int Update(Product_Supplier obj)
        {
            this.context.Product_Suppliers.Attach(obj);
            context.Entry(obj).State = System.Data.EntityState.Modified;
            return this.context.SaveChanges();
        }
    }
}
