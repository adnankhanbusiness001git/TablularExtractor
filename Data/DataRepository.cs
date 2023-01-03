using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablularExtractor.Model;

namespace TablularExtractor.Data
{
    class DataRepository
    {
        internal static void ImportEmployee(List<Employee> entities)
        {
            using (var ctx = new TabularContext())
            {
                ctx.Employee.AddRange(entities);
                ctx.SaveChanges();
            }
        }
    }
}
