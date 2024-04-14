namespace Deductions
{
    public partial class CreateTransaction : Form
    {
        private int? _id = null;
        public CreateTransaction(string investmentName)
        {
            InitializeComponent();
            LoadData(investmentName);
        }

        public CreateTransaction(Transaction transaction)
        {
            InitializeComponent();
            LoadData(transaction);
        }

        public CreateTransaction(string investmentName, string investmentType)
        {
            InitializeComponent();
            LoadData(investmentName, investmentType);
        }
        private void LoadData(Transaction transaction)
        {
            List<string> investments = Database.getAllInvestments();
            investmentComboBox.DataSource = investments;
            investmentComboBox.SelectedItem = transaction.getInvestmentName();
            categoryTextBox.Text = transaction.Item;
            TransactionValueTextBox.Text = transaction.Amount.ToString();

            string[] transactionTypes = ["Income", "Expense"];
            TransactionTypeComboBox.DataSource = transactionTypes;  
            TransactionTypeComboBox.SelectedItem = transaction.TransactionType;

            TransactionDatePicker.Value = transaction.Date;
            noteTextBox.Text = transaction.Note;
            _id = transaction.getTransactionId();
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
            decimal value;
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
            if (Decimal.TryParse(TransactionValueTextBox.Text, out value))
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
                DateTime date = TransactionDatePicker.Value.Date;
                int fy = ToFinancialYear(TransactionDatePicker.Value);
                Database.UpsertTransaction(new Transaction(name, date, DateTime.Now, value, TransactionTypeComboBox.Text, fy, investmentComboBox.Text, noteTextBox.Text, "", _id));
                this.DialogResult = DialogResult.OK;
            }
        }

        public static int ToFinancialYear(DateTime dateTime)
        {
            return dateTime.Month >= 7 ? dateTime.Year + 1 : dateTime.Year;
        }
    }
}
