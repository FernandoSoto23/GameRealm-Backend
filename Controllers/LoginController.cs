using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioRestaurante.Models;

namespace ServicioRestaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        [HttpGet]
        [Route("Loguear")]
        public RespuestaJson<UsuarioStandard> Loguear(string email,string pwd)
        {
            pwd = Encrypt.GetSHA256(pwd);
            var usuario = UsuarioStandard.Loguear(email,pwd);
            var JSON = new RespuestaJson<UsuarioStandard>();
            if (usuario.Token != null)
            {
                 
                JSON.Status = true;
                JSON.Msg= "ok";
                JSON.Dato = usuario;
            }
            else
            {
                JSON.Status = false;
                JSON.Msg = "error";
            }

            return JSON;


        }

        [HttpGet]
        [Route("validar")]
        public bool Validar(string token)
        {

            bool validacion;
            validacion = Models.Usuario.CheckToken(token);
            return validacion;
        }






        [HttpGet]
        [Route("admin")]
        public RespuestaJson<Usuario> Admin(string email, string pwd)
        {
            pwd = Encrypt.GetSHA256(pwd);
            var usuario = Administrador.LoguearAdmin(email, pwd);
            var JSON = new RespuestaJson<Usuario>();
            if (usuario.Token != null)
            {

                JSON.Status = true;
                JSON.Msg = "ok";
                JSON.Dato = usuario;
            }
            else
            {
                JSON.Status = false;
                JSON.Msg = "error";
            }

            return JSON;


        }

        [HttpGet]
        [Route("Auth")]
        public RespuestaJson<Usuario> AutentificaAdministrador(string token)
        {
            var usuario = Administrador.AutentificarAdministrador(token);
            var JSON = new RespuestaJson<Usuario>();
            if (usuario.Token != null)
            {

                JSON.Status = true;
                JSON.Msg = "ok";
                JSON.Dato = usuario;
            }
            else
            {
                JSON.Status = false;
                JSON.Msg = "error";
            }

            return JSON;


        }

        [HttpGet]
        [Route("CorreoDeVerificacion")]
        public void EnviarCorreoDeVerificacion(string email)
        {
            UsuarioStandard.EnviarCorreo(email);
        }
    }

        //[HttpPost]
        //[Route("CrearUsuario")]
        //public bool CrearUsuario([FromBody] Usuario Entidad)
        //{
        //    bool s = false;
        //    return s;
        //}
    
}
