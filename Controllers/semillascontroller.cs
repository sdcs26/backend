using Microsoft.AspNetCore.Mvc;
using WebApplication2.models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("semillas")]
    public class semillascontroller : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public dynamic listarcliente()
        {
            List<semilla> semilla = new List<semilla>
            {
                new semilla
                {
                    id="1",
                   nombre="calabaza",
                   cantidad="10000",
                    
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

            return semilla;



            //}
            //[HttpPost]
            //[Route("guardar")]
            //public dynamic guardarcliente()



            //}
        }
    }
}
