using POSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSService
{
    public abstract class ServiceFactory
    {
        public static IProductService GetProductService()
        {
            return new ProductService(DataAccessFactory.GetProductDataAccess());
        }
    }
}
