using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanificacionAulas.Models.AulasYHorarios
{
    [Table("tblMaterias", Schema = "dbo")]
    public partial class TblMateria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tblMateriaId { get; set; }

        [Required]
        public string CodMateria { get; set; }

        [Required]
        public string CodCarrera { get; set; }

        [Required]
        public short Plan { get; set; }

        public string Abreviatura { get; set; }

        public string Nombre { get; set; }

        public string AñoDeCursado { get; set; }

        public string Electiva { get; set; }

        public ICollection<Clase> Clases { get; set; }

        public ICollection<TblMatytallere> TblMatytalleres { get; set; }

    }
}