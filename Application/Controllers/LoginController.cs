using Microsoft.AspNetCore.Mvc;
using WebApi.Response;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    { 
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] CredencialesUsuario request)
        {
            var isSuccess = _usuarioService.IniciarSesion(request.Usuario, request.Password);
            if (isSuccess)
            {
                var token = await _usuarioService.ConstruirToken(new CredencialesUsuario
                {
                    Usuario = request.Usuario,
                    Password = request.Password
                });
                return Ok(token);
            }
            else
            {
                return Unauthorized("Error al inciar sesión.");
            }
        }

    }
}
