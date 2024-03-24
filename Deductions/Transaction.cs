namespace Deductions
{
    internal class Transaction
    {
        public String TransactionType { get; }
        public String transactionName { get; }
        public DateTime date { get; }
        public Double value { get; set; }
        public int financialYear { get; }
        public String investmentName { get; }

        public Transaction(String transactionName,
                            DateTime date,
                            Double value,
                            String TransactionType,
                            int financialYear,
                            String investmentName)
        {
            this.transactionName = transactionName;
            this.TransactionType = TransactionType;
            this.date = date;
            this.value = value;
            this.financialYear = financialYear;
            this.investmentName = investmentName;
        }
        
    }
}
