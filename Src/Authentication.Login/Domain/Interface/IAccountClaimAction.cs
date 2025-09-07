namespace Authentication.Login.Domain.Interface
{
    public interface IAccountClaimAction
    {
        int Id { get; set; }
        int IdAccount { get; set; }
        IAccount Account { get; set; }
        int IdClaimAction { get; set; }
        IClaimAction ClaimAction { get; set; }
    }
}