using POSData;
using POSEntity;
using POSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POSService
{
    class Product_SupplierService : IProduct_SupplierService
    {
        private IProduct_SupplierDataAccess data;
        public Product_SupplierService(IProduct_SupplierDataAccess data)
        {
            this.data = data;
        }
        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product_Supplier Get(int id)
        {
            return data.Get(id);
        }

        public IEnumerable<Product_Supplier> GetAll()
        {
            return data.GetAll();
        }

        public int Insert(Product_Supplier obj)
        {
            return data.Insert(obj);
        }

        public int Update(Product_Supplier obj)
        {
            return data.Update(obj);
        }
    }
}
