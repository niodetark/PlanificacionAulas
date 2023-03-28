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
    public partial class AddClase
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

        protected override async Task OnInitializedAsync()
        {

            espaciosForEspacioId = await AulasYHorariosService.GetEspacios();

            tblMateriaForMateriatblMateriaId = await AulasYHorariosService.GetTblMateria();
        }
        protected bool errorVisible;
        protected PlanificacionAulas.Models.AulasYHorarios.Clase clase;

        protected IEnumerable<PlanificacionAulas.Models.AulasYHorarios.Espacio> espaciosForEspacioId;

        protected IEnumerable<PlanificacionAulas.Models.AulasYHorarios.TblMateria> tblMateriaForMateriatblMateriaId;

        protected async Task FormSubmit()
        {
            try
            {
                await AulasYHorariosService.CreateClase(clase);
                DialogService.Close(clase);
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





        bool hasEspacioIdValue;

        [Parameter]
        public int? EspacioId { get; set; }

        bool hasMateriatblMateriaIdValue;

        [Parameter]
        public int MateriatblMateriaId { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            clase = new PlanificacionAulas.Models.AulasYHorarios.Clase();

            hasEspacioIdValue = parameters.TryGetValue<int?>("EspacioId", out var hasEspacioIdResult);

            if (hasEspacioIdValue)
            {
                clase.EspacioId = hasEspacioIdResult;
            }

            hasMateriatblMateriaIdValue = parameters.TryGetValue<int>("MateriatblMateriaId", out var hasMateriatblMateriaIdResult);

            if (hasMateriatblMateriaIdValue)
            {
                clase.MateriatblMateriaId = hasMateriatblMateriaIdResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}