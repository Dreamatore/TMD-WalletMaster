using OfficeOpenXml;
using TMD_WalletMaster.Core.Models;
using TMD_WalletMaster.Core.Services.Interfaces;

namespace TMD_WalletMaster.Core.Services
{
    public class ExcelImportService : IExcelImportService
    {
        public List<BankTransaction> ImportTransactions(Stream fileStream)
        {
            var transactions = new List<BankTransaction>();

            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)  // Предполагаем, что первая строка содержит заголовки
                {
                    var transaction = new BankTransaction
                    {
                        TransactionDate = worksheet.Cells[row, 1].GetValue<DateTime>(),  // Дата совершения операции
                        ConfirmationDate = worksheet.Cells[row, 2].GetValue<DateTime>(),  // Дата подтверждения
                        Category = worksheet.Cells[row, 3].GetValue<string>(),  // Категория
                        Amount = worksheet.Cells[row, 4].GetValue<decimal>(),  // Сумма
                        Currency = worksheet.Cells[row, 5].GetValue<string>(),  // Валюта
                        Description = worksheet.Cells[row, 6].GetValue<string>(),  // Описание
                        DetailedDescription = worksheet.Cells[row, 7].GetValue<string>(),  // Подробное описание
                        DestinationAccount = worksheet.Cells[row, 8].GetValue<string>(),  // Куда зачислено
                        SourceAccount = worksheet.Cells[row, 9].GetValue<string>(),  // Откуда списано
                        SenderAccountNumber = worksheet.Cells[row, 10].GetValue<string>(),  // Номер счета отправителя
                        TransferNumber = worksheet.Cells[row, 11].GetValue<string>(),  // Номер перевода
                        MCCCode = worksheet.Cells[row, 12].GetValue<string>()  // MCC код транзакции
                    };

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
    }
}
