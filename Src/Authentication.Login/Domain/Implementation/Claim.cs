using Authentication.Login.Domain.Interface;
using Authentication.Login.Enum;
using Foundation.Base.Domain.Implementation;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents a claim entity in the Role-Based Access Control (RBAC) system.
    /// Claims define permissions or roles that can be assigned to users through claim-action mappings.
    /// Inherits from Entity base class and implements IClaim interface.
    /// </summary>
    public class Claim : Entity, IClaim
    {
        /// <summary>
        /// Gets or sets the type/category of this claim.
        /// Defines the classification of the permission (e.g., Role, Permission, Policy).
        /// </summary>
        public ClaimType Type { get; set; }
        
        /// <summary>
        /// Gets or sets the unique value/name that identifies this claim.
        /// This is the actual permission identifier (e.g., "UserManagement", "AdminAccess").
        /// Must be unique within the system for proper RBAC functionality.
        /// </summary>
        public string Value { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets a human-readable description of what this claim represents.
        /// Helps administrators understand the purpose and scope of the permission.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the collection of claim-action mappings for this claim.
        /// Represents which system actions this claim grants access to.
        /// Used to build the complete permission matrix for users.
        /// </summary>
        public ICollection<ClaimAction> LstClaimAction { get; set; } = new List<ClaimAction>();

        /// <summary>
        /// Interface implementation for claim-action relationships.
        /// Provides type-safe access to claim-action mappings through the IClaim interface.
        /// </summary>
        ICollection<IClaimAction> IClaim.LstClaimAction
        {
            get => (ICollection<IClaimAction>)LstClaimAction;
            set => LstClaimAction = (ICollection<ClaimAction>)value;
        }
    }
}