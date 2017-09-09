using POSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSData.Interfaces
{
    public interface IProduct_SupplierDataAccess
    {
        IEnumerable<Product_Supplier> GetAll();
        Product_Supplier Get(int id);
        int Insert(Product_Supplier obj);
        int Update(Product_Supplier obj);
        int Delete(int id);
    }
}
