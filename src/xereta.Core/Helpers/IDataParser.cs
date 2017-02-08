using System.Collections.Generic;
using xereta.Core.Models;

namespace xereta.Core.Helpers
{
    public interface IDataParser
    {
        IEnumerable<PublicWorker> ParseSearch(string searchResult);

        PublicWorker Parse(string id, string profileInformation, IEnumerable<string> salaryInformation);
    }
}