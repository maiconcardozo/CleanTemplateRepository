using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Foundation.Base.Repository.Interface;

namespace Authentication.Login.Repository.Interface
{
    public interface IAccountRepository : IEntityRepository<Account>
    {
        Account? GetByUserName(string userName);

        Task<Account?> GetByUserNameAsync(string userName);

        IEnumerable<Account> GetByUserNameAndPasswordList(IEnumerable<string> userNames);

        IEnumerable<Account> GetByUserNameList(IEnumerable<string> userNames);

        void UpdatePassword(string userName, string newPassword);

        void UpdateUserName(string oldUserName, string newUserName);

        void DeleteByUserName(string userName);

        void DeleteByUserNameList(IEnumerable<string> userNames);

        void DeleteByUserNameAndPassword(Account account);

        void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> userNamePasswordPairs);

        void DeleteByUserNameAndPasswordList(IEnumerable<(string UserName, string password)> userNamePasswordPairs, bool isDelete);

        void DeleteByUserNameList(IEnumerable<string> userNames, bool isDelete);
    }
}
