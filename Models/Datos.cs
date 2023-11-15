using System.Data.SqlClient;


namespace ServicioRestaurante.Models
{
    public class Datos
    {
        static public SqlConnection conx = new SqlConnection();

        public static void Conectar()
        {
            conx.ConnectionString = "server=SKYNEXTG\\SQLEXPRESS;database=gameRealDTB;integrated security = yes;";
            conx.Open();
        }

        public static void Desconectar()
        {
            if(conx != null && conx.State == System.Data.ConnectionState.Open)
                conx.Close();
        }
    }
}
