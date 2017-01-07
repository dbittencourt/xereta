using System.Collections.Generic;
using AngleSharp.Dom;
using xereta.Models;

namespace xereta.Helpers
{
    public class HTMLParser : IDataParser
    {

        private readonly string htmlResultId = "listagem";
        AngleSharp.Parser.Html.HtmlParser _htmlParser;

        public HTMLParser()
        {
            _htmlParser = new AngleSharp.Parser.Html.HtmlParser();
        } 
        public List<SearchResult> Parse(string html)
        {
            var table = _htmlParser.Parse(html).QuerySelector(@"table[summary]");        
            var result = ParseTable(table);

            return result;   
        }

        private List<SearchResult> ParseTable(IElement table)
        {
            var result = new List<SearchResult>(); 
            table.FirstElementChild.RemoveChild(table.FirstElementChild.FirstElementChild);
            var childs = table.FirstElementChild.Children;
            foreach(var child in childs)
            {
                result.Add(ParseResult(child));
            }
            return result; 
        }

        /// <summary>
        /// Parses search's individual results
        /// </summary>
        /// <param name="child">A person retrieved bu the search</param>
        /// <returns></returns>
        private SearchResult ParseResult(IElement child)
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