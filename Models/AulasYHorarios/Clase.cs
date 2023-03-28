using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanificacionAulas.Models.AulasYHorarios
{
    [Table("clases", Schema = "dbo")]
    public partial class Clase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClaseId { get; set; }

        [Required]
        public TimeSpan Inicio { get; set; }

        [Required]
        public TimeSpan Fin { get; set; }

        [Required]
        public int MateriatblMateriaId { get; set; }

        public int? EspacioId { get; set; }

        public Espacio Espacio { get; set; }

        public TblMateria TblMateria { get; set; }

    }
}