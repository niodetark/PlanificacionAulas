using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanificacionAulas.Models.AulasYHorarios
{
    [Table("tblMatytalleres", Schema = "dbo")]
    public partial class TblMatytallere
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMatyTaller { get; set; }

        [Required]
        public string CodTaller { get; set; }

        [Required]
        public string CodMateria { get; set; }

        [Required]
        public string CodCarrera { get; set; }

        [Required]
        public short Plan { get; set; }

        public string NombMateria { get; set; }

        public float? CantAlumnos { get; set; }

        public int? LegajoProfaCargo { get; set; }

        public float? CargosProfeso { get; set; }

        public string Resolución { get; set; }

        public string Año { get; set; }

        public byte? IdArea { get; set; }

        public string Nombre1 { get; set; }

        [Required]
        public int tblMateriaId { get; set; }

        public TblMateria TblMateria { get; set; }

    }
}