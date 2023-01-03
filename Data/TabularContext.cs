using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablularExtractor.Model;

namespace TablularExtractor.Data
{
    public class TabularContext : System.Data.Entity.DbContext
    {
        public TabularContext() : base("name=TabularDBConnectionString")
        {

        }

        public System.Data.Entity.DbSet<Employee> Employee { get; set; }

    }
}
