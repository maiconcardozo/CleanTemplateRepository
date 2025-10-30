using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents a system action entity in the Role-Based Access Control (RBAC) system.
    /// Actions define specific operations that can be performed in the system (e.g., Create, Read, Update, Delete).
    /// Can be mapped to claims to create fine-grained permission systems.
    /// </summary>
    [DebuggerDisplay("Name={Name}, Id={Id}")]
    public class Action : Entity, IAction
    {
        /// <summary>
        /// Gets or sets the name/identifier of this action.
        /// Represents a specific operation that can be performed in the system.
        /// Examples: "Create", "Read", "Update", "Delete", "Execute", "Approve".
        /// Should be descriptive and follow consistent naming conventions.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of claim-action mappings for this action.
        /// Represents which claims/permissions grant access to perform this action.
        /// Used in RBAC to determine if a user can perform specific operations.
        /// </summary>
        public ICollection<ClaimAction> LstClaimAction { get; set; } = new List<ClaimAction>();

        /// <summary>
        /// Gets or sets interface implementation for claim-action relationships.
        /// Provides type-safe access to claim-action mappings through the IAction interface.
        /// Enables polymorphic handling of action entities across different contexts.
        /// </summary>
        ICollection<IClaimAction> IAction.LstClaimAction
        {
            get => (ICollection<IClaimAction>)LstClaimAction;
            set => LstClaimAction = (ICollection<ClaimAction>)value;
        }
    }
}
