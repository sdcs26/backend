using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Net;
using System.Net.Mail;
using YourProject.Models;

namespace PasswordRecoveryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordRecoveryController : ControllerBase
    {
        private readonly IDatabaseService _dbService;
        private readonly ISecurityService _securityService;

        public PasswordRecoveryController(IDatabaseService dbService, ISecurityService securityService)
        {
            _dbService = dbService;
            _securityService = securityService;
        }

        /// <summary>
        /// Enviar un correo electrónico con el enlace de recuperación de contraseña
        /// </summary>
        /// <param name="email">El correo electrónico del usuario</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("recover")]
        public IActionResult SendRecoveryEmail([FromBody] string email)
        {
            var user = _dbService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // Generar token de recuperación
            string token = GenerateToken();

            // Guardar el token en la base de datos
            _dbService.SaveRecoveryToken(user.id, token);

            // Enviar el correo electrónico
            bool isSent = SendEmail(email, token);

            if (!isSent)
            {
                return StatusCode(500, new { message = "Error al enviar el correo" });
            }

            return Ok(new { message = "Correo enviado exitosamente" });
        }

        /// <summary>
        /// Restablecer la contraseña del usuario
        /// </summary>
        /// <param name="resetRequest">Contiene el token de recuperación y la nueva contraseña</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest resetRequest)
        {
            var user = _dbService.GetUserByToken(resetRequest.Token);
            if (user == null)
            {
                return BadRequest(new { message = "Token inválido o expirado" });
            }

            // Encriptar la nueva contraseña
            string hashedPassword = _securityService.HashPassword(resetRequest.NewPassword);

            // Actualizar la contraseña en la base de datos
            _dbService.UpdateUserPassword(user.id, hashedPassword);

            // Eliminar el token de recuperación
            _dbService.DeleteRecoveryToken(user.id);

            return Ok(new { message = "Contraseña restablecida exitosamente" });
        }

        // Generar token seguro
        private string GenerateToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        // Método para enviar correos electrónicos
        private bool SendEmail(string toEmail, string token)
        {
            try
            {
                // Configuración del servidor SMTP
                var smtpClient = new SmtpClient("smtp.tu_email.com") // Cambia esto por el servidor SMTP que uses
                {
                    Port = 587, // Usualmente 587 para TLS
                    Credentials = new NetworkCredential("tu_email@dominio.com", "tu_contraseña"), // Cambia esto por tus credenciales
                    EnableSsl = true,
                };

                // Crear el mensaje
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("tu_email@dominio.com"), // Cambia esto por tu correo
                    Subject = "Recuperación de Contraseña",
                    Body = $"Haga clic en el siguiente enlace para restablecer su contraseña: \n\n" +
                           $"https://tu_dominio.com/reset-password?token={token}", // Cambia esto por la URL de tu aplicación
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                // Enviar el correo
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Manejar errores (opcional)
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                return false;
            }
        }
    }

    // Clase para las solicitudes de restablecimiento de contraseña
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
