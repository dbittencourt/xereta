using System.Collections.Generic;
using xereta.Models;

namespace xereta.Helpers
{
    public interface IDataParser
    {
        IEnumerable<SearchResult> ParseSearch(string searchResult);

        PublicWorker Parse(string profileInformation, string salaryInformation);
    }
}