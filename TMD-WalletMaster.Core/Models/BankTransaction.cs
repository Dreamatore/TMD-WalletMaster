namespace TMD_WalletMaster.Core.Models
{
    public class BankTransaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }  // Дата совершения операции
        public DateTime ConfirmationDate { get; set; }  // Дата подтверждения
        public string Category { get; set; }  // Категория
        public decimal Amount { get; set; }  // Сумма
        public string Currency { get; set; }  // Валюта
        public string Description { get; set; }  // Описание
        public string DetailedDescription { get; set; }  // Подробное описание
        public string DestinationAccount { get; set; }  // Куда зачислено
        public string SourceAccount { get; set; }  // Откуда списано
        public string SenderAccountNumber { get; set; }  // Номер счета отправителя
        public string TransferNumber { get; set; }  // Номер перевода
        public string MCCCode { get; set; }  // MCC код транзакции
    }
}