using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace PlanificacionAulas.Pages
{
    public partial class TblMateria
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public AulasYHorariosService AulasYHorariosService { get; set; }

        protected IEnumerable<PlanificacionAulas.Models.AulasYHorarios.TblMateria> tblMateria;

        protected RadzenDataGrid<PlanificacionAulas.Models.AulasYHorarios.TblMateria> grid0;
        protected override async Task OnInitializedAsync()
        {
            tblMateria = await AulasYHorariosService.GetTblMateria();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTblMateria>("Add TblMateria", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PlanificacionAulas.Models.AulasYHorarios.TblMateria> args)
        {
            await DialogService.OpenAsync<EditTblMateria>("Edit TblMateria", new Dictionary<string, object> { {"tblMateriaId", args.Data.tblMateriaId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PlanificacionAulas.Models.AulasYHorarios.TblMateria tblMateria)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await AulasYHorariosService.DeleteTblMateria(tblMateria.tblMateriaId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete TblMateria" 
                });
            }
        }

        protected PlanificacionAulas.Models.AulasYHorarios.TblMateria tblMaterias;
        protected async Task GetChildData(PlanificacionAulas.Models.AulasYHorarios.TblMateria args)
        {
            tblMaterias = args;
            var ClasesResult = await AulasYHorariosService.GetClases(new Query { Filter = $@"i => i.MateriatblMateriaId == {args.tblMateriaId}", Expand = "Espacio,TblMateria" });
            if (ClasesResult != null)
            {
                args.Clases = ClasesResult.ToList();
            }
            var TblMatytalleresResult = await AulasYHorariosService.GetTblMatytalleres(new Query { Filter = $@"i => i.tblMateriaId == {args.tblMateriaId}", Expand = "TblMateria" });
            if (TblMatytalleresResult != null)
            {
                args.TblMatytalleres = TblMatytalleresResult.ToList();
            }
        }

        protected RadzenDataGrid<PlanificacionAulas.Models.AulasYHorarios.Clase> ClasesDataGrid;

        protected async Task ClasesAddButtonClick(MouseEventArgs args, PlanificacionAulas.Models.AulasYHorarios.TblMateria data)
        {
            var dialogResult = await DialogService.OpenAsync<AddClase>("Add Clases", new Dictionary<string, object> { {"MateriatblMateriaId" , data.tblMateriaId} });
            await GetChildData(data);
            await ClasesDataGrid.Reload();
        }

        protected async Task ClasesRowSelect(PlanificacionAulas.Models.AulasYHorarios.Clase args, PlanificacionAulas.Models.AulasYHorarios.TblMateria data)
        {
            var dialogResult = await DialogService.OpenAsync<EditClase>("Edit Clases", new Dictionary<string, object> { {"ClaseId", args.ClaseId} });
            await GetChildData(data);
            await ClasesDataGrid.Reload();
        }

        protected async Task ClasesDeleteButtonClick(MouseEventArgs args, PlanificacionAulas.Models.AulasYHorarios.Clase clase)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await AulasYHorariosService.DeleteClase(clase.ClaseId);

                    await GetChildData(tblMaterias);

                    if (deleteResult != null)
                    {
                        await ClasesDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Clase" 
                });
            }
        }

        protected RadzenDataGrid<PlanificacionAulas.Models.AulasYHorarios.TblMatytallere> TblMatytalleresDataGrid;

        protected async Task TblMatytalleresAddButtonClick(MouseEventArgs args, PlanificacionAulas.Models.AulasYHorarios.TblMateria data)
        {
            var dialogResult = await DialogService.OpenAsync<AddTblMatytallere>("Add TblMatytalleres", new Dictionary<string, object> { {"tblMateriaId" , data.tblMateriaId} });
            await GetChildData(data);
            await TblMatytalleresDataGrid.Reload();
        }

        protected async Task TblMatytalleresRowSelect(PlanificacionAulas.Models.AulasYHorarios.TblMatytallere args, PlanificacionAulas.Models.AulasYHorarios.TblMateria data)
        {
            var dialogResult = await DialogService.OpenAsync<EditTblMatytallere>("Edit TblMatytalleres", new Dictionary<string, object> { {"IdMatyTaller", args.IdMatyTaller} });
            await GetChildData(data);
            await TblMatytalleresDataGrid.Reload();
        }

        protected async Task TblMatytalleresDeleteButtonClick(MouseEventArgs args, PlanificacionAulas.Models.AulasYHorarios.TblMatytallere tblMatytallere)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await AulasYHorariosService.DeleteTblMatytallere(tblMatytallere.IdMatyTaller);

                    await GetChildData(tblMaterias);

                    if (deleteResult != null)
                    {
                        await TblMatytalleresDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete TblMatytallere" 
                });
            }
        }

        string lastFilter;
        protected async void Grid0Render(DataGridRenderEventArgs<PlanificacionAulas.Models.AulasYHorarios.TblMateria> args)
        {
            if (grid0.Query.Filter != lastFilter) {
                tblMaterias = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter)
            {
                await grid0.SelectRow(tblMaterias);
            }

            lastFilter = grid0.Query.Filter;
        }
    }
}