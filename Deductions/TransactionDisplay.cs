namespace Deductions
{
    internal class TransactionDisplay
    {
        private string _name;  
        public DateTime Date { get; }
        public string Item { get; }
        public string Amount { get; set; }
        public string TransactionType { get; }
        public int FinancialYear { get; }
        public DateTime LastModifiedDate { get; }
        public string Note { get; }
        public TransactionDisplay(string category,
                            DateTime Date,
                            DateTime lastModifiedDate,
                            decimal amount,
                            string TransactionType,
                            int financialYear,
                            string note)
        {
            this.Item = category;
            this.TransactionType = TransactionType;
            this.Date = Date.Date;
            this.LastModifiedDate = lastModifiedDate;
            this.Amount = "$" + amount;
            this.FinancialYear = financialYear;
            this.Note = note;
        }

        public TransactionDisplay(Transaction transaction)
        {
            this._name = "secret string";
            this.Item = transaction.category;
            this.TransactionType = transaction.TransactionType;
            this.Date = transaction.date.Date;
            this.LastModifiedDate = transaction.lastModifiedDate;
            this.Amount = "$" + transaction.amount;
            this.FinancialYear = transaction.financialYear;
            this.Note = transaction.note;
        }
    }
}
