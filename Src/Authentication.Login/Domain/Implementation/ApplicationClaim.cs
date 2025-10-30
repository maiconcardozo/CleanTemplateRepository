using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents the many-to-many relationship between Application and Claim entities.
    /// ApplicationClaim maps which claims are available within a specific application context.
    /// This enables application-level permission discrimination and multi-tenant scenarios.
    /// Inherits from Entity base class and implements IApplicationClaim interface.
    /// </summary>
    [DebuggerDisplay("IdApplication={IdApplication}, IdClaim={IdClaim}, Id={Id}")]
    public class ApplicationClaim : Entity, IApplicationClaim
    {
        /// <summary>
        /// Gets or sets the foreign key reference to the Application entity.
        /// Links this mapping to a specific application.
        /// </summary>
        public int IdApplication { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the Application entity.
        /// Provides access to the full application details for this mapping.
        /// </summary>
        public Application Application { get; set; } = null!;

        /// <summary>
        /// Gets or sets the foreign key reference to the Claim entity.
        /// Links this mapping to a specific claim/permission.
        /// </summary>
        public int IdClaim { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the Claim entity.
        /// Provides access to the full claim details for this mapping.
        /// </summary>
        public Claim Claim { get; set; } = null!;

        /// <summary>
        /// Gets or sets interface implementation for application relationship.
        /// Provides type-safe access to application through the IApplicationClaim interface.
        /// </summary>
        IApplication IApplicationClaim.Application
        {
            get => Application;
            set => Application = (Application)value;
        }

        /// <summary>
        /// Gets or sets interface implementation for claim relationship.
        /// Provides type-safe access to claim through the IApplicationClaim interface.
        /// </summary>
        IClaim IApplicationClaim.Claim
        {
            get => Claim;
            set => Claim = (Claim)value;
        }
    }
}
