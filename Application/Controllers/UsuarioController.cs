using Cepdi_Prueba.Models;
using Farmacia.Database.EFCore.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Response;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public List<Usuario> Get()
        {
            return _usuarioService.ObtenerUsuarios();
        }

        [HttpGet("{id}")]
        public async Task<Usuario> Get(int id)
        {
            return await _usuarioService.ObtenerUsuarioPorId(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            usuario.FechaCreacion = DateTime.Now;
            var createdUsuario = await _usuarioService.CrearUsuario(usuario);
            return Ok(createdUsuario);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _usuarioService.ActualizarUsuario(id, usuario);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _usuarioService.EliminarUsuario(id);
        }

        [HttpPost("ObtenerUsuariosPaginados")]
        public async Task<ResultPage> ObtenerUsuariosPaginados([FromBody] Paginacion paginacion)
        {
            return await _usuarioService.ObtenerUsuariosPaginados(paginacion);
        }


    }
}
