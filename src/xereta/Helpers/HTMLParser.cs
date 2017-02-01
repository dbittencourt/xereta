using System.Collections.Generic;
using System.Globalization;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using xereta.Models;

namespace xereta.Helpers
{
    public class HTMLParser : IDataParser
    {
        AngleSharp.Parser.Html.HtmlParser _htmlParser;

        public HTMLParser()
        {
            _htmlParser = new AngleSharp.Parser.Html.HtmlParser();
        } 
        public IEnumerable<PublicWorker> ParseSearch(string html)
        {
            var table = _htmlParser.Parse(html).QuerySelector(@"table[summary]");        
            var result = ParseSearchTable(table);

            return result;   
        }

        public PublicWorker Parse(string profileHtml, IEnumerable<string> salariesHtml)
        {
            var publicWorker = new PublicWorker();
            var profileDoc = _htmlParser.Parse(profileHtml);
            ParseProfileResult(publicWorker, profileDoc);

            publicWorker.Salaries = new Dictionary<string, float>();

            foreach(string salaryHtml in salariesHtml)
            {
                var salaryDoc = _htmlParser.Parse(salaryHtml);
                ParseSalaryProfileResult(publicWorker, salaryDoc);
            }

            return publicWorker;
        }

        private void ParseProfileResult(PublicWorker publicWorker, IHtmlDocument doc)
        {
            // those selector values where extracted directly from the page
            publicWorker.Name = BeautifyString(doc.QuerySelector("#resumo > table > tbody > tr:nth-child(1) > td.colunaValor").TextContent);
            publicWorker.CPF = BeautifyString(doc.QuerySelector("#resumo > table > tbody > tr:nth-child(2) > td.colunaValor").TextContent);
            publicWorker.SIAPE = BeautifyString(doc.QuerySelector("#listagemConvenios > table > tbody > tr > td > table > tbody > tr:nth-child(1) > td:nth-child(2) > strong").TextContent);
            publicWorker.WorkingDepartment = BeautifyString(doc.QuerySelector("#listagemConvenios > table > tbody > tr > td > table > tbody > tr:nth-child(14) > td:nth-child(2) > strong").TextContent);
            publicWorker.OriginDepartment = BeautifyString(doc.QuerySelector("#listagemConvenios > table > tbody > tr > td > table > tbody > tr:nth-child(9) > td:nth-child(2) > strong").TextContent);
            publicWorker.Role = BeautifyString(doc.QuerySelector("#listagemConvenios > table > tbody > tr > td > table > tbody > tr:nth-child(2) > td:nth-child(2) > strong").TextContent);
        }

        private void ParseSalaryProfileResult(PublicWorker publicWorker, IHtmlDocument salaryDoc)
        {
            float val, sum = 0;
            var date = salaryDoc.QuerySelector("#listagemConvenios > table > thead > tr.remuneracaohead1 > th").TextContent.Split(' ');
            string month = BeautifyString(date[3]);
            string year = date[5];

            // adds all kinds of income
            foreach (var income in salaryDoc.QuerySelectorAll("#listagemConvenios > table > tbody > tr.remuneracaolinhatotalliquida > td.colunaValor"))
            {
                float.TryParse(income.TextContent, NumberStyles.Currency, new CultureInfo("pt-BR"), out val);
                sum += val;
            }
            
            publicWorker.Salaries.Add(year + "-" + month, sum);        
        }

        private IEnumerable<PublicWorker> ParseSearchTable(IElement table)
        {
            var result = new List<PublicWorker>(); 
            table.FirstElementChild.RemoveChild(table.FirstElementChild.FirstElementChild);
            var childs = table.FirstElementChild.Children;
            foreach(var child in childs)
            {
                result.Add(ParseSearchResult(child));
            }
            return result; 
        }

        /// <summary>
        /// Parses search's individual results
        /// </summary>
        /// <param name="child">A person retrieved bu the search</param>
        /// <returns></returns>
        private PublicWorker ParseSearchResult(IElement child)
        {
            PublicWorker result = new PublicWorker();

            result.CPF = child.Children[0].InnerHtml;

            var name = child.QuerySelector("a");
            result.Id = name.GetAttribute("href").Split('=')[1];
            
            result.Name = BeautifyString(name.InnerHtml);
            result.OriginDepartment = BeautifyString(child.Children[2].InnerHtml);
            result.WorkingDepartment = BeautifyString(child.Children[3].InnerHtml);

            return result;
        } 

        private string BeautifyString(string str)
        {
            string beautifiedString = "";
            foreach (var part in str.ToLower().TrimEnd().Split(' '))
            {
                if (!part.Equals("\n") && !part.Equals(string.Empty))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(part, @"\W")) 
                        beautifiedString += part;
                    else
                        beautifiedString += part.Substring(0,1).ToUpper() +  part.Substring(1) + " ";
                }
                    
            }

            return beautifiedString.TrimEnd();
        }
    }
}