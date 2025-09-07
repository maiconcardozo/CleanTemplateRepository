namespace Authentication.Login.DTO
{
    public class ActionResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DtCreated { get; set; }
        public DateTime? DtDeleted { get; set; }
        public DateTime? DtUpdated { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}