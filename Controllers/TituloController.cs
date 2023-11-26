using Microsoft.AspNetCore.Mvc;
using GameRealm.Models;
using System.Text.Json;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameRealm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TituloController : ControllerBase
    {
        
        [HttpGet]
        [Route("ListarTitulos")]
        public RespuestaJson<ListaTitulo>GetMenus()
        {
            var JSON = new RespuestaJson<ListaTitulo>();
            ListaTitulo lista = new ListaTitulo();
            lista = Titulo.ListarTitulos();
            JSON.Status = true;
            JSON.Msg = "ok";
            JSON.Dato = lista;

            return JSON;
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
        [Route("ObtenerTitulo")]
        public Titulo ObtenerTitulo(int codigo)
        {
            Titulo titulo = new Titulo();
            titulo = Titulo.Producto(codigo);
            return titulo;
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
