namespace Authentication.Login.Domain.Interface
{
    public interface IApplicationClaim
    {
        int Id { get; set; }

        int IdApplication { get; set; }

        IApplication Application { get; set; }

        int IdClaim { get; set; }

        IClaim Claim { get; set; }
    }
}
