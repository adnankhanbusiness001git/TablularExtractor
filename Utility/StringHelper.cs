using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TablularExtractor.Model;

namespace TablularExtractor.Utility
{
    public static class StringHelper
    {
        internal static string RemoveInitialExtraDetailsTill(string cipherText)
        {
            var regex = new Regex(string.Format(@"\b{0}\b", "Labour Card Details"),
                          RegexOptions.IgnoreCase);
            if (regex.Matches(cipherText).Count > 1)
            {
                var result = new List<string>();
                for (int i = 0; i < regex.Matches(cipherText).Count; i++)
                {
                    result.Add(cipherText.Substring(regex.Matches(cipherText)[i].Index));
                }
                //return result.Replace("Labour Card Details", "").Replace("*", "").Replace("?", "");
            }

            string text = cipherText.Substring(cipherText.LastIndexOf("Labour Card Details"));
            return text.Replace("Labour Card Details", "").Replace("*", "").Replace("?", "");
        }
        private static string GetIntegers(string text)
        {
            string result = string.Empty;
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsNumber(text[i]))
                    result += text[i];
            }

            return result;
        }
        internal static List<string> GetScanedList(string scanningText)
        {
            var data = new List<string>();

            int numberOfLines = 30;
            int from = 0;
            int to = 150;
            for (int line = 0; line <= numberOfLines; line++)
            {
                string lineData = string.Empty;
                var scaner = GetLinearText(from, to, scanningText);
                if (GetIntegers(scaner.Item3).Length > 42)
                    scaner = GetLinearText(from, to - 40, scanningText);
                scanningText = scanningText.Substring(scaner.Item2);
                if (scanningText.Length < 180)
                {
                    data.Add(scanningText);
                    break;
                }
                data.Add(scaner.Item3);
            }
            return data;
        }


        private static Tuple<int, int, string> GetLinearText(int from, int to, string scanningText)
        {
            string result = string.Empty;
            string passport = string.Empty;
            int start = 0;
            int end = 0;
            for (int i = to; i-- > (to - 200);)
            {
                try
                {
                    start = from;
                    end = i;
                    result = scanningText.Substring(from, end);
                    passport = result.Substring(result.Length - 7);
                    int value = 0;
                    if (int.TryParse(passport, out value))
                        break;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return Tuple.Create(start, end, result);
        }

        internal static Employee GetManagedData(string lineText, int pageNo, int lineNo, string filePath)
        {
            var entity = new Employee();
            entity.LineNumber = lineNo;
            entity.PageNumber = pageNo;
            entity.FilePath = filePath;

            try
            {
                // initial values
                string[] country_list = { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua &amp; Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia &amp; Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre &amp; Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts &amp; Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad &amp; Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks &amp; Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe" };

                string initialText = lineText.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                int passport = 8;
                int mol = 14;
                int expiry = 10;
                int labourCard = 8;
                int afdsf = 0;
                if (lineNo == 19 && pageNo == 3)
                    afdsf = 0;

                var stackInt = new Stack<char>();
                var stackString = new Stack<char>();

                for (var i = initialText.Length - 1; i >= 0; i--)
                {
                    if (!char.IsNumber(initialText[i]))
                    {
                        if (char.IsWhiteSpace(initialText[i]))
                            stackInt.Push(initialText[i]);
                        if (char.IsLetter(initialText[i - 5]))
                        {
                            stackInt.Push(initialText[i - 1]);
                            break;
                        }
                        if (char.IsLetter(initialText[i]))
                            stackString.Push(initialText[i]);
                        if (char.IsLetter(initialText[i - 1]))
                            stackString.Push(initialText[i - 1]);
                        break;
                    }

                    stackInt.Push(initialText[i]);
                }

                entity.PassportNumber = $"{new string(stackString.ToArray())} {new string(stackInt.ToArray())}";
                passport = entity.PassportNumber.Replace(" ", "").Length;
                initialText = initialText.Replace(entity.PassportNumber.Replace(" ", ""), "");
                char chkLastChar = initialText.Last();
                if (!IsLastINT(chkLastChar))
                {
                    bool isCountryExists = IsCountryExists(country_list, initialText);
                    if (!isCountryExists)
                    {
                        entity.PassportNumber = chkLastChar + entity.PassportNumber;
                        initialText = initialText + entity.PassportNumber.First();
                        if (!IsCountryExists(country_list, initialText))
                        {
                            initialText = initialText + entity.PassportNumber.Substring(2);
                        }
                    }
                    entity.PassportNumber = entity.PassportNumber.Insert(0, chkLastChar.ToString());
                    initialText = initialText.Remove(initialText.Length - 1, 1);
                }

                entity.PersonalNumber = initialText.Substring(initialText.Length - mol);
                initialText = initialText.Replace(entity.PersonalNumber, "");
                entity.Expiry = initialText.Substring(initialText.Length - expiry);
                initialText = initialText.Replace(entity.Expiry, "");
                entity.LabourCard = initialText.Substring(initialText.Length - labourCard);
                initialText = initialText.Replace(entity.LabourCard, "");
                country_list.ToList().ForEach(c =>
                {
                    if (initialText.ToUpper().EndsWith(c.ToUpper()))
                        entity.Country = c;
                });
                // secoundary values
                string secoundaryText = lineText.ToUpper().Replace(entity.PassportNumber.ToUpper(), "").Replace(entity.PersonalNumber.ToUpper(), "").Replace(entity.Expiry.ToUpper(), "").Replace(entity.LabourCard.ToUpper(), "").Replace(entity.Country.ToUpper(), "");
                secoundaryText = secoundaryText.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).First();
                var secoundaryArray = secoundaryText.Split(' ');
                entity.Job = $"{secoundaryArray[secoundaryArray.Length - 2]} {secoundaryArray[secoundaryArray.Length - 1]}";
                entity.EmployeeName = secoundaryText.Replace($"{secoundaryArray[secoundaryArray.Length - 2]} {secoundaryArray[secoundaryArray.Length - 1]}", "");


            }
            catch (Exception ex)
            {

            }

            return entity;

            bool IsCountryExists(string[] country_list, string initialText)
            {
                bool isCountryExists = false;
                country_list.ToList().ForEach(c =>
                {
                    isCountryExists = initialText.Contains(c);
                });
                return isCountryExists;
            }
        }
        private static bool IsLastINT(char lastChar)
        {
            int value = 0;
            return int.TryParse(lastChar.ToString(), out value);
        }
        private static bool IsFirstINT(char firstChar)
        {
            int value = 0;
            return int.TryParse(firstChar.ToString(), out value);
        }
    }
}
