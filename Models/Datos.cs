using System.Data.SqlClient;


namespace ServicioRestaurante.Models
{
    public class Datos
    {
        static public SqlConnection conx = new SqlConnection();

        public static void Conectar()
        {
            conx.ConnectionString = "server=localhost;database=SekyhRestaurante;user id = sa;password=1234;";
            conx.Open();
        }

        public static void Desconectar()
        {
            if(conx != null && conx.State == System.Data.ConnectionState.Open)
                conx.Close();
        }
    }
}
