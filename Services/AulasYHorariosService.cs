using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using PlanificacionAulas.Data;

namespace PlanificacionAulas
{
    public partial class AulasYHorariosService
    {
        AulasYHorariosContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly AulasYHorariosContext context;
        private readonly NavigationManager navigationManager;

        public AulasYHorariosService(AulasYHorariosContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportClasesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/clases/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/clases/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportClasesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/clases/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/clases/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnClasesRead(ref IQueryable<PlanificacionAulas.Models.AulasYHorarios.Clase> items);

        public async Task<IQueryable<PlanificacionAulas.Models.AulasYHorarios.Clase>> GetClases(Query query = null)
        {
            var items = Context.Clases.AsQueryable();

            items = items.Include(i => i.Espacio);
            items = items.Include(i => i.TblMateria);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnClasesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnClaseGet(PlanificacionAulas.Models.AulasYHorarios.Clase item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Clase> GetClaseByClaseId(int claseid)
        {
            var items = Context.Clases
                              .AsNoTracking()
                              .Where(i => i.ClaseId == claseid);

            items = items.Include(i => i.Espacio);
            items = items.Include(i => i.TblMateria);
  
            var itemToReturn = items.FirstOrDefault();

            OnClaseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnClaseCreated(PlanificacionAulas.Models.AulasYHorarios.Clase item);
        partial void OnAfterClaseCreated(PlanificacionAulas.Models.AulasYHorarios.Clase item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Clase> CreateClase(PlanificacionAulas.Models.AulasYHorarios.Clase clase)
        {
            OnClaseCreated(clase);

            var existingItem = Context.Clases
                              .Where(i => i.ClaseId == clase.ClaseId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Clases.Add(clase);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(clase).State = EntityState.Detached;
                throw;
            }

            OnAfterClaseCreated(clase);

            return clase;
        }

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Clase> CancelClaseChanges(PlanificacionAulas.Models.AulasYHorarios.Clase item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnClaseUpdated(PlanificacionAulas.Models.AulasYHorarios.Clase item);
        partial void OnAfterClaseUpdated(PlanificacionAulas.Models.AulasYHorarios.Clase item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Clase> UpdateClase(int claseid, PlanificacionAulas.Models.AulasYHorarios.Clase clase)
        {
            OnClaseUpdated(clase);

            var itemToUpdate = Context.Clases
                              .Where(i => i.ClaseId == clase.ClaseId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(clase);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterClaseUpdated(clase);

            return clase;
        }

        partial void OnClaseDeleted(PlanificacionAulas.Models.AulasYHorarios.Clase item);
        partial void OnAfterClaseDeleted(PlanificacionAulas.Models.AulasYHorarios.Clase item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Clase> DeleteClase(int claseid)
        {
            var itemToDelete = Context.Clases
                              .Where(i => i.ClaseId == claseid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnClaseDeleted(itemToDelete);


            Context.Clases.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterClaseDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEspaciosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/espacios/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/espacios/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEspaciosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/espacios/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/espacios/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEspaciosRead(ref IQueryable<PlanificacionAulas.Models.AulasYHorarios.Espacio> items);

        public async Task<IQueryable<PlanificacionAulas.Models.AulasYHorarios.Espacio>> GetEspacios(Query query = null)
        {
            var items = Context.Espacios.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEspaciosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEspacioGet(PlanificacionAulas.Models.AulasYHorarios.Espacio item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Espacio> GetEspacioByEspacioId(int espacioid)
        {
            var items = Context.Espacios
                              .AsNoTracking()
                              .Where(i => i.EspacioId == espacioid);

  
            var itemToReturn = items.FirstOrDefault();

            OnEspacioGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEspacioCreated(PlanificacionAulas.Models.AulasYHorarios.Espacio item);
        partial void OnAfterEspacioCreated(PlanificacionAulas.Models.AulasYHorarios.Espacio item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Espacio> CreateEspacio(PlanificacionAulas.Models.AulasYHorarios.Espacio espacio)
        {
            OnEspacioCreated(espacio);

            var existingItem = Context.Espacios
                              .Where(i => i.EspacioId == espacio.EspacioId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Espacios.Add(espacio);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(espacio).State = EntityState.Detached;
                throw;
            }

            OnAfterEspacioCreated(espacio);

            return espacio;
        }

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Espacio> CancelEspacioChanges(PlanificacionAulas.Models.AulasYHorarios.Espacio item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEspacioUpdated(PlanificacionAulas.Models.AulasYHorarios.Espacio item);
        partial void OnAfterEspacioUpdated(PlanificacionAulas.Models.AulasYHorarios.Espacio item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Espacio> UpdateEspacio(int espacioid, PlanificacionAulas.Models.AulasYHorarios.Espacio espacio)
        {
            OnEspacioUpdated(espacio);

            var itemToUpdate = Context.Espacios
                              .Where(i => i.EspacioId == espacio.EspacioId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(espacio);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEspacioUpdated(espacio);

            return espacio;
        }

        partial void OnEspacioDeleted(PlanificacionAulas.Models.AulasYHorarios.Espacio item);
        partial void OnAfterEspacioDeleted(PlanificacionAulas.Models.AulasYHorarios.Espacio item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.Espacio> DeleteEspacio(int espacioid)
        {
            var itemToDelete = Context.Espacios
                              .Where(i => i.EspacioId == espacioid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEspacioDeleted(itemToDelete);


            Context.Espacios.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEspacioDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblMateriaToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/tblmateria/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/tblmateria/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblMateriaToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/tblmateria/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/tblmateria/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblMateriaRead(ref IQueryable<PlanificacionAulas.Models.AulasYHorarios.TblMateria> items);

        public async Task<IQueryable<PlanificacionAulas.Models.AulasYHorarios.TblMateria>> GetTblMateria(Query query = null)
        {
            var items = Context.TblMateria.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTblMateriaRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblMateriaGet(PlanificacionAulas.Models.AulasYHorarios.TblMateria item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMateria> GetTblMateriaByTblMateriaId(int tblmateriaid)
        {
            var items = Context.TblMateria
                              .AsNoTracking()
                              .Where(i => i.tblMateriaId == tblmateriaid);

  
            var itemToReturn = items.FirstOrDefault();

            OnTblMateriaGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblMateriaCreated(PlanificacionAulas.Models.AulasYHorarios.TblMateria item);
        partial void OnAfterTblMateriaCreated(PlanificacionAulas.Models.AulasYHorarios.TblMateria item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMateria> CreateTblMateria(PlanificacionAulas.Models.AulasYHorarios.TblMateria tblmateria)
        {
            OnTblMateriaCreated(tblmateria);

            var existingItem = Context.TblMateria
                              .Where(i => i.tblMateriaId == tblmateria.tblMateriaId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblMateria.Add(tblmateria);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblmateria).State = EntityState.Detached;
                throw;
            }

            OnAfterTblMateriaCreated(tblmateria);

            return tblmateria;
        }

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMateria> CancelTblMateriaChanges(PlanificacionAulas.Models.AulasYHorarios.TblMateria item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblMateriaUpdated(PlanificacionAulas.Models.AulasYHorarios.TblMateria item);
        partial void OnAfterTblMateriaUpdated(PlanificacionAulas.Models.AulasYHorarios.TblMateria item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMateria> UpdateTblMateria(int tblmateriaid, PlanificacionAulas.Models.AulasYHorarios.TblMateria tblmateria)
        {
            OnTblMateriaUpdated(tblmateria);

            var itemToUpdate = Context.TblMateria
                              .Where(i => i.tblMateriaId == tblmateria.tblMateriaId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblmateria);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblMateriaUpdated(tblmateria);

            return tblmateria;
        }

        partial void OnTblMateriaDeleted(PlanificacionAulas.Models.AulasYHorarios.TblMateria item);
        partial void OnAfterTblMateriaDeleted(PlanificacionAulas.Models.AulasYHorarios.TblMateria item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMateria> DeleteTblMateria(int tblmateriaid)
        {
            var itemToDelete = Context.TblMateria
                              .Where(i => i.tblMateriaId == tblmateriaid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblMateriaDeleted(itemToDelete);


            Context.TblMateria.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblMateriaDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTblMatytalleresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/tblmatytalleres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/tblmatytalleres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblMatytalleresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/aulasyhorarios/tblmatytalleres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/aulasyhorarios/tblmatytalleres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblMatytalleresRead(ref IQueryable<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> items);

        public async Task<IQueryable<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere>> GetTblMatytalleres(Query query = null)
        {
            var items = Context.TblMatytalleres.AsQueryable();

            items = items.Include(i => i.TblMateria);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTblMatytalleresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblMatytallereGet(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> GetTblMatytallereByIdMatyTaller(int idmatytaller)
        {
            var items = Context.TblMatytalleres
                              .AsNoTracking()
                              .Where(i => i.IdMatyTaller == idmatytaller);

            items = items.Include(i => i.TblMateria);
  
            var itemToReturn = items.FirstOrDefault();

            OnTblMatytallereGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTblMatytallereCreated(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item);
        partial void OnAfterTblMatytallereCreated(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> CreateTblMatytallere(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere tblmatytallere)
        {
            OnTblMatytallereCreated(tblmatytallere);

            var existingItem = Context.TblMatytalleres
                              .Where(i => i.IdMatyTaller == tblmatytallere.IdMatyTaller)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblMatytalleres.Add(tblmatytallere);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblmatytallere).State = EntityState.Detached;
                throw;
            }

            OnAfterTblMatytallereCreated(tblmatytallere);

            return tblmatytallere;
        }

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> CancelTblMatytallereChanges(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblMatytallereUpdated(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item);
        partial void OnAfterTblMatytallereUpdated(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> UpdateTblMatytallere(int idmatytaller, PlanificacionAulas.Models.AulasYHorarios.TblMatytallere tblmatytallere)
        {
            OnTblMatytallereUpdated(tblmatytallere);

            var itemToUpdate = Context.TblMatytalleres
                              .Where(i => i.IdMatyTaller == tblmatytallere.IdMatyTaller)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblmatytallere);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTblMatytallereUpdated(tblmatytallere);

            return tblmatytallere;
        }

        partial void OnTblMatytallereDeleted(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item);
        partial void OnAfterTblMatytallereDeleted(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere item);

        public async Task<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> DeleteTblMatytallere(int idmatytaller)
        {
            var itemToDelete = Context.TblMatytalleres
                              .Where(i => i.IdMatyTaller == idmatytaller)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblMatytallereDeleted(itemToDelete);


            Context.TblMatytalleres.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblMatytallereDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}