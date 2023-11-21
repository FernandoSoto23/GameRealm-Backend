using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace GameRealm.Models
{
    public class Titulo
    {

        #region Atributos
        private int codigo;
        private string nombre;
        private string imagen;
        private double precio;
        private string descripcion;
        private int categoria;
        #endregion

        #region Propiedades

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
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
        public double Precio
        {
            get { return precio; }
            set { if (Precio > -1) precio = value; }
        }
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        public int Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }


        #endregion

        #region Metodos
        public static ListaTitulo ListarTitulos()
        {
            Datos.Conectar();
            string Cadena = $"SELECT * FROM Titulo";
            SqlCommand cmd = new SqlCommand(Cadena, Datos.conx);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            ListaTitulo lista = new ListaTitulo();
            try
            {
                while (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        Titulo titulo = new Titulo();
                        titulo.Codigo = int.Parse(dr["codigo"].ToString());
                        titulo.Nombre = dr["Nombre"].ToString() ?? "vacio";
                        titulo.Imagen = dr["imagen"].ToString() ?? "vacio";
                        titulo.Precio = double.Parse(dr["precio"].ToString() ?? "0");
                        titulo.Descripcion = dr["descripcion"].ToString() ?? "vacio";
                        titulo.Categoria = int.Parse(dr["Categoria"].ToString() ?? "0");

                        Console.WriteLine(titulo);

                        lista.Add(titulo);
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
        public static ListaTitulo ListarTituloPorCategoria(int Categoria)
        {
            Datos.Conectar();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Titulo WHERE Categoria = @Categoria", Datos.conx);
            cmd.Parameters.AddWithValue("@Categoria", Categoria);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            ListaTitulo lista = new ListaTitulo();

            while (dr.HasRows)
            {
                if (dr.Read())
                {
                    Titulo titulo = new Titulo();
                    titulo.Codigo = int.Parse(dr["codigo"].ToString());
                    titulo.Nombre = dr["Nombre"].ToString();
                    titulo.Imagen = dr["imagen"].ToString();
                    titulo.Precio = double.Parse(dr["precio"].ToString());
                    titulo.Descripcion = dr["descripcion"].ToString();
                    titulo.Categoria = int.Parse(dr["categoria"].ToString());


                    lista.Add(titulo);
                }
                else
                {
                    dr.NextResult();
                }
            }

            Datos.Desconectar();
            return lista;
        }
        public static Titulo Orden(int codigo)
        {
            Datos.Conectar();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Titulo WHERE codigo = @codigo", Datos.conx);
            cmd.Parameters.AddWithValue("@codigo", codigo);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            ListaTitulo lista = new ListaTitulo();

            Titulo orden = new Titulo();
            if (dr.Read())
            {

                orden.Codigo = int.Parse(dr["codigo"].ToString());
                orden.Nombre = dr["nombre"].ToString() ?? "";
                orden.Imagen = dr["imagen"].ToString() ?? "";
                orden.Precio = double.Parse(dr["precio"].ToString() ?? "0");
                orden.Descripcion = dr["descripcion"].ToString() ?? "";
                orden.Categoria = int.Parse(dr["categoria"].ToString() ?? "0");



            }

            Datos.Desconectar();
            return orden;
        }
        public static bool CrearNuevoAnuncio(Titulo entidad,int id,string token)
        {
            Datos.Conectar();
            string cadena = "spAddTitulo @id,@token,@Nombre,@imagen,@precio,@descripcion,@Categoria";
            SqlCommand cmd = new SqlCommand(cadena, Datos.conx);
            cmd.Parameters.AddWithValue("@id",id);
            cmd.Parameters.AddWithValue("@token",token);
            cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
            cmd.Parameters.AddWithValue("@imagen", entidad.Imagen);
            cmd.Parameters.AddWithValue("@precio", entidad.Precio);
            cmd.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
            cmd.Parameters.AddWithValue("@Categoria", entidad.Categoria);

            try
            {
                cmd.ExecuteNonQuery();
                Datos.Desconectar();
                return true;
            }
            catch (Exception error)
            {
               
                Console.WriteLine(error);
                Datos.Desconectar();
                return false;
            }



        }
        public static bool Actualizar(Titulo entidad,int codigo,int id,string token)
        {
            Datos.Conectar();
            string cadena = "spUpdateTitulo @id,@token,@codigo,@nombre,@imagen,@precio,@descripcion,@Categoria";

            SqlCommand cmd = new SqlCommand(cadena, Datos.conx);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@codigo", codigo);
            cmd.Parameters.AddWithValue("@nombre", entidad.Nombre);
            cmd.Parameters.AddWithValue("@imagen", entidad.Imagen);
            cmd.Parameters.AddWithValue("@precio", entidad.Precio);
            cmd.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
            cmd.Parameters.AddWithValue("@Categoria", entidad.Categoria);

            try
            {
                cmd.ExecuteNonQuery();
                Datos.Desconectar();
                return true;
            }
            catch (Exception error)
            {

                Console.WriteLine(error);
                Datos.Desconectar();
                return false;
            }



        }
        #endregion


    }

    public class TituloAdministrador<Type>
    {
        #region Atributos   
        private int id;
        private string token;
        private Type titulo;
        #endregion

        #region Propiedades
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        public Type Titulo
        {
            get { return titulo; }
            set { titulo = value; }
        }
        #endregion

        #region Metodos
        
        #endregion


    }
    public class ListaTitulo : List<Titulo>
    {

    }
}
