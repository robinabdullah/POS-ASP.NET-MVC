using POSEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSData
{
    class POSDataContext : DbContext
    {
            public DbSet<Sale> Sales { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<Product_Supplier> Product_Suppliers { get; set; }
    }
}
