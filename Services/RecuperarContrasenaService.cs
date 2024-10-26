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
                    <html lang='es'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                color: #333;
                                line-height: 1.6;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 20px auto;
                                background: #ffffff;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            h2 {{
                                color: #4CAF50;
                            }}
                            .button {{
                                background-color: #4CAF50;
                                color: white;
                                padding: 10px 20px;
                                text-decoration: none;
                                border-radius: 5px;
                                display: inline-block;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 0.9em;
                                color: #777;
                            }}
                        </style>
                        <title>Recuperación de Contraseña</title>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Solicitud de Recuperación de Contraseña</h2>
                            <p>Hola,</p>
                            <p>Hemos recibido una solicitud para restablecer tu contraseña. Si realizaste esta solicitud, usa el siguiente token para completar el proceso:</p>
                            <p style='font-size: 1.2em; font-weight: bold; color: #333;'>{token}</p>
                            <p>Si no solicitaste restablecer tu contraseña, por favor ignora este mensaje.</p>
                            <p>Puedes usar este token para cambiar tu contraseña en las próximas 24 horas.</p>
                            <div class='footer'>
                                <p>Gracias,<br>El equipo de Sowing O2</p>
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
