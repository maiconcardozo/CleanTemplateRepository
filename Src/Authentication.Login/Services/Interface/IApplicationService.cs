using Authentication.Login.Domain.Implementation;

namespace Authentication.Login.Services.Interface
{
    public interface IApplicationService
    {
        IEnumerable<Application> GetAll();

        Application? GetById(int id);

        Application? GetByName(string name);

        void AddApplication(Application application);

        void UpdateApplication(Application application);

        void DeleteApplication(int id);
    }
}
