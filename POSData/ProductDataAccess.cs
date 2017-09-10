using POSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSEntity;

namespace POSData
{
    class ProductDataAccess : IProductDataAccess
    {
        private POSDataContext context;
        public ProductDataAccess(POSDataContext context)
        {
            this.context = context;
        }
        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product Get(int id)
        {
            return context.Products.SingleOrDefault(x => x.ID == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products.ToList();
        }

        public int Insert(Product product)
        {
            this.context.Products.Add(product);
            return this.context.SaveChanges();
        }

        public int Update(Product product)
        {
            this.context.Products.Attach(product);
            context.Entry(product).State = System.Data.EntityState.Modified;
            return this.context.SaveChanges();
        }
    }
}
