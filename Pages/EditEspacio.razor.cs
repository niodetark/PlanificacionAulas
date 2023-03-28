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
    public partial class EditEspacio
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

        [Parameter]
        public int EspacioId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            espacio = await AulasYHorariosService.GetEspacioByEspacioId(EspacioId);
        }
        protected bool errorVisible;
        protected PlanificacionAulas.Models.AulasYHorarios.Espacio espacio;

        protected async Task FormSubmit()
        {
            try
            {
                await AulasYHorariosService.UpdateEspacio(EspacioId, espacio);
                DialogService.Close(espacio);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}