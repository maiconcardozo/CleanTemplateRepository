using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;
using System.Collections.Generic;

namespace Authentication.Login.Repository.Interface
{
    public interface IClaimRepository : IEntityRepository<Claim>
    {
        Claim? GetByValue(string value);
        new Claim? GetById(int id);

        IEnumerable<Claim> GetAllActive();
        new void Update(Claim claim);
    }
}