using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using PlanificacionAulas.Data;

namespace PlanificacionAulas.Controllers
{
    public partial class ExportAulasYHorariosController : ExportController
    {
        private readonly AulasYHorariosContext context;
        private readonly AulasYHorariosService service;

        public ExportAulasYHorariosController(AulasYHorariosContext context, AulasYHorariosService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/AulasYHorarios/clases/csv")]
        [HttpGet("/export/AulasYHorarios/clases/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClasesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetClases(), Request.Query), fileName);
        }

        [HttpGet("/export/AulasYHorarios/clases/excel")]
        [HttpGet("/export/AulasYHorarios/clases/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClasesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetClases(), Request.Query), fileName);
        }

        [HttpGet("/export/AulasYHorarios/espacios/csv")]
        [HttpGet("/export/AulasYHorarios/espacios/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEspaciosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEspacios(), Request.Query), fileName);
        }

        [HttpGet("/export/AulasYHorarios/espacios/excel")]
        [HttpGet("/export/AulasYHorarios/espacios/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEspaciosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEspacios(), Request.Query), fileName);
        }

        [HttpGet("/export/AulasYHorarios/tblmateria/csv")]
        [HttpGet("/export/AulasYHorarios/tblmateria/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblMateriaToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblMateria(), Request.Query), fileName);
        }

        [HttpGet("/export/AulasYHorarios/tblmateria/excel")]
        [HttpGet("/export/AulasYHorarios/tblmateria/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblMateriaToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblMateria(), Request.Query), fileName);
        }

        [HttpGet("/export/AulasYHorarios/tblmatytalleres/csv")]
        [HttpGet("/export/AulasYHorarios/tblmatytalleres/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblMatytalleresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblMatytalleres(), Request.Query), fileName);
        }

        [HttpGet("/export/AulasYHorarios/tblmatytalleres/excel")]
        [HttpGet("/export/AulasYHorarios/tblmatytalleres/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTblMatytalleresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblMatytalleres(), Request.Query), fileName);
        }
    }
}
