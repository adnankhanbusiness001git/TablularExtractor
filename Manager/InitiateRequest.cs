using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablularExtractor.Data;
using TablularExtractor.Model;
using TablularExtractor.Utility;

namespace TablularExtractor.Manager
{
    public static class InitiateRequest
    {
        public static void ImportPDF(string Filename)
        {
            List<Employee> data = new List<Employee>();
            try
            {
                PdfReader reader = new PdfReader((string)Filename);
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                    String cipherText = PdfTextExtractor.GetTextFromPage(reader, page, its);
                    cipherText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(cipherText)));

                    string scanningText=StringHelper.RemoveInitialExtraDetailsTill(cipherText);

                    List<string> scanedList = StringHelper.GetScanedList(scanningText);

                    for (int line = 0; line < scanedList.Count; line++)
                    {
                        try
                        {
                            if (line < scanedList.Count)
                            {
                                var dataEntity = StringHelper.GetManagedData(scanedList[line], page, line + 1, Filename);
                                data.Add(dataEntity);
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
                reader.Close();
                DataRepository.ImportEmployee(data);
            }
            catch (Exception ex)
            {
            }

        }
    }
}
