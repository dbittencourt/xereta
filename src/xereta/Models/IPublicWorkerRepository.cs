using System.Collections.Generic;

namespace xereta.Models
{
    public interface IPublicWorkerRepository
    {
        void Add(PublicWorker worker);
        
        PublicWorker Get(string id);

        IEnumerable<PublicWorker> GetAll();

        void Update(PublicWorker worker);

        void Delete(PublicWorker worker);
        
    }

}