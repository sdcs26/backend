using WebApplication2.Models;
using YourProject.Models;

namespace YourProject.Models
{
    public class MockDatabaseService : IDatabaseService
    {
        private List<cliente> _users = new List<cliente>();
        private Dictionary<string, string> _recoveryTokens = new Dictionary<string, string>();

        public MockDatabaseService()
        {
            _users.Add(new cliente { id = "1", correo = "usuario@example.com", PasswordHash = "hashed_password" });
        }

        public cliente GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.correo == email);
        }

        public void SaveRecoveryToken(string userId, string token)
        {
            if (_recoveryTokens.ContainsKey(userId))
                _recoveryTokens[userId] = token;
            else
                _recoveryTokens.Add(userId, token);
        }

        public cliente GetUserByToken(string token)
        {
            var userId = _recoveryTokens.FirstOrDefault(rt => rt.Value == token).Key;
            return _users.FirstOrDefault(u => u.id == userId);
        }

        public void UpdateUserPassword(string userId, string hashedPassword)
        {
            var user = _users.FirstOrDefault(u => u.id == userId);
            if (user != null)
                user.PasswordHash = hashedPassword;
        }

        public void DeleteRecoveryToken(string userId)
        {
            if (_recoveryTokens.ContainsKey(userId))
                _recoveryTokens.Remove(userId);
        }
    }
}

