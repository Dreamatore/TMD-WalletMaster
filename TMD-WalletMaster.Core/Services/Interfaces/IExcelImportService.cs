using System.Collections.Generic;
using System.IO;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Services.Interfaces
{
    public interface IExcelImportService
    {
        List<BankTransaction> ImportTransactions(Stream fileStream);
    }
}