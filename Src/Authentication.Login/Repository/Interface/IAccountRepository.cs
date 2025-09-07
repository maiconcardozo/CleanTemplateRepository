using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Foundation.Base.Repository.Interface;

namespace Authentication.Login.Repository.Interface
{
    public interface IAccountRepository : IEntityRepository<Account>
    {
        Account? GetByUserName(string UserName);
        Task<Account?> GetByUserNameAsync(string UserName);
        IEnumerable<Account> GetByUserNameAndPasswordList(IEnumerable<string> UserNames);
        IEnumerable<Account> GetByUserNameList(IEnumerable<string> UserNames);
        void UpdatePassword(string UserName, string newPassword);
        void UpdateUserName(string oldUserName, string newUserName);
        void DeleteByUserName(string UserName);
        void DeleteByUserNameList(IEnumerable<string> UserNames);
        void DeleteByUserNameAndPassword(Account account);
        void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> UserNamePasswordPairs);
        void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> UserNamePasswordPairs, bool isDelete);
        void DeleteByUserNameList(IEnumerable<string> UserNames, bool isDelete);
    }
}
