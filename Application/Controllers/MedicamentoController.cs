using Cepdi_Prueba.Models;
using Cepdi_Prueba.VModels;
using Farmacia.Database.EFCore.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MedicamentoController : ControllerBase
    {
        private readonly IMedicamentoService _medicamentoService;

        public MedicamentoController(IMedicamentoService medicamentoService)
        {
            _medicamentoService = medicamentoService;
        }

        [HttpGet]
        public List<Medicamento> Get()
        {
            return _medicamentoService.ObtenerMedicamentos();
        }

        [HttpGet("vmedicamentos")]
        public async Task<List<v_medicamentos>> GetVMedicamentos()
        {
            return await _medicamentoService.ObtenerVMedicamentos();
        }

        [HttpGet("{id}")]
        public async Task<Medicamento> Get(int id)
        {
            return await _medicamentoService.ObtenerMedicamentoPorId(id);
        }

        [HttpPost]
        public IActionResult CrearMedicamento([FromBody] Medicamento medicamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _medicamentoService.CrearMedicamento(medicamento);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Medicamento medicamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _medicamentoService.ActualizarMedicamento(id, medicamento);
            return Ok(medicamento);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _medicamentoService.EliminarMedicamento(id);
        }
        [HttpPost("ObtenerMedicamentosPaginados")]
        public async Task<ResultPage> ObtenerMedicamentosPaginados([FromBody] Paginacion paginacion)
        {
            return await _medicamentoService.ObtenerMedicamentosPaginados(paginacion);
        }
    }
}
