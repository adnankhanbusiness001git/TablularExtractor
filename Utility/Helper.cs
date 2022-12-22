using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabularExtractor.Model;
using TabularExtractor.TextVisualizer;

namespace TabularExtractor.Model
{
    internal static class Helper
    {
        internal static FileSetting Condinates(ConfigPdf config)
        {
            var setting = new FileSetting();
            PdfReader reader = new PdfReader(config.FilePath);
            setting.PageSize = reader.GetPageSize(1);

            return setting;
        }

        internal static DataTable GenerateDataTable(ConfigPdf config)
        {
            DataTable dt = new DataTable();
            if (config.Columns.Count > 0)
                config.Columns.ForEach(x => dt.Columns.Add(x.ColumnName));

            return dt;
        }

        internal static ConfigPdf GeneratePDFConfiguration()
        {
            ConfigPdf config = new ConfigPdf();
            config.FilePath = @"E:\Files\MUNSHA LIST 07202022\622352_MOL.pdf";

            var columnProps = new List<ColumnSetting>();
            string[] columnNames = { "Line No", "Page No" ,"Passport No", "Person Details", "JOB", "Nationality", "Labour Card Details", "MOL", "Expiry"};
            for (int i = 0; i < columnNames.Length; i++)
            {
                columnProps.Add(new ColumnSetting { ColumnName = columnNames[i] });
            }
            config.Columns.AddRange(columnProps);
            return config;
        }
    }
}
