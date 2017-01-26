using System.Threading.Tasks;

namespace xereta.Helpers
{
    public interface IDataRetriever
    {
         Task<string> SearchAsync(string query);

         Task<string> GetProfileAsync(string id);

         Task<string> GetProfileSalaryAsync(string id);
    }
}