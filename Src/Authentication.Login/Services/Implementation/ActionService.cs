using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;
using System.Collections.Generic;
using ActionEntity = Authentication.Login.Domain.Implementation.Action;

namespace Authentication.Login.Services.Implementation
{
    public class ActionService : IActionService
    {
        private readonly IActionRepository _actionRepository;

        public ActionService(IActionRepository actionRepository)
        {
            _actionRepository = actionRepository;
        }

        public IEnumerable<ActionEntity> GetAll() => _actionRepository.GetAll();

        public ActionEntity? GetById(int id) => _actionRepository.GetById(id);

        public ActionEntity? GetByName(string name) => _actionRepository.GetByName(name);

        public void AddAction(ActionEntity action)
        {
            // Set audit fields for tracking when and by whom the action was created
            action.DtCreated = DateTime.Now;
            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(action.CreatedBy))
            {
                action.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _actionRepository.Add(action);
        }

        public void UpdateAction(ActionEntity action)
        {
            // Update audit fields for tracking modifications
            action.DtUpdated = DateTime.Now;
            // Use the UpdatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(action.UpdatedBy))
            {
                action.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _actionRepository.Update(action);
        }

        public void DeleteAction(int id)
        {
            var action = _actionRepository.GetById(id);
            if (action != null)
                _actionRepository.Remove(action);
        }
    }
}