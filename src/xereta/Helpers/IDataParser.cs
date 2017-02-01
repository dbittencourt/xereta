using System.Collections.Generic;
using xereta.Models;

namespace xereta.Helpers
{
    public interface IDataParser
    {
        IEnumerable<PublicWorker> ParseSearch(string searchResult);

        PublicWorker Parse(string profileInformation, IEnumerable<string> salaryInformation);
    }
}