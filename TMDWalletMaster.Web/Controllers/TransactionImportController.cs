using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMDWalletMaster.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionImportController : ControllerBase
    {
        private readonly IExcelImportService _excelImportService;

        public TransactionImportController(IExcelImportService excelImportService)
        {
            _excelImportService = excelImportService;
        }

        [HttpPost("upload")]
        public ActionResult<List<BankTransaction>> UploadFile()
        {
            var file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                var transactions = _excelImportService.ImportTransactions(stream);
                return Ok(transactions);
            }
        }
    }
}