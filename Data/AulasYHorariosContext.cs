using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using PlanificacionAulas.Models.AulasYHorarios;

namespace PlanificacionAulas.Data
{
    public partial class AulasYHorariosContext : DbContext
    {
        public AulasYHorariosContext()
        {
        }

        public AulasYHorariosContext(DbContextOptions<AulasYHorariosContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PlanificacionAulas.Models.AulasYHorarios.Clase>()
              .HasOne(i => i.Espacio)
              .WithMany(i => i.Clases)
              .HasForeignKey(i => i.EspacioId)
              .HasPrincipalKey(i => i.EspacioId);

            builder.Entity<PlanificacionAulas.Models.AulasYHorarios.Clase>()
              .HasOne(i => i.TblMateria)
              .WithMany(i => i.Clases)
              .HasForeignKey(i => i.MateriatblMateriaId)
              .HasPrincipalKey(i => i.tblMateriaId);

            builder.Entity<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere>()
              .HasOne(i => i.TblMateria)
              .WithMany(i => i.TblMatytalleres)
              .HasForeignKey(i => i.tblMateriaId)
              .HasPrincipalKey(i => i.tblMateriaId);
            this.OnModelBuilding(builder);
        }

        public DbSet<PlanificacionAulas.Models.AulasYHorarios.Clase> Clases { get; set; }

        public DbSet<PlanificacionAulas.Models.AulasYHorarios.Espacio> Espacios { get; set; }

        public DbSet<PlanificacionAulas.Models.AulasYHorarios.TblMateria> TblMateria { get; set; }

        public DbSet<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> TblMatytalleres { get; set; }
    }
}