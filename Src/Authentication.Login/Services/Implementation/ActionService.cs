using System.Collections.Generic;
using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;
using ActionEntity = Authentication.Login.Domain.Implementation.Action;

namespace Authentication.Login.Services.Implementation
{
    public class ActionService : IActionService
    {
        private readonly IActionRepository actionRepository;

        public ActionService(IActionRepository actionRepository)
        {
            this.actionRepository = actionRepository;
        }

        public IEnumerable<ActionEntity> GetAll() => actionRepository.GetAll();

        public ActionEntity? GetById(int id) => actionRepository.GetById(id);

        public ActionEntity? GetByName(string name) => actionRepository.GetByName(name);

        public void AddAction(ActionEntity action)
        {
            // Set audit fields for tracking when and by whom the action was created
            action.DtCreated = DateTime.Now;

            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(action.CreatedBy))
            {
                action.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            actionRepository.Add(action);
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

            actionRepository.Update(action);
        }

        public void DeleteAction(int id)
        {
            var action = actionRepository.GetById(id);
            if (action != null)
            {
                actionRepository.Remove(action);
            }
        }
    }
}
