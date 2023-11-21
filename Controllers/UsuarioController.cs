using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameRealm.Models;

namespace GameRealm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        #region Logueo
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
        #endregion

        #region Validacion
        [HttpGet]
        [Route("validar")]
        public bool Validar(string token)
        {

            bool validacion;
            validacion = Models.Usuario.CheckToken(token);
            return validacion;
        }
        #endregion

        #region AdminLogueo
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
        #endregion

        #region Autentificar administrador
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
        #endregion

        #region Codigo de verificacion
        [HttpGet]
        [Route("CorreoDeVerificacion")]
        public string EnviarCorreoDeVerificacion(string usuario,string codigo)
        {
            return UsuarioStandard.AuthCodigoDeVerificacion(usuario,codigo);
            
        }
        #endregion

        [HttpPost]
        [Route("CrearUsuario")]
        public bool CrearUsuario([FromBody] Models.UsuarioStandard Entidad)
        {
            bool Respuesta = Models.UsuarioStandard.CrearNuevoUsuario(Entidad);
            return Respuesta;
        }
    }



}
