using System.Collections.Generic;
using AngleSharp.Dom;
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
        public IEnumerable<SearchResult> ParseSearch(string html)
        {
            var table = _htmlParser.Parse(html).QuerySelector(@"table[summary]");        
            var result = ParseSearchTable(table);

            return result;   
        }

        public PublicWorker Parse(string profileHtml, string salaryHtml)
        {
            var table = _htmlParser.Parse(profileHtml).QuerySelector(@"table[summary]");
            var publicWorker = ParseProfileResult(table);

            return publicWorker;
        }

        private PublicWorker ParseProfileResult(IElement table)
        {
            var publicWorker = new PublicWorker();
            table.FirstElementChild.RemoveChild(table.FirstElementChild.FirstElementChild);

            publicWorker.CPF = "";
            publicWorker.Id = "";
            publicWorker.Name = "";
            publicWorker.WorkingDepartment = "";
            publicWorker.OriginDepartment = "";

            return publicWorker;
        }

        private IEnumerable<SearchResult> ParseSearchTable(IElement table)
        {
            var result = new List<SearchResult>(); 
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
        private SearchResult ParseSearchResult(IElement child)
        {
            SearchResult result = new SearchResult();

            result.CPF = child.Children[0].InnerHtml;

            var name = child.QuerySelector("a");
            result.Id = name.GetAttribute("href").Split('=')[1];
            
            result.Name = BeautifyString(name.InnerHtml).TrimEnd();
            result.OriginDepartment = BeautifyString(child.Children[2].InnerHtml).TrimEnd();
            result.WorkingDepartment = BeautifyString(child.Children[3].InnerHtml).TrimEnd();

            return result;
        } 

        private string BeautifyString(string str)
        {
            string beautifiedString = "";
            foreach (var part in str.ToLower().TrimEnd().Split(' '))
            {
                beautifiedString += part.Substring(0,1).ToUpper() +  part.Substring(1) + " ";
            }

            return beautifiedString;
        }
    }
}