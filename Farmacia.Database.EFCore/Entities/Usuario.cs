using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cepdi_Prueba.Models
{
    [Table("usuarios", Schema = "dbo")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }  // Clave primaria, autoincrementable.

        [StringLength(200)]
        public string Nombre { get; set; }  // Nombre del usuario, con restricción de longitud.

        public DateTime? FechaCreacion { get; set; }  // Fecha opcional de creación del usuario.

        [StringLength(100)]
        [Column("usuario")]
        public string? Email { get; set; }  // Email del usuario, opcional y con nombre de columna personalizado.

        [StringLength(100)]
        public string Password { get; set; }  // Contraseña del usuario, con restricción de longitud.

        public int? IdPerfil { get; set; }  // ID opcional de un perfil asociado, representa una relación.

        public int? Estatus { get; set; }  // Estado del usuario, opcional.
    }
}