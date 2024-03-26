using System.Data.SQLite;
namespace Deductions
{
    public partial class HomePage : Form
    {
        private string accountName;
        private string? investmentName = null;
        private string financialYearString = "All";
        private bool loadNewDates = true;
        private HashSet<string> allDates;

        public HomePage()
        {
            InitializeComponent();
            InitDb();
            LoadData();
        }

        private void InitDb()
        {
            string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Deductions");
            if (!Directory.Exists(appDir))
            {
                Directory.CreateDirectory(appDir);
            }
            var db_location = Path.Combine(appDir, "Deductions.db");

            SQLiteConnection sqlite_conn;

            // Create a new database if it does not exist
            sqlite_conn = new SQLiteConnection("Data Source=" + db_location + "; Version = 3;");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
                // check if the tables exist
                List<string> tables = new List<string>();
                var command = sqlite_conn.CreateCommand();
                command.CommandText =
                    @"
                    SELECT
                        name
                    FROM
                        sqlite_schema
                    WHERE
                        type = 'table' AND
                        name NOT LIKE 'sqlite_%'
                ";
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var tableName = reader.GetString(0);
                        tables.Add(tableName);
                    }
                }
                string[] expectedTables = ["Transactions, Accounts, Investments"];
                bool tablesPresent = false;
                foreach (string item in expectedTables)
                {
                    if (tables.Contains(item))
                    { 
                        tablesPresent = true; 
                    } else
                    {
                        tablesPresent = false;
                        break;
                    }
                }
                
                //create the tables if they dont exist
                if (!tablesPresent)
                {
                    // create the Acc table
                    command.CommandText =
                    @"
                        CREATE TABLE Accounts(
                        AccountName TEXT NOT NULL PRIMARY KEY
                        );
                    ";
                    command.ExecuteNonQuery();

                    // create the Investments table
                    command.CommandText =
                    @"
                        CREATE TABLE Investments(
                        InvestmentName TEXT NOT NULL PRIMARY KEY,
                        AccountName TEXT NOT NULL,
                        FOREIGN KEY (AccountName) REFERENCES Accounts (AccountName)
                        );
                    ";
                    command.ExecuteNonQuery();
                    
                    // create the Transactions table
                    command.CommandText =
                    @"
                        CREATE TABLE Transactions (
                        TransactionId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Category TEXT NOT NULL,
                        Date INTEGER NOT NULL,
                        Value REAL NOT NULL,
                        TransactionType TEXT NOT NULL,
                        FinancialYear INTEGER NOT NULL,
                        InvestmentName TEXT NOT NULL,
                        FOREIGN KEY (InvestmentName) REFERENCES Investments (InvestmentName)
                        );
                    ";
                    command.ExecuteNonQuery();

                    // create the account
                    command.CommandText =
                    @"
                        INSERT into Accounts (AccountName)
                        VALUES ('Me');
                    ";
                    command.ExecuteNonQuery();

                    // create a dummy investment
                    command.CommandText =
                    @"
                        INSERT into Investments (InvestmentName, AccountName)
                        VALUES ('Test', 'Me');
                    ";
                    command.ExecuteNonQuery();

                    // create dummy transactions
                    command.CommandText =
                    @"
                        INSERT INTO Transactions (category, InvestmentName, Value, Date, TransactionType, FinancialYear)
                        VALUES 
                            ('Jan rent', 'Test', 1500, 1706679357, 'Income', '2024'),
                            ('Feb rent', 'Test', 1500, 1709184957, 'Income', '2024'),
                            ('Cleaning Costs', 'Test', 950, 1707973200, 'Expense', '2024'),
                            ('2024 Depreciation', 'Test', 4589.23, 1707973200, 'Depreciation/Capital Expense', '2024');
                    ";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("exception found" + ex);
            }
            return;
        }

        private void LoadData()
        {
            System.Diagnostics.Debug.WriteLine($" loading!");
            List<string> accounts = Database.GetAccounts();
            accountComboBox.DataSource = accounts;
            List<string> investments = Database.getAllInvestments();
            List<string> investmentsWithSelection = new List<string>();

            foreach (string investment in investments)
            {
                if (investment == investmentName)
                {
                    investmentsWithSelection = investmentsWithSelection.Prepend(investment).ToList();
                }
                else
                {
                    investmentsWithSelection.Add(investment);
                }
            }
            System.Diagnostics.Debug.WriteLine($"lnegth of investmentts is, {investmentsWithSelection.Count} !");

            investmentComboBox.DataSource = investmentsWithSelection;
            investmentComboBox.SelectedIndex = 0;
            investmentName = investmentsWithSelection[0];

            DisplayTransactions();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {

        }
        private void DeleteTransactions(object sender, EventArgs e)
        {

            System.Diagnostics.Debug.WriteLine($" deleting row(s)!");
            if (this.TransactionDataGridView.SelectedRows.Count > 0)
            {
                List<Transaction> transactionList = new List<Transaction>();
                foreach (DataGridViewRow row in TransactionDataGridView.SelectedRows)
                {
                    string transactionType = row.Cells[0].Value.ToString();
                    string category = row.Cells[1].Value.ToString();
                    DateTime date = ((DateTimeOffset)DateTime.Parse(row.Cells[2].Value.ToString())).UtcDateTime;
                    double value = Double.Parse(row.Cells[3].Value.ToString());
                    int financialYear = int.Parse(row.Cells[4].Value.ToString());
                    string investmentName = row.Cells[5].Value.ToString();
                    transactionList.Add(new Transaction(category, date, value, transactionType, financialYear, investmentName));

                }
                Database.DeleteTransactions(transactionList);
                LoadData();
            }
        }
        private void DisplayTransactions()
        {
            System.Diagnostics.Debug.WriteLine($" displaying transactions!");
            List<Transaction> allTransactions = Database.LoadTransactions(investmentName, financialYearString);
            double netValue = 0;
            allTransactions.ForEach(transaction =>
            {
                netValue += transaction.TransactionType == "Income" ? transaction.amount : -transaction.amount;
                
            });

            TransactionDataGridView.DataSource = allTransactions;
            if (loadNewDates)
            {

                System.Diagnostics.Debug.WriteLine($" loading new dates!");
                allDates = new HashSet<string>(["All"]);
                foreach (Transaction transaction in allTransactions)
                {
                    allDates.Add(transaction.financialYear.ToString());
                }
                List<string> dates = new List<string>(allDates);
                FinancialYearComboBox.DataSource = dates;
                financialYearString = "All";
                FinancialYearComboBox.SelectedItem = financialYearString;
                System.Diagnostics.Debug.WriteLine($" new fy is {financialYearString}!");
                loadNewDates = false;
            }
            NetValueLabel.Text = $"The net value for investment {investmentName} is {netValue}";
        }

        private void createInvestmentButton_Click(object sender, EventArgs e)
        {
            CreateInvestment createInvestmentForm = new CreateInvestment();
            if (createInvestmentForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }


        private void createTransactionButton_Click(object sender, EventArgs e)
        {

            CreateTransaction createTransactionForm = new CreateTransaction(investmentName);
            if (createTransactionForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void investmentComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if (investmentComboBox.SelectedItem.ToString() != investmentName)
            {
                //MessageBox.Show("Selected value changed to: " + investmentComboBox.SelectedItem.ToString());

                System.Diagnostics.Debug.WriteLine($" changing investments");
                investmentName = investmentComboBox.SelectedItem.ToString();

                loadNewDates = true;
                financialYearString = "All";
                LoadData();
            }
        }

        private void financialYearComboBox_SelectionChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Selected value changed to: " + investmentComboBox.SelectedItem.ToString());
            string fy = FinancialYearComboBox.SelectedItem.ToString();
            if (fy != financialYearString)
            {
                System.Diagnostics.Debug.WriteLine($" changing FY");
                financialYearString = fy;
            }
            LoadData();
        }
        private int _previousIndex;
        private bool _sortDirection;
        private void TransactionDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction

            TransactionDataGridView.DataSource = SortData(
                (List<Transaction>)TransactionDataGridView.DataSource, TransactionDataGridView.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
        }

        private List<Transaction> SortData(List<Transaction> list, string column, bool ascending)
        {
            return ascending ?
                list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() :
                list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList();
        }
    }
}
