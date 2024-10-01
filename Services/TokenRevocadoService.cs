using Sowing_O2.Dtos;
using Sowing_O2.Repositories;

namespace Sowing_O2.Services
{
    public interface ITokenRevocadoService
    {
        bool IsTokenRevoked(string token);
        void AddRevokedToken(TokenRevocadoDto tokenRevocadoDto);
    }

    public class TokenRevocadoService : ITokenRevocadoService
    {
        private readonly TokenRevocadoRepositories _tokenRevocadoRepository;

        public TokenRevocadoService(TokenRevocadoRepositories tokenRevocadoRepository)
        {
            _tokenRevocadoRepository = tokenRevocadoRepository;
        }

        public bool IsTokenRevoked(string token)
        {
            return _tokenRevocadoRepository.IsTokenRevoked(token);
        }

        public void AddRevokedToken(TokenRevocadoDto tokenRevocadoDto)
        {
            _tokenRevocadoRepository.AddTokenRevocado(tokenRevocadoDto);
        }
    }
}
