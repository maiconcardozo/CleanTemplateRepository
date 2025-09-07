using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;
using System.Collections.Generic;

namespace Authentication.Login.Repository.Interface
{
    public interface IActionRepository : IEntityRepository<Domain.Implementation.Action>
    {
        Domain.Implementation.Action? GetByName(string name);
        IEnumerable<Domain.Implementation.Action> GetAllActive();
    }
}