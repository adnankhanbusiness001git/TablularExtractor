using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.util;
using TabularExtractor.Model;

namespace TabularExtractor.Utility
{
    internal  class ValidationHelper
    {
        PdfReader _pdfReader;
        public ValidationHelper(PdfReader pdfReader)
        {
            _pdfReader=pdfReader;
        }
        internal Tuple<bool, ColumnSetting> AnalyzeResult(ColumnSetting column,string text)
        {
            var result =new ColumnSetting();
            if (column.ColumnName == Enums.ColumnName.PassportNo)
                return ValidateField(column,text);
            
            if (column.ColumnName == Enums.ColumnName.PersonDetails)
                return ValidateField(column, text);

            if (column.ColumnName == Enums.ColumnName.Nationality)
                return ValidateField(column, text);

            if (column.ColumnName == Enums.ColumnName.MOL)
                return ValidateField(column, text);

            if (column.ColumnName == Enums.ColumnName.LabourCardDetails)
                return ValidateField(column, text);

            if (column.ColumnName == Enums.ColumnName.JOB)
                return ValidateField(column, text);

            if (column.ColumnName == Enums.ColumnName.Expiry)
                return ValidateField(column, text);

            return Tuple.Create(true, result);
        }

        private Tuple<bool, ColumnSetting> ValidateField(ColumnSetting column, string text)
        {
            bool isSuccess=TryCatch(column, text);
            if (!isSuccess)
            {
                //column.Condinates = column.Condinates.XAxis + 1;
                do
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        column.Condinates.XAxis = column.Condinates.XAxis + i;
                        column.Condinates.YAxis = column.Condinates.YAxis + i;
                        isSuccess =TryCatch(column, GetText(column));
                        if (isSuccess)
                            break;
                    }
                } while (isSuccess);
            }
            return Tuple.Create(isSuccess, column);
        }

        private  string GetText(ColumnSetting columnSetting)
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

        //private Tuple<bool, ColumnSetting> ValidateExpiry(ColumnSetting column, string text)
        //{
        //    bool isSuccess=TryCatch(column, text);
        //    if (!isSuccess)
        //    {
        //        //column.Condinates = column.Condinates.XAxis + 1;
        //        do
        //        {   

        //        } while (isSuccess);
        //    }
        //    return null;
        //}

        private bool TryCatch(ColumnSetting column, string text)
        {
            try
            {
                if (column.ColumnName == Enums.ColumnName.PassportNo)
                    return text.Length != 8 ? false : true;

                if (column.ColumnName == Enums.ColumnName.PersonDetails)
                    return Regex.IsMatch(text, @"^[\p{L} \.'\-]+$");

                if (column.ColumnName == Enums.ColumnName.Nationality)
                    return Regex.IsMatch(text, @"^[\p{L} \.'\-]+$");

                if (column.ColumnName == Enums.ColumnName.MOL)
                    return text.Length != 15 ? false : true;

                if (column.ColumnName == Enums.ColumnName.LabourCardDetails)
                    return text.Length != 8 ? false : true;

                if (column.ColumnName == Enums.ColumnName.JOB)
                    return Regex.IsMatch(text, @"^[\p{L} \.'\-]+$");

                if (column.ColumnName == Enums.ColumnName.Expiry)
                    Convert.ToDateTime(text);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        //private Tuple<bool, ColumnSetting> ValidateJOB(ColumnSetting column, string text)
        //{
        //    throw new NotImplementedException();
        //}

        //private Tuple<bool, ColumnSetting> ValidateLabourCardDetails(ColumnSetting column, string text)
        //{
        //    throw new NotImplementedException();
        //}

        //private Tuple<bool, ColumnSetting> ValidateMOL(ColumnSetting column, string text)
        //{
        //    throw new NotImplementedException();
        //}

        //private Tuple<bool, ColumnSetting> ValidateNationality(ColumnSetting column, string text)
        //{
        //    throw new NotImplementedException();
        //}

        //private Tuple<bool, ColumnSetting> ValidatePersonDetails(ColumnSetting column, string text)
        //{
        //    throw new NotImplementedException();
        //}

        //private Tuple<bool, ColumnSetting> ValidatePassport(ColumnSetting column, string text)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
