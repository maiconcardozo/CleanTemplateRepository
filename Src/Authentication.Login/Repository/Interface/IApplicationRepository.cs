using System.Collections.Generic;
using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;

namespace Authentication.Login.Repository.Interface
{
    public interface IApplicationRepository : IEntityRepository<Application>
    {
        Application? GetByName(string name);

        IEnumerable<Application> GetAllActive();
    }
}
