using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cepdi_Prueba.Models
{
    [Table("formasfarmaceuticas", Schema = "dbo")]
    public class FormaFarmaceutica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFormaFarmaceutica { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        public int? Habilitado { get; set; }
    }
}