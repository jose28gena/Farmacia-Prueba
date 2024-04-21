using Cepdi_Prueba.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormaFarmaceuticaController : ControllerBase
    {
        private readonly IFormasFarmaceuticasService _formaFarmaceuticaService;

        public FormaFarmaceuticaController(IFormasFarmaceuticasService formaFarmaceuticaService)
        {
            _formaFarmaceuticaService = formaFarmaceuticaService;
        }

        [HttpGet]
        public List<FormaFarmaceutica> Get()
        {
            return _formaFarmaceuticaService.ObtenerFormasFarmaceuticas();
        }

        [HttpGet("{id}")]
        public async Task<FormaFarmaceutica> Get(int id)
        {
            return await _formaFarmaceuticaService.ObtenerFormaFarmaceuticaPorId(id);
        }

        [HttpPost]
        public async Task<FormaFarmaceutica> Post([FromBody] FormaFarmaceutica formaFarmaceutica)
        {
            return await _formaFarmaceuticaService.CrearFormaFarmaceutica(formaFarmaceutica);
        }

     
    }
}
