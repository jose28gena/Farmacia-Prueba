using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cepdi_Prueba.VModels
{
    [Table("v_medicamentos", Schema = "dbo")]
    public class v_medicamentos
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Concentracion { get; set; }

        public int? IdFormaFarmaceutica { get; set; }

        [StringLength(100)]
        public string FormasFarmaceuticas { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }

        public int? Stock { get; set; }

        [StringLength(100)]
        public string Presentacion { get; set; }

        [StringLength(50)]
        public string Estatus { get; set; }

        public int? IdEstatus { get; set; }
    }
}