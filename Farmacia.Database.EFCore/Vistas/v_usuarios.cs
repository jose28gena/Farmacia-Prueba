using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cepdi_Prueba.Models
{
    [Table("v_usuarios", Schema = "dbo")]
    public class v_usuarios
    {
        [Key]
        public int IdUsuario { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [StringLength(100)]
        [Column("usuario")]
        public string Email { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        public int? IdPerfil { get; set; }

        [StringLength(50)]
        public string Estatus { get; set; }

        public int? IdEstatus { get; set; }
    }
}