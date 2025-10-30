using Foundation.Base.Util;

namespace Authentication.Login.Services
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return StringHelper.HashArgon2(password);
        }

        public bool Verify(string password, string hash)
        {
            return StringHelper.VerifyArgon2Hash(password, hash);
        }
    }
}
