using Cepdi_Prueba.Models;
using Farmacia.Database.EFCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface IFormasFarmaceuticasService
    {
        List<FormaFarmaceutica> ObtenerFormasFarmaceuticas();
        Task<List<FormaFarmaceutica>> ObtenerFormasFarmaceuticasAsync();
        Task<FormaFarmaceutica> ObtenerFormaFarmaceuticaPorId(int id);
        Task<FormaFarmaceutica> CrearFormaFarmaceutica(FormaFarmaceutica formaFarmaceutica);
     

    }
    public class FormasFarmaceuticasService : IFormasFarmaceuticasService
    {
        private readonly FarmaciaContext _dbContext;

        public FormasFarmaceuticasService(FarmaciaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<FormaFarmaceutica> ObtenerFormasFarmaceuticas()
        {
            var formasFarmaceuticas = _dbContext.FormaFarmaceuticas.Where(x => x.Habilitado == 1).ToList();
            return formasFarmaceuticas;
        }

        public async Task<List<FormaFarmaceutica>> ObtenerFormasFarmaceuticasAsync()
        {
            var formasFarmaceuticas = await _dbContext.FormaFarmaceuticas.ToListAsync();
            return formasFarmaceuticas;
        }

        public async Task<FormaFarmaceutica> ObtenerFormaFarmaceuticaPorId(int id)
        {
            var formaFarmaceutica = await _dbContext.FormaFarmaceuticas.FindAsync(id);
            return formaFarmaceutica;
        }

        public async Task<FormaFarmaceutica> CrearFormaFarmaceutica(FormaFarmaceutica formaFarmaceutica)
        {
            _dbContext.FormaFarmaceuticas.Add(formaFarmaceutica);
            await _dbContext.SaveChangesAsync();
            return formaFarmaceutica;
        }

    
    }
}
