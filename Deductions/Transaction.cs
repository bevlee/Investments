namespace Deductions
{
    public class Transaction
    {
        private int? _transactionId;
        private string _investmentName;
        public DateTime Date { get; }
        public string Item { get; }
        public decimal Amount { get; }
        public string TransactionType { get; }
        public int FinancialYear { get; }
        public DateTime LastModifiedDate { get; }
        public string Note { get; }
        public string Source { get; }
        public Transaction(string category,
                            DateTime date,
                            DateTime lastModifiedDate,
                            decimal amount,
                            string transactionType,
                            int financialYear,
                            string investmentName,
                            string note,
                            string source,
                            int? transactionId)
        {
            _transactionId = transactionId;
            Item = category;
            TransactionType = transactionType;
            Date = date.Date;
            LastModifiedDate = lastModifiedDate;
            Amount = amount;
            FinancialYear = financialYear;
            _investmentName = investmentName;
            Note = note;
            Source = source;
        }
        public override string ToString()

        {
            return $"TransactionId: {_transactionId}\nInvestment: {_investmentName}\n Date: {Date}\nItem: {Item}\namount: {Amount}\nTransactionType: {TransactionType}\n financialYear: {FinancialYear}\n lastModifiedDate: {LastModifiedDate}\n note: {Note}\n source: {Source}\n";
        }
        public int? getTransactionId() { return _transactionId ; }
        public string getInvestmentName() { return _investmentName; }
    }
}