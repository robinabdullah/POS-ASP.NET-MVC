using POSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSData.Interfaces
{
    public interface ISaleDataAccess
    {
        IEnumerable<Sale> GetAll();
        Sale Get(int id);
        int Insert(Sale sale);
        int Update(Sale sale);
        int Delete(int id);
    }
}
