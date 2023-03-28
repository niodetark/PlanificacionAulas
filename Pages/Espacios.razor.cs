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
    public partial class Espacios
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

        protected IEnumerable<PlanificacionAulas.Models.AulasYHorarios.Espacio> espacios;

        protected RadzenDataGrid<PlanificacionAulas.Models.AulasYHorarios.Espacio> grid0;
        protected override async Task OnInitializedAsync()
        {
            espacios = await AulasYHorariosService.GetEspacios();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEspacio>("Add Espacio", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PlanificacionAulas.Models.AulasYHorarios.Espacio> args)
        {
            await DialogService.OpenAsync<EditEspacio>("Edit Espacio", new Dictionary<string, object> { {"EspacioId", args.Data.EspacioId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PlanificacionAulas.Models.AulasYHorarios.Espacio espacio)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await AulasYHorariosService.DeleteEspacio(espacio.EspacioId);

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
                    Detail = $"Unable to delete Espacio" 
                });
            }
        }

        protected PlanificacionAulas.Models.AulasYHorarios.Espacio espacio;
        protected async Task GetChildData(PlanificacionAulas.Models.AulasYHorarios.Espacio args)
        {
            espacio = args;
            var ClasesResult = await AulasYHorariosService.GetClases(new Query { Filter = $@"i => i.EspacioId == {args.EspacioId}", Expand = "Espacio,TblMateria" });
            if (ClasesResult != null)
            {
                args.Clases = ClasesResult.ToList();
            }
        }

        protected RadzenDataGrid<PlanificacionAulas.Models.AulasYHorarios.Clase> ClasesDataGrid;

        protected async Task ClasesAddButtonClick(MouseEventArgs args, PlanificacionAulas.Models.AulasYHorarios.Espacio data)
        {
            var dialogResult = await DialogService.OpenAsync<AddClase>("Add Clases", new Dictionary<string, object> { {"EspacioId" , data.EspacioId} });
            await GetChildData(data);
            await ClasesDataGrid.Reload();
        }

        protected async Task ClasesRowSelect(PlanificacionAulas.Models.AulasYHorarios.Clase args, PlanificacionAulas.Models.AulasYHorarios.Espacio data)
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

                    await GetChildData(espacio);

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

        string lastFilter;
        protected async void Grid0Render(DataGridRenderEventArgs<PlanificacionAulas.Models.AulasYHorarios.Espacio> args)
        {
            if (grid0.Query.Filter != lastFilter) {
                espacio = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter)
            {
                await grid0.SelectRow(espacio);
            }

            lastFilter = grid0.Query.Filter;
        }
    }
}