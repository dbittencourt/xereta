using System.Collections.Generic;

namespace xereta.Helpers
{
    public class HTMLParser : IHTMLParser
    {
        public List<string> Parse(string html)
        {
            var result = new List<string>(); 
            result.Add("Funcionou");
            
            return result;   
        }
    }
}