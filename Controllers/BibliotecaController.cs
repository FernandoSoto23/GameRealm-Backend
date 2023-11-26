using GameRealm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameRealm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BibliotecaController : Controller
    {
        [HttpGet]
        [Route("ListarBiblioteca")]
        public RespuestaJson<ListaBiblioteca> ListarBiblioteca(int idUsuario)
        {   
            var lista = new ListaBiblioteca();
            lista = Biblioteca.ListarBibliotecas(idUsuario);
            var JSON = new RespuestaJson<ListaBiblioteca>();
            JSON.Status = true;
            JSON.Msg = "ok";
            JSON.Dato = lista;

            return JSON;


        }
    }
}
