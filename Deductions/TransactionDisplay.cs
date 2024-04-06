namespace Deductions
{
    internal class TransactionDisplay
    {
        public string date { get; }
        public string item { get; }
        public string amount { get; set; }
        public string TransactionType { get; }
        public int financialYear { get; }
        public DateTime lastModifiedDate { get; }
        public string note { get; }
        public TransactionDisplay(string category,
                            string date,
                            DateTime lastModifiedDate,
                            decimal amount,
                            string TransactionType,
                            int financialYear,
                            string note)
        {
            this.item = category;
            this.TransactionType = TransactionType;
            this.date = date;
            this.lastModifiedDate = lastModifiedDate;
            this.amount = "$" + amount;
            this.financialYear = financialYear;
            this.note = note;
        }

        public TransactionDisplay(Transaction transaction)
        {
            this.item = transaction.category;
            this.TransactionType = transaction.TransactionType;
            this.date = transaction.date.Date.ToString();
            this.lastModifiedDate = transaction.lastModifiedDate;
            this.amount = "$" + transaction.amount;
            this.financialYear = transaction.financialYear;
            this.note = transaction.note;
        }
    }
}
