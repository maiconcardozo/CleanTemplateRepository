using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.Login.Repository.Implementation
{
    public class ClaimRepository : EntityRepository<Claim>, IClaimRepository
    {
        private readonly DbContext _context;

        public ClaimRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public Claim? GetByValue(string value)
        {
            return _context.Set<Claim>().FirstOrDefault(c => c.Value == value);
        }

        public IEnumerable<Claim> GetAllActive()
        {
            return _context.Set<Claim>().Where(c => c.IsActive).ToList();
        }

        public new Claim? GetById(int id)
        {
            return _context.Set<Claim>().FirstOrDefault(c => c.Id == id);
        }

        public new void Update(Claim claim)
        {
            var existingClaim = _context.Set<Claim>().FirstOrDefault(c => c.Id == claim.Id);
           
            if (existingClaim != null)
            {
                existingClaim.Type = claim.Type;
                existingClaim.Value = claim.Value;
                existingClaim.Description = claim.Description;
                existingClaim.IsActive = claim.IsActive;
                existingClaim.DtUpdated = DateTime.UtcNow;
                existingClaim.UpdatedBy = claim.UpdatedBy;
                _context.Update(existingClaim);
                _context.SaveChanges();
            }
        }
    }
}