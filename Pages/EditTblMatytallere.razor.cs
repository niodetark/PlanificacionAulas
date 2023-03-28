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
    public partial class EditTblMatytallere
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
        public int IdMatyTaller { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tblMatytallere = await AulasYHorariosService.GetTblMatytallereByIdMatyTaller(IdMatyTaller);

            tblMateriaFortblMateriaId = await AulasYHorariosService.GetTblMateria();
        }
        protected bool errorVisible;
        protected PlanificacionAulas.Models.AulasYHorarios.TblMatytallere tblMatytallere;

        protected IEnumerable<PlanificacionAulas.Models.AulasYHorarios.TblMateria> tblMateriaFortblMateriaId;

        protected async Task FormSubmit()
        {
            try
            {
                await AulasYHorariosService.UpdateTblMatytallere(IdMatyTaller, tblMatytallere);
                DialogService.Close(tblMatytallere);
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





        bool hastblMateriaIdValue;

        [Parameter]
        public int tblMateriaId { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            tblMatytallere = new PlanificacionAulas.Models.AulasYHorarios.TblMatytallere();

            hastblMateriaIdValue = parameters.TryGetValue<int>("tblMateriaId", out var hastblMateriaIdResult);

            if (hastblMateriaIdValue)
            {
                tblMatytallere.tblMateriaId = hastblMateriaIdResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}