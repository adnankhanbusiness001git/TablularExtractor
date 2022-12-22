using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabularExtractor.Model;

namespace TablularExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigPdf config = new ConfigPdf();
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Development"]))
                config = Helper.GeneratePDFConfiguration();

            ResponseManager response = DataOperations.ManageText(config);
        }
    }
}
