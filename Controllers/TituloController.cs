using Microsoft.AspNetCore.Mvc;
using ServicioRestaurante.Models;
using System.Text.Json;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServicioRestaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TituloController : ControllerBase
    {

        [HttpGet]
        public ListaTitulo GetMenus()
        {
            ListaTitulo lista = new ListaTitulo();
            lista = Titulo.ListarTitulos();
            return lista;
        }

        [HttpGet]
        [Route("ListarxTipo")]
        public ListaTitulo Cargar(int categoria)
        {
            ListaTitulo lista = new ListaTitulo();
            lista = Titulo.ListarTituloPorCategoria(categoria);
            return lista;
        }

        [HttpGet]
        [Route("Titulo")]
        public Titulo Orden(int codigo)
        {
            Titulo orden = new Titulo();
            orden = Titulo.Orden(codigo);
            return orden;
        }

        [HttpPost]
        [Route("Guardar")]
        public bool Guardar([FromBody]TituloAdministrador<Titulo> entidad)
        {
            int id =entidad.Id;
            string token = entidad.Token;
            var menu = entidad.Titulo;
            return Titulo.CrearNuevoAnuncio(menu,id,token);

        }

        [HttpPut]
        [Route("Actualizar")]
        public bool Actualizar(int codigo,[FromBody] TituloAdministrador<Titulo> entidad)
        {
            int id = entidad.Id;
            string token = entidad.Token;
            var menu = entidad.Titulo;
            return Titulo.Actualizar(menu,codigo,id,token);
        }
    }
}
