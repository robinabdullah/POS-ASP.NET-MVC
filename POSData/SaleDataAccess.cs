using POSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSEntity;

namespace POSData
{
    class SaleDataAccess : ISaleDataAccess
    {
        private POSDataContext context;
        public SaleDataAccess(POSDataContext context)
        {
            this.context = context;
        }
        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Sale Get(int id)
        {
            return context.Sales.SingleOrDefault(x => x.Invoice_ID == id);
        }

        public IEnumerable<Sale> GetAll()
        {
            return context.Sales.ToList();
        }

        public int Insert(Sale sale)
        {
            this.context.Sales.Add(sale);
            return this.context.SaveChanges();
        }

        public int Update(Sale sale)
        {
            this.context.Sales.Attach(sale);
            context.Entry(sale).State = System.Data.EntityState.Modified;
            return this.context.SaveChanges();
        }
    }
}
