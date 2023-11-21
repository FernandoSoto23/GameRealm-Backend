namespace GameRealm.Models
{
    public class RespuestaJson<Type>
    {
        public bool Status { get; set; }
        public string Msg { get; set; }
        public Type Dato { get; set; }

    }
}
