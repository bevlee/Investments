namespace Deductions
{
    public partial class ManageInvestments : Form
    {
        public ManageInvestments()
        {
            InitializeComponent();
            LoadData();
        }

        private void createInvestmentButton_Click(object sender, EventArgs e)
        {
            CreateInvestment createInvestmentForm = new CreateInvestment();
            if (createInvestmentForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            System.Diagnostics.Debug.WriteLine($" loading!");

            List<(string, string)> investments = Database.getAllInvestmentsAndAccounts();


            investmentsListBox.DataSource = investments;
        }

        private void deleteInvestmentButton_Click(object sender, EventArgs e)
        {
            ValueTuple<string, string> selectedTuple =
                  (ValueTuple<string, string>)investmentsListBox.SelectedItem;
            DialogResult dialogResult = MessageBox.Show(
                $"Are you sure you wish to delete\n\n" +
                $"Investment: {selectedTuple.Item1}\n\n" +
                $"All the corresponding data associated with it will be deleted too!", "Delete Investment", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Database.DeleteInvestment(selectedTuple.Item1);
                LoadData();
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
