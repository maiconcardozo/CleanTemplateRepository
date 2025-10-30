using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents an application entity in the authentication system.
    /// Applications are used to discriminate and organize claims per application/system.
    /// This enables multi-tenant or multi-application permission management.
    /// Inherits from Entity base class and implements IApplication interface.
    /// </summary>
    [DebuggerDisplay("Name={Name}, Id={Id}")]
    public class Application : Entity, IApplication
    {
        /// <summary>
        /// Gets or sets the unique name/identifier of this application.
        /// Represents the application identifier in the system.
        /// Examples: "WebPortal", "MobileApp", "AdminPanel".
        /// Should be descriptive and follow consistent naming conventions.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a human-readable description of what this application represents.
        /// Helps administrators understand the purpose and scope of the application.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of application-claim mappings for this application.
        /// Represents which claims are associated with this application.
        /// Used to build application-specific permission matrices.
        /// </summary>
        public ICollection<ApplicationClaim> LstApplicationClaim { get; set; } = new List<ApplicationClaim>();

        /// <summary>
        /// Gets or sets interface implementation for application-claim relationships.
        /// Provides type-safe access to application-claim mappings through the IApplication interface.
        /// </summary>
        ICollection<IApplicationClaim> IApplication.LstApplicationClaim
        {
            get => (ICollection<IApplicationClaim>)LstApplicationClaim;
            set => LstApplicationClaim = (ICollection<ApplicationClaim>)value;
        }
    }
}
