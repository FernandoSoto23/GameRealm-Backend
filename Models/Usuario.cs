using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace GameRealm.Models
{
    public class Usuario
    {
      
        #region Atributos 

        private int id;
        private string nombre;
        private string nombreUsuario;
        private string email;
        private string pwd;
        private string telefono;
        private string token = "";
        private string codigoVerificacion = "";
        private string fechaDeNacimiento;
        private string pais;

        #endregion

        #region Propiedades/Getters y Setters
        public int Id {
            get { return id; }
            set {  id = value; } 
        }
        
        public string Nombre {
            get { return nombre; }
            set { nombre = value; }
        }
        public string NombreUsuario {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }
        public string Email {
            get { return email; }
            set { email = value; }
        }
        public string Pwd {
            get { return pwd; }
            set { pwd = value; }
        }
        public string Telefono {
            get { return telefono; }
            set { telefono = value; }
        }

        public string Token {
            get { return token; }
            set { token = value; }
        }
        public string CodigoVerificacion
        {
            get { return codigoVerificacion; }
            set { codigoVerificacion = value; }

        }
        public string FechaDeNacimiento
        {
            get { return fechaDeNacimiento; }
            set { fechaDeNacimiento = value; }

        }
        public string Pais
        {
            get { return pais; }
            set { pais = value; }

        }
        #endregion

        #region Metodos de la clase

        public static bool CheckToken(string token)
        {
            bool verificar = false;
            Datos.Conectar();
            Usuario usuario = new Usuario();
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("SELECT * FROM usuario WHERE token = @token", Datos.conx);
            cmd.Parameters.AddWithValue("@token", token);
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                verificar = true;
            }
            Datos.Desconectar();
            return verificar;
        }

        #endregion

    }


    public class UsuarioStandard : Usuario
    {
        #region Atributos
        private bool activo;

        #endregion

        #region Propiedades
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }
        #endregion

        #region Metodos de la Clase

        public static UsuarioStandard ObtenerUsuarioPorId(int id)
        {
            UsuarioStandard u = new UsuarioStandard();
            Datos.Conectar();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE id = @id", Datos.conx);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    u.Id = int.Parse(dr["id"].ToString());
                    u.Nombre = dr["nombreCompleto"].ToString();
                    u.NombreUsuario = dr["nombreUsuario"].ToString();
                    u.Email = dr["email"].ToString();
                    u.Telefono = dr["telefono"].ToString();
                    u.Activo = dr["activo"].ToString() == "s";
                    u.Token = dr["token"].ToString();
                    u.FechaDeNacimiento = dr["fecha"].ToString();
                    u.Pais = dr["pais"].ToString();
                    
                }
            }
            catch (Exception)
            {
                Datos.Desconectar();
                throw;
            }
            Datos.Desconectar();
            return u;
        }

        public static UsuarioStandard Loguear(string email, string pwd)
        {
            UsuarioStandard u = new UsuarioStandard();
            Datos.Conectar();
            SqlCommand cmd = new SqlCommand("select * from usuario where email = @email and pwd = @pwd and [Admin] = 'n' and activo = 's'", Datos.conx);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@Pwd", pwd);
            SqlDataReader dr;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    u.Id = int.Parse(dr["id"].ToString());
                    u.Nombre = dr["nombreCompleto"].ToString();
                    u.NombreUsuario = dr["nombreUsuario"].ToString();
                    u.Email = dr["email"].ToString();
                    u.Pwd = dr["pwd"].ToString();
                    u.Telefono = dr["telefono"].ToString();
                    u.Activo = dr["activo"].ToString() == "s";
                    u.Token = dr["token"].ToString();
                    u.FechaDeNacimiento = dr["fecha"].ToString();
                    u.Pais = dr["pais"].ToString();
                }
            }
            catch (Exception)
            {
                Datos.Desconectar();
                throw;
            }
            Datos.Desconectar();
            return u;
        }
        public static bool CrearNuevoUsuario(UsuarioStandard Entidad)
        {
            bool respuesta = false;
            UsuarioStandard u = new UsuarioStandard();
            Datos.Conectar();
            string CodigoDeVerificacion = EnviarCorreo(Entidad.Email);
            string token = Encrypt.GenerarToken();
            string password = Encrypt.GetSHA256(Entidad.Pwd);
            string query = "spAddUser @nombreCompleto,@nombreUsuario,@email,@pwd,@telefono,@codigo,@token";
            SqlCommand cmd = new SqlCommand(query, Datos.conx);
            cmd.Parameters.AddWithValue("@nombreCompleto", Entidad.Nombre);
            cmd.Parameters.AddWithValue("@nombreUsuario", Entidad.NombreUsuario);
            cmd.Parameters.AddWithValue("@email", Entidad.Email);
            cmd.Parameters.AddWithValue("@pwd", password);
            cmd.Parameters.AddWithValue("@telefono", Entidad.Telefono);
            cmd.Parameters.AddWithValue("@codigo", CodigoDeVerificacion);
            cmd.Parameters.AddWithValue("token", token);


            try
            {
                cmd.ExecuteNonQuery();
                respuesta = true;
                Datos.Desconectar();
                return respuesta;
            }
            catch (Exception)
            {
                Datos.Desconectar();
                return respuesta;
            }

        }
        public static string EnviarCorreo(string correo)
        {
            string codigoVerificacion = GenerarCodigoVerificacion(); // Genera el código de verificación

            string remitente = "SekyhSoftware@hotmail.com"; // Cambia esto al correo que hayas creado
            string destinatario = correo; // Cambia esto al correo del usuario

            EnviarCorreo(remitente, destinatario, "Código de verificación", codigoVerificacion);
            return codigoVerificacion;
        }
        private static string GenerarCodigoVerificacion()
        {
            string codigo = "";
            Random NumeroAleatorio = new Random();
            return codigo = NumeroAleatorio.Next(1000, 9999).ToString();

        }

        private static void EnviarCorreo(string remitente, string destinatario, string asunto, string mensaje)
        {
            try
            {
                // Configura el cliente SMTP para Hotmail
                SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(remitente, "Fernando#23"); // Cambia esto a tu contraseña de Hotmail

                // Crea el mensaje de correo electrónico
                MailMessage emailMessage = new MailMessage();
                emailMessage.From = new MailAddress(remitente);
                emailMessage.To.Add(new MailAddress(destinatario));
                emailMessage.Subject = asunto;
                emailMessage.Body = mensaje;

                // Envía el mensaje
                client.Send(emailMessage);

                Console.WriteLine("El correo se ha enviado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
        public static string AuthCodigoDeVerificacion(string usuario,string codigoVerificacion)
        {
            Datos.Conectar();
            string respuesta = "error";
            string query = "ActivarUsuarioVerificado @usuario,@codigoVerificacion";
            SqlCommand cmd = new SqlCommand(query,Datos.conx);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@codigoVerificacion",codigoVerificacion);
            SqlDataReader dr;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if(dr["codigoVerificacion"].ToString() == codigoVerificacion)
                    {
                        Datos.Desconectar();
                        respuesta = "ok";
                        return respuesta;
                    }
                }
            }
            catch (Exception)
            {
                Datos.Desconectar();
                throw;
            }
            Datos.Desconectar();
            return respuesta;
        }
        #endregion

    }

    public class Administrador : Usuario
    {
        #region Atributos
        private bool admin;
        #endregion

        #region Propiedades
        public bool Admin
        {
            get { return admin; }
            set { admin = value; }
        }
        #endregion

        #region Metodos de la Clase
        public static Usuario LoguearAdmin(string email, string pwd)
        {
            Datos.Conectar();
            Administrador admin = new Administrador();
            string query = "SELECT * FROM usuario WHERE email = @email and pwd = @pwd and [Admin] = 's'";
            SqlCommand cmd = new SqlCommand(query, Datos.conx);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@Pwd", pwd);
            SqlDataReader dr;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    admin.Id = int.Parse(dr["id"].ToString());
                    admin.Nombre = dr["nombreCompleto"].ToString();
                    admin.NombreUsuario = dr["nombreUsuario"].ToString();
                    admin.Email = dr["email"].ToString();
                    admin.Pwd = dr["pwd"].ToString();
                    admin.Telefono = dr["telefono"].ToString();
                    admin.Token = dr["token"].ToString();
                    admin.Admin = dr["admin"].ToString() == "s";
                }
            }
            catch (Exception)
            {
                Datos.Desconectar();
                throw;
            }
            Datos.Desconectar();
            return admin;
        }

        public static Usuario AutentificarAdministrador(string token)
        {
            Datos.Conectar();
            Administrador admin = new Administrador();
            string query = "SELECT * FROM usuario WHERE token = @token and [Admin] = 's'";
            SqlCommand cmd = new SqlCommand(query, Datos.conx);
            cmd.Parameters.AddWithValue("@token", token);
            SqlDataReader dr;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    admin.Id = int.Parse(dr["id"].ToString());
                    admin.Nombre = dr["nombreCompleto"].ToString();
                    admin.NombreUsuario = dr["nombreUsuario"].ToString();
                    admin.Email = dr["email"].ToString();
                    admin.Pwd = dr["pwd"].ToString();
                    admin.Telefono = dr["telefono"].ToString();
                    admin.Token = dr["token"].ToString();
                    admin.Admin = dr["admin"].ToString() == "s";
                }
            }
            catch (Exception)
            {
                Datos.Desconectar();
                throw;
            }
            Datos.Desconectar();
            return admin;
        }
        #endregion

    }
}

