using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabularExtractor.Model
{
    class ConfigPdf
    {
        public string FilePath { get; set; }
        public List<ColumnSetting> Columns { get; set; } = new List<ColumnSetting>();
    }
}
