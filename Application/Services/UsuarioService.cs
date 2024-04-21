using Cepdi_Prueba.Models;
using Farmacia.Database.EFCore;
using Farmacia.Database.EFCore.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using WebApi.Controllers;
using WebApi.Response;
namespace WebApi.Services
{
    public interface IUsuarioService
    {
        bool IniciarSesion(string username, string password);
        List<Usuario> ObtenerUsuarios();
        Task<List<v_usuarios>> ObtenerVUsuarios(); // Added Task<> and async modifier
        bool ValidarContrasena(string contrasena); // Added method declaration
        Task<Usuario> ObtenerUsuarioPorId(int id);
        Task<Usuario> CrearUsuario(Usuario usuario);
        Task<Usuario> ActualizarUsuario(int id, Usuario usuario);
        Task<bool> EliminarUsuario(int id);
        Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario);
        Task<ResultPage> ObtenerUsuariosPaginados(Paginacion paginacion);

    }
    public class UsuarioService : IUsuarioService
    {
        private readonly FarmaciaContext _dbContext;
        public IConfiguration Configuration { get; set; }
        public UsuarioService(FarmaciaContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            Configuration = configuration;
        }

        public bool IniciarSesion(string username, string password)
        {
            // Retrieve the user from the database based on the username
            var user = _dbContext.Usuarios.FirstOrDefault(u => u.Email == username);

            // Check if the user exists and the password matches
            if (user != null && user.Password == password)
            {
                // User is authenticated
                return true;
            }

            // User is not authenticated
            return false;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            // Retrieve all users from the database
            var usuarios = _dbContext.Usuarios.ToList();

            return usuarios;
        }

        public async Task<List<v_usuarios>> ObtenerVUsuarios() // Added async modifier
        {
            // Retrieve all users from the database
            var vUsuarios = await _dbContext.GetVUsuarios();

            return vUsuarios;
        }

        public bool ValidarContrasena(string contrasena)
        {
            // Validate password requirements
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
            return regex.IsMatch(contrasena);
        }
        public async Task<Usuario> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _dbContext.Usuarios.FindAsync(id);
            return usuario;
        }
        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();
            return usuario;
        }
        public async Task<Usuario> ActualizarUsuario(int id, Usuario usuario)
        {
            var usuarioExistente = await _dbContext.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return null;
            }

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Password = usuario.Password;
            usuarioExistente.IdPerfil = usuario.IdPerfil;
            usuarioExistente.Estatus = usuario.Estatus;


            await _dbContext.SaveChangesAsync();
            return usuarioExistente;
        }
        public async Task<bool> EliminarUsuario(int id)
        {
            var usuario = await _dbContext.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }

            _dbContext.Usuarios.Remove(usuario);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private async Task<Usuario> obtenerUsuarioByUserName(string username)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == username);

        }
        public async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            // Retrieve the user from the database based on the username
            var user = await obtenerUsuarioByUserName(credencialesUsuario.Usuario);

            // Check if the user exists and the password matches
            if (user != null && user.Password == credencialesUsuario.Password)
            {
                // Create claims for the user
                var claims = new List<Claim>()
                    {
                        new Claim("Usuario", credencialesUsuario.Usuario),
                    };

                // Get the secret key from the configuration


                var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                // Create the signing credentials
                var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

                // Set the expiration date for the token
                var expiracion = DateTime.UtcNow.AddDays(7);

                // Create the security token
                var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                    expires: expiracion, signingCredentials: creds);

                // Return the authentication response
                return new RespuestaAutenticacion()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Expiracion = expiracion
                };
            }

            // User is not authenticated
            return null;
        }

        public async Task<ResultPage> ObtenerUsuariosPaginados(Paginacion paginacion)
        {
            // Inicia la consulta en la base de datos sin cargar los datos en memoria
            var query = _dbContext.VUsuarios.AsQueryable();

            // Aplicar el filtro si existe
            if (!string.IsNullOrEmpty(paginacion.Filtro))
            {
                var filtro = $"%{paginacion.Filtro}%";
                query = query.Where(u => EF.Functions.Like(u.Nombre, filtro) || EF.Functions.Like(u.Email, filtro));
            }

            // Calcular el número total de registros y páginas
            var totalRegistros = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)paginacion.RegistrosPorPagina);

            // Validar el número de página
            if (paginacion.Pagina < 1 || paginacion.Pagina > totalPaginas)
            {
                // Retorna una lista vacía en lugar de lanzar una excepción
                return new ResultPage(totalRegistros, totalPaginas, paginacion.Pagina, paginacion.RegistrosPorPagina, new List<v_usuarios>());
            }

            // Calcular la cantidad de registros a omitir y obtener la página solicitada
            var registrosSaltados = (paginacion.Pagina - 1) * paginacion.RegistrosPorPagina;
            var usuarios = await query.Skip(registrosSaltados).Take(paginacion.RegistrosPorPagina).ToListAsync();

            return new ResultPage(totalRegistros, totalPaginas, paginacion.Pagina, paginacion.RegistrosPorPagina, usuarios);
        }

    }
}
