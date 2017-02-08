using System.Collections.Generic;
using System.Threading.Tasks;

namespace xereta.Core.Helpers
{
    public interface IDataRetriever
    {
         Task<string> SearchAsync(string query);

         Task<string> GetProfileAsync(string id);

         Task<IEnumerable<string>> GetProfileSalaryAsync(string id, int numberOfSalaries);
    }
}