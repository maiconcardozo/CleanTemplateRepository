using Authentication.Login.Domain.Implementation;
using ActionEntity = Authentication.Login.Domain.Implementation.Action;

namespace Authentication.Login.Services.Interface
{
    public interface IActionService
    {
        IEnumerable<ActionEntity> GetAll();
        ActionEntity? GetById(int id);
        ActionEntity? GetByName(string name);
        void AddAction(ActionEntity action);
        void UpdateAction(ActionEntity action);
        void DeleteAction(int id);
    }
}