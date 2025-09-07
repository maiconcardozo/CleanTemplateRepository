namespace Authentication.Login.Enum
{
    /// <summary>
    /// Represents the type of claim associated with a user or entity.
    /// </summary>
    public enum ClaimType
    {
        /// <summary>
        /// Indicates a claim that grants a specific permission or access right.
        /// Example: "CanEditUser", "ViewReports".
        /// </summary>
        Permission,

        /// <summary>
        /// Indicates a claim that specifies a role assigned to the user.
        /// Example: "Administrator", "User", "Manager".
        /// </summary>
        Role,

        /// <summary>
        /// Indicates a custom claim type, used for application-specific or non-standard claims.
        /// Example: "Department", "EmployeeId".
        /// </summary>
        Custom
    }
}
