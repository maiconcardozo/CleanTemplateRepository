using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class AccountRepository : EntityRepository<Account>, IAccountRepository
    {
        private readonly DbContext _context;

        public AccountRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public Account? GetByUserName(string UserName)
        {
            return Context.Set<Account>().FirstOrDefault(a => a.UserName == UserName);
        }

        public async Task<Account?> GetByUserNameAsync(string UserName)
        {
            return await Context.Set<Account>().FirstOrDefaultAsync(a => a.UserName == UserName);
        }

        public IEnumerable<Account> GetByUserNameAndPasswordList(IEnumerable<string> UserNames)
        {
            return Context.Set<Account>().Where(a => UserNames.Contains(a.UserName)).ToList();
        }

        public IEnumerable<Account> GetByUserNameList(IEnumerable<string> UserNames)
        {
            return Context.Set<Account>().Where(a => UserNames.Contains(a.UserName)).ToList();
        }

        public void UpdatePassword(string UserName, string newPassword)
        {
            var account = GetByUserName(UserName);
            if (account != null)
            {
                account.Password = newPassword;
                Context.Set<Account>().Update(account);
            }
        }

        public void UpdateUserName(string oldUserName, string newUserName)
        {
            var account = GetByUserName(oldUserName);
            if (account != null)
            {
                account.UserName = newUserName;
                Context.Set<Account>().Update(account);
            }
        }

        public void DeleteByUserName(string UserName)
        {
            var account = GetByUserName(UserName);
            if (account != null)
            {
                Remove(account); // Use base soft delete method instead of hard delete
            }
        }

        public void DeleteByUserNameList(IEnumerable<string> UserNames)
        {
            var accounts = GetByUserNameList(UserNames);
            if (accounts != null && accounts.Any())
            {
                RemoveRange(accounts); // Use base soft delete method instead of hard delete
            }
        }

        public void DeleteByUserNameAndPassword(Account account)
        {
            var accountDeleted = GetByUserName(account.UserName);

            if (accountDeleted != null)
            {
                Remove(accountDeleted); // Use base soft delete method instead of hard delete
            }
        }

        public void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> UserNamePasswordPairs)
        {
            var accounts = GetByUserNameAndPasswordList(UserNamePasswordPairs.Select(lp => lp.UserName).ToList());

            if (accounts != null && accounts.Any())
            {
                RemoveRange(accounts); // Use base soft delete method instead of hard delete
            }
        }

        public void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> UserNamePasswordPairs, bool isDelete)
        {
            if (isDelete)
            {
                DeleteByUserNameAndPasswordList(UserNamePasswordPairs);
            }
        }

        public void DeleteByUserNameList(IEnumerable<string> UserNames, bool isDelete)
        {
            if (isDelete)
            {
                DeleteByUserNameList(UserNames);
            }
        }
    }
}
