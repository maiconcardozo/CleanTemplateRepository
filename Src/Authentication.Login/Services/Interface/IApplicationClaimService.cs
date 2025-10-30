using Authentication.Login.Domain.Implementation;

namespace Authentication.Login.Services.Interface
{
    public interface IApplicationClaimService
    {
        IEnumerable<ApplicationClaim> GetAll();

        ApplicationClaim? GetById(int id);

        IEnumerable<ApplicationClaim> GetByApplicationId(int applicationId);

        IEnumerable<ApplicationClaim> GetByClaimId(int claimId);

        void AddApplicationClaim(ApplicationClaim applicationClaim);

        void UpdateApplicationClaim(ApplicationClaim applicationClaim);

        void DeleteApplicationClaim(int id);
    }
}
