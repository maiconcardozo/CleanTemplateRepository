namespace Authentication.Login.DTO
{
    public class ActionPayLoadDTO
    {
        public string Name { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}