using System.Collections.Generic;

namespace Authentication.Login.Domain.Interface
{
    public interface IApplication
    {
        int Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        ICollection<IApplicationClaim> LstApplicationClaim { get; set; }
    }
}
