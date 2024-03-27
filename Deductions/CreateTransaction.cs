namespace Deductions
{
    public partial class CreateTransaction : Form
    {
        public CreateTransaction(string investmentName)
        {
            InitializeComponent();
            LoadData(investmentName);
        }

        public CreateTransaction(string investmentName, string investmentType)
        {
            InitializeComponent();
            LoadData(investmentName, investmentType);
        }
        private void LoadData(string investmentName)
        {

            List<string> investments = Database.getAllInvestments();
            investmentComboBox.DataSource = investments;
            investmentComboBox.SelectedItem = investmentName;

            string[] transactionTypes = ["Income", "Expense"];
            TransactionTypeComboBox.DataSource = transactionTypes;
        }

        private void LoadData(string investmentName, string investmentType)
        {

            List<string> investments = Database.getAllInvestments();
            investmentComboBox.DataSource = investments;
            investmentComboBox.SelectedItem = investmentName;

            string[] transactionTypes = ["Income", "Expense"];
            TransactionTypeComboBox.DataSource = transactionTypes;
            TransactionTypeComboBox.SelectedItem = investmentType;
        }
        private void createTransactionButton_Click(object sender, EventArgs e)
        {
            
            bool valid = true;
            double value;
            if (TransactionTypeComboBox.Text == null)
            {
                valid = false;
                TransactionTypeComboBox.BackColor = Color.Red;
            }
            else
            {

                TransactionTypeComboBox.BackColor = Color.White;
            }
            if (categoryTextBox.Text.Length == 0)
            {
                valid = false;
                categoryTextBox.BackColor = Color.Red;
            }
            else
            {

                categoryTextBox.BackColor = Color.White;
            }
            if (Double.TryParse(TransactionValueTextBox.Text, out value))
            {
                TransactionValueTextBox.BackColor = Color.White;
            }
            else
            {
                valid = false;
                TransactionValueTextBox.BackColor = Color.Red;
            }
            if (valid)
            {
                string name = categoryTextBox.Text;
                DateTime date = ((DateTimeOffset)TransactionDatePicker.Value).UtcDateTime;
                int fy = ToFinancialYear(TransactionDatePicker.Value);
                Database.CreateNewTransaction(new Transaction(name, date, value, TransactionTypeComboBox.Text, 24, investmentComboBox.Text));
                this.DialogResult = DialogResult.OK;
            }
        }

        public static int ToFinancialYear(DateTime dateTime)
        {
            return dateTime.Month >= 7 ? dateTime.Year + 1 : dateTime.Year;
        }
    }
}
