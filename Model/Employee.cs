using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablularExtractor.Model
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public int LineNumber { get; internal set; }
        public int PageNumber { get; internal set; }
        public string FilePath { get; internal set; }
        public string PassportNumber { get; internal set; }
        public string PersonalNumber { get; internal set; }
        public string Expiry { get; internal set; }
        public string LabourCard { get; internal set; }
        public string Country { get; internal set; }
        public string EmployeeName { get; internal set; }
        public string Job { get; internal set; }
    }
}
