using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanificacionAulas.Models.AulasYHorarios
{
    [Table("espacio", Schema = "dbo")]
    public partial class Espacio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EspacioId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int capacidad { get; set; }

        public ICollection<Clase> Clases { get; set; }

    }
}