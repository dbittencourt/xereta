using System.Collections.Generic;

namespace xereta.Helpers
{
    public interface IHTMLParser
    {
        IEnumerable<string> Parse(string html);
    }
}