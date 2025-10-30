using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class AccountRepository : EntityRepository<Account>, IAccountRepository
    {
        private readonly DbContext context;

        public AccountRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public Account? GetByUserName(string userName)
        {
            return Context.Set<Account>().FirstOrDefault(a => a.UserName == userName);
        }

        public async Task<Account?> GetByUserNameAsync(string userName)
        {
            return await Context.Set<Account>().FirstOrDefaultAsync(a => a.UserName == userName);
        }

        public IEnumerable<Account> GetByUserNameAndPasswordList(IEnumerable<string> userNames)
        {
            return Context.Set<Account>().Where(a => userNames.Contains(a.UserName)).ToList();
        }

        public IEnumerable<Account> GetByUserNameList(IEnumerable<string> userNames)
        {
            return Context.Set<Account>().Where(a => userNames.Contains(a.UserName)).ToList();
        }

        public void UpdatePassword(string userName, string newPassword)
        {
            var account = GetByUserName(userName);
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

        public void DeleteByUserName(string userName)
        {
            var account = GetByUserName(userName);
            if (account != null)
            {
                Remove(account); // Use base soft delete method instead of hard delete
            }
        }

        public void DeleteByUserNameList(IEnumerable<string> userNames)
        {
            var accounts = GetByUserNameList(userNames);
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

        public void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> userNamePasswordPairs)
        {
            var accounts = GetByUserNameAndPasswordList(userNamePasswordPairs.Select(lp => lp.UserName).ToList());

            if (accounts != null && accounts.Any())
            {
                RemoveRange(accounts); // Use base soft delete method instead of hard delete
            }
        }

        public void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> userNamePasswordPairs, bool isDelete)
        {
            if (isDelete)
            {
                DeleteByUserNameAndPasswordList(userNamePasswordPairs);
            }
        }

        public void DeleteByUserNameList(IEnumerable<string> userNames, bool isDelete)
        {
            if (isDelete)
            {
                DeleteByUserNameList(userNames);
            }
        }
    }
}
