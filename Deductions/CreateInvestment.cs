namespace Deductions
{
    public partial class CreateInvestment : Form
    {
        public CreateInvestment()
        {
            InitializeComponent();
            List<string> accounts = Database.GetAccounts();
            accountComboBox.DataSource = accounts;
        }
        private void confirmCreateInvestment_Click(object sender, EventArgs e)
        {
            bool valid = true;
            if (accountComboBox.Text == null )
            {
                valid = false;
                accountComboBox.BackColor = Color.Red;
            } else
            {

                accountComboBox.BackColor = Color.White;
            }
            if (InvestmentName_Textbox.Text.Length == 0)
            {
                valid = false;
                InvestmentName_Textbox.BackColor = Color.Red;
            } else
            {

                InvestmentName_Textbox.BackColor = Color.White;
            }
            if (valid)
            {
                String name = InvestmentName_Textbox.Text;
                String accountName = accountComboBox.Text;
                Database.CreateNewInvestment(name, accountName);
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
