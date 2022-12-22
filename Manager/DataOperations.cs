using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Data;
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

                OrganizeExtraction();
            }

            //response.Text = PdfTextRequest();

            return response;
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
            foreach (var col in _config.Columns)
            {
                line += 1;
                if (col.ColumnName == "Passport No")
                    //finalTest.PassportNos.Item1= Enums.ColumnName.PassportNo.ToString();
                    passportNo= SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == "Person Details")
                    personDetails = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                    //personDetails = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == "JOB")
                    job = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == "Nationality")
                    nationality = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                    //nationality = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == "Labour Card Details")
                    cardDetails = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == "MOL")
                    mol = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                    //mol = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                if (col.ColumnName == "Expiry")
                    expiry = SubmitValue(validator.AnalyzeResult(col, GetText(col)));
                    //expiry = SubmitValue(validator.AnalyzeResult(col, GetText(col)));

                //var dr =  _table.NewRow();
                //
                //dr["Passport No"] = passportNo;
                //dr["Person Details"] = personDetails;
                //dr["JOB"] = job;
                //dr["Nationality"] = nationality;
                //dr["Labour Card Details"] = cardDetails;
                //dr["MOL"] = mol;
                //dr["Expiry"] = expiry;
                //dr["Line No"] = lineNo;
                //dr["Page No"] = col.PageNo.ToString();
                //
                //_table.Rows.Add(dr);
            }
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
    }
}
