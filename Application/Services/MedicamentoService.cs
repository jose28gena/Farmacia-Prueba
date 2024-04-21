using Cepdi_Prueba.Models;
using Cepdi_Prueba.VModels;
using Farmacia.Database.EFCore;
using Farmacia.Database.EFCore.Response;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;

namespace WebApi.Services
{
    public interface IMedicamentoService
    {
        List<Medicamento> ObtenerMedicamentos();
        Task<List<v_medicamentos>> ObtenerVMedicamentos();
        Task<Medicamento> ObtenerMedicamentoPorId(int id);
        Task<Medicamento> CrearMedicamento(Medicamento medicamento);
        Task<Medicamento> ActualizarMedicamento(int id, Medicamento medicamento);
        Task<bool> EliminarMedicamento(int id);
        Task<ResultPage> ObtenerMedicamentosPaginados(Paginacion paginacion);
    }
    public class MedicamentoService : IMedicamentoService
    {
        private readonly FarmaciaContext _dbContext;

        public MedicamentoService(FarmaciaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Medicamento> ObtenerMedicamentos()
        {
            var medicamentos = _dbContext.Medicamentos.Where(x => x.Bhabilitado == 1).ToList();
            return medicamentos;
        }

        public async Task<List<v_medicamentos>> ObtenerVMedicamentos()
        {
            var vMedicamentos = await _dbContext.GetVMedicamentos();
            return vMedicamentos;
        }

        public async Task<Medicamento> ObtenerMedicamentoPorId(int id)
        {
            var medicamento = await _dbContext.Medicamentos.FindAsync(id);
            return medicamento;
        }

        public async Task<Medicamento> CrearMedicamento(Medicamento medicamento)
        {

            if (medicamento == null) throw new ArgumentNullException(nameof(medicamento));
            if (medicamento.IdFormaFarmaceutica == 0) throw new ArgumentException("Forma Farmacéutica inválida.");

            try
            {
               var FormaFarmaceutica =  _dbContext.FormaFarmaceuticas.Find(medicamento.IdFormaFarmaceutica);
                if (FormaFarmaceutica == null) throw new InvalidOperationException("Forma Farmacéutica no encontrada.");

                medicamento.Bhabilitado = 1;
                medicamento.FormaFarmaceutica = FormaFarmaceutica;
                _dbContext.Medicamentos.Add(medicamento);
                _dbContext.SaveChanges();

                return medicamento;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your error policy
                throw new InvalidOperationException("Error al crear el medicamento.", ex);
            }
        }

        public async Task<Medicamento> ActualizarMedicamento(int id, Medicamento medicamento)
        {
            var medicamentoExistente = await _dbContext.Medicamentos.FindAsync(id);
            if (medicamentoExistente == null)
            {
                return null;
            }

            medicamentoExistente.Nombre = medicamento.Nombre;
            medicamentoExistente.Precio = medicamento.Precio;
            medicamentoExistente.Stock = medicamento.Stock;
            medicamentoExistente.Concentracion = medicamento.Concentracion;
            medicamentoExistente.Presentacion = medicamento.Presentacion;
            medicamentoExistente.IdFormaFarmaceutica = medicamento.IdFormaFarmaceutica;


            await _dbContext.SaveChangesAsync();
            return medicamentoExistente;
        }

        public async Task<bool> EliminarMedicamento(int id)
        {
            var medicamento = await _dbContext.Medicamentos.FindAsync(id);
            if (medicamento == null)
            {
                return false;
            }

            _dbContext.Medicamentos.Remove(medicamento);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ResultPage> ObtenerMedicamentosPaginados(Paginacion paginacion)
        {
            // Inicia la consulta en la base de datos sin cargar los datos en memoria
            var query = _dbContext.VMedicamentos.AsQueryable();

            // Aplicar el filtro si existe
            if (!string.IsNullOrEmpty(paginacion.Filtro))
            {
                var filtro = $"%{paginacion.Filtro}%";
                query = query.Where(u => EF.Functions.Like(u.Nombre, filtro) || EF.Functions.Like(u.Presentacion, filtro)
                || EF.Functions.Like(u.FormasFarmaceuticas, filtro)
                || EF.Functions.Like(u.Precio.ToString(), filtro)
                || EF.Functions.Like(u.Stock.ToString(), filtro)
                || EF.Functions.Like(u.Concentracion, filtro)

                );
            }

            // Calcular el número total de registros y páginas
            var totalRegistros = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)paginacion.RegistrosPorPagina);

            // Validar el número de página
            if (paginacion.Pagina < 1 || paginacion.Pagina > totalPaginas)
            {
                // Retorna una lista vacía en lugar de lanzar una excepción
                return new ResultPage(totalRegistros, totalPaginas, paginacion.Pagina, paginacion.RegistrosPorPagina, new List<v_medicamentos>());
            }

            // Calcular la cantidad de registros a omitir y obtener la página solicitada
            var registrosSaltados = (paginacion.Pagina - 1) * paginacion.RegistrosPorPagina;
            var medicamentos = await query.Skip(registrosSaltados).Take(paginacion.RegistrosPorPagina).ToListAsync();

            return new ResultPage(totalRegistros, totalPaginas, paginacion.Pagina, paginacion.RegistrosPorPagina, medicamentos);
        }
    }
}
