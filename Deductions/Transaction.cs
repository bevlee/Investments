namespace Deductions
{
    internal class Transaction
    {
        public string investmentName { get; }
        public DateTime date { get; }
        public string category { get; }
        public decimal amount { get; set; }
        public string TransactionType { get; }
        public int financialYear { get; }
        public DateTime lastModifiedDate { get; }
        public string note { get; }
        public string source { get; }
        public Transaction(string category,
                            DateTime date,
                            DateTime lastModifiedDate,
                            decimal amount,
                            string TransactionType,
                            int financialYear,
                            string investmentName,
                            string note,
                            string source)
        {
            this.category = category;
            this.TransactionType = TransactionType;
            this.date = date.Date;
            this.lastModifiedDate = lastModifiedDate;
            this.amount = amount;
            this.financialYear = financialYear;
            this.investmentName = investmentName;
            this.note = note;
            this.source = source;
        }
        public override string ToString()

        {
            return $"Investment: {investmentName}\n date: {date}\ncategory: {category}\namount: {amount}\nTransactionType: {TransactionType}\n financialYear: {financialYear}\n lastModifiedDate: {lastModifiedDate}\n note: {note}\n source: {source}\n";
        }
    }
}