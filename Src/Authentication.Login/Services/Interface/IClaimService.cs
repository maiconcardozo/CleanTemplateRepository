using System.Collections.Generic;
using Authentication.Login.Domain.Implementation;

namespace Authentication.Login.Services.Interface
{
    /// <summary>
    /// Service interface for managing claims in the Role-Based Access Control (RBAC) system.
    /// Claims represent permissions or roles that can be assigned to users through claim-action mappings.
    /// </summary>
    public interface IClaimService
    {
        /// <summary>
        /// Retrieves all claims defined in the system.
        /// </summary>
        /// <returns>Collection of all claim entities.</returns>
        IEnumerable<Claim> GetAll();

        /// <summary>
        /// Retrieves a specific claim by its unique identifier.
        /// </summary>
        /// <param name="id">Unique claim identifier.</param>
        /// <returns>Claim entity if found, null otherwise.</returns>
        Claim? GetById(int id);

        /// <summary>
        /// Retrieves a claim by its value/name.
        /// </summary>
        /// <param name="value">Claim value to search for.</param>
        /// <returns>Claim entity if found, null otherwise.</returns>
        Claim? GetByValue(string value);

        /// <summary>
        /// Creates a new claim in the system.
        /// </summary>
        /// <param name="claim">Claim entity to create.</param>
        void AddClaim(Claim claim);

        /// <summary>
        /// Updates an existing claim entity.
        /// </summary>
        /// <param name="claim">Claim entity with updated information.</param>
        void UpdateClaim(Claim claim);

        /// <summary>
        /// Deletes a claim by its unique identifier.
        /// </summary>
        /// <param name="id">Claim ID to delete.</param>
        void DeleteClaim(int id);
    }
}
