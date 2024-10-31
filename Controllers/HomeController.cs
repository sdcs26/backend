using Front_Prueba.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Front_Prueba.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult CrearUsuario()
        {
            return View();
        }
        public IActionResult RecuperarContra()
        {
            return View();
        }
        public IActionResult ConfirmarTokenContrasena()
        {
            return View();
        }
        public IActionResult LoginExitoso()
        {
            return View(); 
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListarUsuarios()
        {
            return View();
        }
        public IActionResult RegistroInventario()
        {
            return View();
        }
        public IActionResult VistaInventario()
        {
            return View();
        }
        public IActionResult Traslados()
        {
            return View();
        }
        public IActionResult EditarCantidad()
        {
            return View();
        }
        public IActionResult ListarPedidos()
        {
            return View();
        }
        public IActionResult RegistrarPedidos()
        {
            return View();
        }
        public IActionResult CambiarEstado()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
