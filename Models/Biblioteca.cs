using System.Data.SqlClient;

namespace GameRealm.Models
{
    public class Biblioteca
    {
        private int id;
        private int idUsuario;
        private string codigoTitulo;
        private string tituloKey;
        private string nombre;
        private string imagen;
        private string plataforma;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public string CodigoTitulo
        {
            get { return codigoTitulo; }
            set { codigoTitulo = value; }
        }

        public string TituloKey
        {
            get { return tituloKey; }
            set { tituloKey = value; }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        public string Plataforma
        {
            get { return plataforma; }
            set { plataforma = value; }
        }
        public static ListaBiblioteca ListarBibliotecas(int id)
        {
            Datos.Conectar();
            string Cadena = $"SELECT b.id,b.idUsuario,b.codigoTitulo,T.Nombre,T.imagen,T.plataforma,b.TituloKey \r\nFROM Biblioteca b \r\nJOIN Titulo T\r\nOn b.CodigoTitulo = T.Codigo\r\nWHERE idUsuario = @id";
            SqlCommand cmd = new SqlCommand(Cadena, Datos.conx);
            cmd.Parameters.AddWithValue("@id",id);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            ListaBiblioteca lista = new ListaBiblioteca();
            try
            {
                while (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        Biblioteca biblioteca = new Biblioteca();
                        biblioteca.Id = int.Parse(dr["id"].ToString());
                        biblioteca.IdUsuario = int.Parse(dr["idUsuario"].ToString() ?? "vacio");
                        biblioteca.CodigoTitulo = dr["codigoTitulo"].ToString() ?? "vacio";
                        biblioteca.TituloKey = (dr["TituloKey"].ToString() ?? "0");
                        biblioteca.Nombre = dr["nombre"].ToString() ?? "vacio";
                        biblioteca.Plataforma = dr["plataforma"].ToString() ?? "vacio";
                        biblioteca.imagen = dr["imagen"].ToString() ?? "vacio";
                        lista.Add(biblioteca);
                    }
                    else
                    {
                        dr.NextResult();
                    }

                }
            }
            catch (Exception e)
            {
                Datos.Desconectar();
                throw e;
            }

            Datos.Desconectar();
            return lista;
        }
    }

    public class ListaBiblioteca : List<Biblioteca>
    {

    }
}
