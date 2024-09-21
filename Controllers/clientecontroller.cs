using Microsoft.AspNetCore.Mvc;
using WebApplication2.models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class clientecontroller : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public dynamic listarcliente()
        {
            List<cliente> clientes = new List<cliente>
            {
                new cliente

                {
                    id="1",
                    correo="pruebas1@gmail.com",
                    edad="24",
                    nombre="juan"
                }

                /* new cliente

                {
                    id="2",
                    correo="pruebas2@gmail.com",
                    edad="24",
                    nombre="pepe"
                }
                */

            };

            return clientes;



            //}
            //[HttpPost]
            //[Route("guardar")]
            //public dynamic guardarcliente()



            //}
        }
    }
}
