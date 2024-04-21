using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cepdi_Prueba.Models
{
    [Table("medicamentos", Schema = "dbo")]
    public class Medicamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMedicamento { get; set; }

        [Required(ErrorMessage = "El nombre del medicamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "La longitud del nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(100, ErrorMessage = "La longitud de la concentración no puede exceder los 100 caracteres.")]
        public string Concentracion { get; set; }

        [Required(ErrorMessage = "El ID de la forma farmacéutica es obligatorio.")]
        public int? IdFormaFarmaceutica { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, 100000, ErrorMessage = "El precio debe estar entre 0 y 100,000.")]
        public decimal? Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int? Stock { get; set; }

        [StringLength(100, ErrorMessage = "La longitud de la presentación no puede exceder los 100 caracteres.")]
        public string Presentacion { get; set; }
        public int? Bhabilitado { get; set; }
        [ForeignKey("IdFormaFarmaceutica")]
        public virtual FormaFarmaceutica FormaFarmaceutica { get; set; }
    }
}