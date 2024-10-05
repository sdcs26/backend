using Sowing_O2.Dtos;
using Sowing_O2.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Sowing_O2.Services
{
    public class TokenRevocadoService
    {
        private readonly TokenRevocadoRepositories _tokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenRevocadoService(TokenRevocadoRepositories tokenRepository, IHttpContextAccessor httpContextAccessor)
        {
            _tokenRepository = tokenRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Logout(string token)
        {
            _tokenRepository.AddTokenRevocado(token);
        }

        public bool IsTokenValid(string token)
        {
            return !_tokenRepository.IsTokenRevoked(token);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (IsTokenValid(token))
            {
                await context.Response.WriteAsync("El token es válido.");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("El token ha sido revocado.");
            }
        }
    }
}

