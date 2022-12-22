using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.util;
using TabularExtractor.TextVisualizer;
using TabularExtractor.Utility;

namespace TabularExtractor.Model
{
    internal static class DataOperations
    {
        public static DataTable _table;
        public static PdfReader _pdfReader;
        public static ConfigPdf _config;
        internal static ResponseManager ManageText(ConfigPdf config)
        {
            var response = new ResponseManager();

            FileSetting setting = Helper.Condinates(config);

            _table = Helper.GenerateDataTable(config);

            _pdfReader = new PdfReader(config.FilePath);

            for (int page = 1; page <= _pdfReader.NumberOfPages; page++)
            {
                var orientation = new MyLocationTextExtractionStrategy();
                config.Columns = ColumnCondinates(page, _pdfReader, orientation, config);

                _config = config;
                for (int i = 1; i < setting.PageSize.Height; i+=10)
                {
                    IncreaseCordinates(i);
                    OrganizeExtraction();
                }
            }

            //response.Text = PdfTextRequest();

            return response;
        }

        private static void IncreaseCordinates(int i)
        {
            foreach (var col in _config.Columns)
            {
                col.Condinates.Height += i;
                col.Condinates.Width += i;
                col.Condinates.XAxis += i;
                col.Condinates.YAxis += i;
            }
        }

        internal static List<ColumnSetting> ColumnCondinates(int pageNo, PdfReader pdfReader, MyLocationTextExtractionStrategy myLocation,ConfigPdf config)
        {
            foreach (var col in config.Columns)
            {
                col.Condinates = ManageColumnSetting(col.ColumnName,pageNo, pdfReader, myLocation);
                col.PageNo = pageNo;
            }
            return config.Columns;
        }
        private static ColumnCordinate ManageColumnSetting(string columnName,int pageNo ,PdfReader pdfReader ,MyLocationTextExtractionStrategy myLocation)
        {
            var cordinates = new ColumnCordinate();

            var ex = PdfTextExtractor.GetTextFromPage(pdfReader, pageNo, myLocation);

            foreach (var point in myLocation.myPoints)
            {
                if (point.Text.Contains(columnName))
                {
                    cordinates.Height = point.Rect.Height;
                    cordinates.Width = point.Rect.Width;
                    cordinates.XAxis = point.Rect.Left;
                    cordinates.YAxis = point.Rect.Top;
                }
            }
            return cordinates;
        }

        private static string GetText(ColumnSetting columnSetting)
        {
            if (columnSetting.ColumnName == Enums.ColumnName.Expiry)
                return "";
            if (columnSetting.ColumnName == Enums.ColumnName.MOL)
                return "";
            if (columnSetting.ColumnName == Enums.ColumnName.PageNo)
                return "";
            if (columnSetting.ColumnName == Enums.ColumnName.LineNo)
                return "";
            var column = columnSetting.Condinates;
            var rect = new RectangleJ(column.XAxis.Value, column.YAxis.Value, column.Width.Value, column.Height.Value);

            RenderFilter[] filter = { new RegionTextRenderFilter(rect) };

            ITextExtractionStrategy strategy = new FilteredTextRenderListener(
                    new LocationTextExtractionStrategy(), filter);

            return PdfTextExtractor.GetTextFromPage(_pdfReader, columnSetting.PageNo.Value, strategy);
        }

        private static void OrganizeExtraction()
        {
            string lineNo= "", pageNo="" , passportNo="", personDetails="", job="", nationality="", cardDetails="", mol="", expiry="";
            int line=0;
            var validator = new ValidationHelper(_pdfReader);
            var finalTest = new ColumnFinalSetting();
            line += 1;
            foreach (var col in _config.Columns)
            {
                if (col.ColumnName == Enums.ColumnName.PassportNo)
                    passportNo= SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == Enums.ColumnName.PersonDetails)
                    personDetails = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == Enums.ColumnName.JOB)
                    job = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == Enums.ColumnName.Nationality)
                    nationality = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == Enums.ColumnName.LabourCardDetails)
                    cardDetails = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == Enums.ColumnName.MOL)
                    mol = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == Enums.ColumnName.Expiry)
                    expiry = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == Enums.ColumnName.PageNo)
                    pageNo= col.PageNo.ToString();
            }
            var dr = _table.NewRow();

            dr["Passport No"] = passportNo;
            dr["Person Details"] = personDetails;
            dr["JOB"] = job;
            dr["Nationality"] = nationality;
            dr["Labour Card Details"] = cardDetails;
            dr["MOL"] = mol;
            dr["Expiry"] = expiry;
            dr["Line No"] = lineNo;
            dr["Page No"] = pageNo;

            _table.Rows.Add(dr);
        }

        private static string SubmitValue(Tuple<bool, ColumnSetting> data)
        {
            if (data.Item1)
            {
                return GetText(data.Item2);
            }
            else
            {
                return "NNN";
            }
        }

        public static DataTable ImportPDF(string Filename)
        {
            string strText = string.Empty;
            List<string[]> list = new List<string[]>();
            string[] PdfData = null;
            try
            {
                PdfReader reader = new PdfReader((string)Filename);
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
                    String cipherText = PdfTextExtractor.GetTextFromPage(reader, page, its);
                    cipherText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(cipherText)));
                    strText = strText + "\n" + cipherText;
                    PdfData = strText.Split('\n');

                }
                reader.Close();
            }
            catch (Exception ex)
            {
            }

            List<string> temp = PdfData.ToList();
            temp.RemoveRange(0,9);
            list = temp.ConvertAll<string[]>(x => x.Split(' ').ToArray());
            List<string> columns = list.FirstOrDefault().ToList();
            DataTable dtTemp = new DataTable();
            //columns.All(x => { dtTemp.Columns.Add(new DataColumn(x)); return true; });
            list.All(x => { dtTemp.Rows.Add(dtTemp.NewRow().ItemArray = x); return true; });
            return dtTemp;
        }

    }
}
