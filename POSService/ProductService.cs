using POSData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSEntity;
using POSService.Interfaces;

namespace POSService
{
    class ProductService : IProductService
    {
        private IProductDataAccess data;
        public ProductService(IProductDataAccess data)
        {
            this.data = data;
        }
        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product Get(int id)
        {
            return data.Get(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return data.GetAll();
        }

        public int Insert(Product product)
        {
            return data.Insert(product);
        }

        public int Update(Product product)
        {
            return data.Update(product);

        }
    }
}
