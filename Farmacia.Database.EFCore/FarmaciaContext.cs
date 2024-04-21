using Cepdi_Prueba.Models;
using Cepdi_Prueba.VModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Farmacia.Database.EFCore
{
    public class FarmaciaContext : DbContext
    {
        public FarmaciaContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Medicamento> Medicamentos { get; set; }
        public virtual DbSet<FormaFarmaceutica> FormaFarmaceuticas { get; set; }
        public DbSet<v_usuarios> VUsuarios { get; set; }
        public DbSet<v_medicamentos> VMedicamentos { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("default");
            }
        }
        public FarmaciaContext() { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<v_usuarios>(entity =>
            {
                entity.HasNoKey(); // Since views do not have primary key
                entity.ToView("v_usuarios"); // Make sure this is the correct view name
            });
            builder.Entity<v_medicamentos>(entity =>
            {
                entity.HasNoKey(); // Since views do not have primary key
                entity.ToView("v_medicamentos"); // Make sure this is the correct view name
            });
            base.OnModelCreating(builder);
        }

        public async Task<List<v_usuarios>> GetVUsuarios()
        {
            // Llamar a la función y devolver los resultados
            return await this.Set<v_usuarios>().FromSqlRaw("SELECT * FROM \"dbo\".\"v_usuarios\"").ToListAsync();
        }
        public async Task<List<v_medicamentos>> GetVMedicamentos()
        {
            // Llamar a la función y devolver los resultados
            return await this.Set<v_medicamentos>().FromSqlRaw("SELECT * FROM \"dbo\".\"v_medicamentos\"").ToListAsync();
        }
    }
}
