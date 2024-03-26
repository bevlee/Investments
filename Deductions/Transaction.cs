namespace Deductions
{
    internal class Transaction
    {
        public String TransactionType { get; }
        public String category { get; }
        public DateTime date { get; }
        public Double amount { get; set; }
        public int financialYear { get; }
        public String investmentName { get; }

        public Transaction(String category,
                            DateTime date,
                            Double amount,
                            String TransactionType,
                            int financialYear,
                            String investmentName)
        {
            this.category = category;
            this.TransactionType = TransactionType;
            this.date = date;
            this.amount = amount;
            this.financialYear = financialYear;
            this.investmentName = investmentName;
        }
        
    }
}
