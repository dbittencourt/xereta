using System.Collections.Generic;

namespace xereta.Helpers
{
    public interface IHTMLParser
    {
        List<string> Parse(string html);
    }
}