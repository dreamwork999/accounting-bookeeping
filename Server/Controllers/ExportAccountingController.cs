using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using AccountingBookkeeping.Server.Data;

namespace AccountingBookkeeping.Server.Controllers
{
    public partial class ExportaccountingController : ExportController
    {
        private readonly accountingContext context;
        private readonly accountingService service;

        public ExportaccountingController(accountingContext context, accountingService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/accounting/pricings/csv")]
        [HttpGet("/export/accounting/pricings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPricingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPricings(), Request.Query), fileName);
        }

        [HttpGet("/export/accounting/pricings/excel")]
        [HttpGet("/export/accounting/pricings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPricingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPricings(), Request.Query), fileName);
        }
    }
}
