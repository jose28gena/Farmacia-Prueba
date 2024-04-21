using System.ComponentModel.DataAnnotations;

namespace WebApi.Response
{
    public class RespuestaAutenticacion
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }

    public class ResponseAutenticacion
    {
        public bool isLogIn { get; set; }
        public CredencialesUsuario credenciales { get; set; }
    }
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Usuario { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
