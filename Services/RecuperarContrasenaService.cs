using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using static Sowing_O2.Utilities.Encriptacion;
using Microsoft.EntityFrameworkCore;

namespace Sowing_O2.Services
{
    public class RecuperarContrasenaService
    {
        private readonly RecuperacionTokenRepository _tokenRepository;
        private readonly UsuarioRepositories _usuarioRepository;
        private readonly IEmailSender _emailSender;
        private readonly ISecurityService _securityService;

        public RecuperarContrasenaService(RecuperacionTokenRepository tokenRepository, UsuarioRepositories usuarioRepository, IEmailSender emailSender, ISecurityService securityService)
        {
            _tokenRepository = tokenRepository;
            _usuarioRepository = usuarioRepository;
            _emailSender = emailSender;
            _securityService = securityService;
        }

        public async Task EnviarRecuperacionContrasenaAsync(string correo)
        {
            var usuario = await _usuarioRepository.GetUsuarioPorCorreo(correo);
            if (usuario == null)
                throw new Exception("El correo ingresado no está registrado.");

            string token = GenerarToken();
            var fechaExpiracion = DateTime.UtcNow.AddHours(1);

            await _tokenRepository.AddRecuperacionTokenAsync(new RecuperacionToken
            {
                Correo = correo,
                Token = token,
                FechaExp = fechaExpiracion
            });

            string htmlMessage = $@"
                        <!DOCTYPE html>
                        <html lang=""es"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Recuperación de Contraseña</title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #ffffff;
                                    display: flex;
                                    justify-content: center;
                                    align-items: center;
                                    height: 100vh;
                                    margin: 0;
                                }}
                                .container {{
                                    width: 400px;
                                    background-color: #e7f8cc;
                                    padding: 20px;
                                    text-align: center;
                                    border-radius: 10px;
                                    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                                    color: #3e3d3d;
                                }}
                                .header {{
                                    font-size: 1.5em;
                                    font-weight: bold;
                                    color: #443500;
                                    margin-bottom: 10px;
                                }}
                                .content {{
                                    font-size: 1em;
                                    margin-bottom: 20px;
                                    line-height: 1.5;
                                }}
                                .token-box {{
                                    background-color: #ffffff;
                                    border: 1px solid #ccc;
                                    border-radius: 8px;
                                    padding: 20px;
                                    font-size: 1.2em;
                                    color: #443500;
                                    font-weight: bold;
                                    margin-bottom: 20px;
                                }}
                                .logo {{
                                    margin: 20px 0;
                                }}
                                .footer {{
                                    font-size: 0.9em;
                                    color: #443500;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class=""container"">
                                <div class=""header"">SOLICITUD DE RECUPERACIÓN DE CONTRASEÑA</div>
                                <div class=""content"">
                                    HOLA,<br>
                                    Hemos recibido una solicitud para restablecer tu contraseña. Si realizaste esta solicitud, usa el siguiente token para completar el proceso:
                                </div>
                                <div class=""token-box"">
                                    <p>{token}</p>
                                </div>
                                <div class=""logo"">
                                    <img src=""https://i.pinimg.com/736x/cd/86/7c/cd867c50a0ee3dae78b7050a7a16f09a.jpg"" alt=""Sowing O2 Logo"" width=""150"">
                                </div>
                                <div class=""footer"">
                                    Si no solicitaste restablecer tu contraseña, por favor ignora este mensaje.<br><br>
                                    Puedes usar este token para cambiar tu contraseña en las próximas 24 horas.<br><br>
                                    GRACIAS,<br>
                                    El equipo de Sowing O2
                                </div>
                            </div>
                        </body>
                        </html>";

            await _emailSender.SendEmailAsync(correo, "Recuperación de Contraseña", htmlMessage);

        }

        public async Task ConfirmarRecuperacionContrasenaAsync(ConfirmarRecuperarContrasenaDto dto)
        {
            try
            {
                // Obtener el token desde el repositorio de tokens
                var token = await _tokenRepository.GetTokenAsync(dto.Token);
                if (token == null || token.FechaExp < DateTime.UtcNow)
                {
                    throw new Exception("Token inválido o expirado.");
                }

                // Obtener el usuario correspondiente al correo registrado en el token
                var usuarioExistente = await _usuarioRepository.GetUsuarioPorCorreoModelo(token.Correo);
                if (usuarioExistente == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                // Actualizar la contraseña del usuario existente
                usuarioExistente.Contrasena = _securityService.HashPassword(dto.NuevaContrasena);

                // Guardar los cambios en el repositorio de usuarios
                await _usuarioRepository.UpdateUsuario(usuarioExistente);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error al guardar los cambios: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        private string GenerarToken()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[32];
                cryptoProvider.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }
    }
}
