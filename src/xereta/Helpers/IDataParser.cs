using System.Collections.Generic;
using xereta.Models;

namespace xereta.Helpers
{
    public interface IDataParser
    {
        List<SearchResult> Parse(string html);
    }
}