namespace Deductions
{
    internal class Transaction
    {
        public string TransactionType { get; }
        public string category { get; }
        public Double amount { get; set; }
        public DateTime date { get; }
        public int financialYear { get; }
        public DateTime lastModifiedDate { get; }
        
        public string investmentName { get; }
        public string note { get; }
        public string source { get; }

        public Transaction(string category,
                            DateTime date,
                            DateTime lastModifiedDate,
                            Double amount,
                            string TransactionType,
                            int financialYear,
                            string investmentName,
                            string note,
                            string source)
        {
            this.category = category;
            this.TransactionType = TransactionType;
            this.date = date;
            this.lastModifiedDate = lastModifiedDate;
            this.amount = amount;
            this.financialYear = financialYear;
            this.investmentName = investmentName;
            this.note = note;
            this.source = source;
        }
    }
}