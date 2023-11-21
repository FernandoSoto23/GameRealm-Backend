using System.Data.SqlClient;


namespace GameRealm.Models
{
    public class Datos
    {
        static public SqlConnection conx = new SqlConnection();

        public static void Conectar()
        {
            conx.ConnectionString = "server=localhost;database=gameRealDTB;integrated security = yes;";
            conx.Open();
        }

        public static void Desconectar()
        {
            if(conx != null && conx.State == System.Data.ConnectionState.Open)
                conx.Close();
        }
    }
}
