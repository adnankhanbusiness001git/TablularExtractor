using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabularExtractor.Model
{
    class ColumnSetting
    {
        public string ColumnName { get; set; }
        public bool? IsString { get; set; }
        public bool? IsNumber { get; set; }
        public bool? IsMoney { get; set; }
        public bool? IsBlank { get; set; }
        public bool? IsFullName { get; set; }
        public int? Length { get; set; }
        public int? PageNo { get; set; }
        public int? LineNo { get; set; }
        public ColumnCordinate Condinates { get;  set; }

    }
}
