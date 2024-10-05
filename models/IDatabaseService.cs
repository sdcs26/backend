using WebApplication2.Models;

namespace YourProject.Models
{
    public interface IDatabaseService
    {
        cliente GetUserByEmail(string email);
        void SaveRecoveryToken(string userId, string token);
        cliente GetUserByToken(string token);
        void UpdateUserPassword(string userId, string hashedPassword);
        void DeleteRecoveryToken(string userId);
    }
}
