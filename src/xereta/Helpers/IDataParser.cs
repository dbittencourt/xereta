using System.Collections.Generic;
using xereta.Models;

namespace xereta.Helpers
{
    public interface IDataParser
    {
        IEnumerable<SearchResult> ParseSearch(string html);

        PublicWorker Parse(string profileHtml, string salaryHtml);
    }
}