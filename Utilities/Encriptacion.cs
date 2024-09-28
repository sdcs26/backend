namespace Sowing_O2.Utilities
{
    public class Encriptacion
    {
        public interface ISecurityService
        {
            string HashPassword(string password);
            bool VerifyPassword(string password, string hashedPassword);
        }

        public class SecurityService : ISecurityService
        {
            public string HashPassword(string password)
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }

            public bool VerifyPassword(string password, string hashedPassword)
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
        }
    }
}
