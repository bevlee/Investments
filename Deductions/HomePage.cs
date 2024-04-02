using System.Data.SQLite;
using LumenWorks.Framework.IO.Csv;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Properties;
namespace Deductions
{
    public partial class HomePage : Form
    {
        private string accountName;
        private string? selectedInvestment = null;
        private string financialYearString = "All";
        private bool loadNewDates = true;
        private HashSet<string> allDates;
        private string[] mandatoryFields = ["Category", "TransactionType", "Amount", "Date"];

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
                string[] expectedTables = ["Transactions", "Accounts", "Investments", "Sources"];
                bool tablesPresent = tables.All(expectedTables.Contains) && (tables.Count == expectedTables.Length);

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
                        Category TEXT NOT NULL,
                        Date INTEGER NOT NULL,
                        LastModifiedDate INTEGER NOT NULL,
                        Value REAL NOT NULL,
                        TransactionType TEXT NOT NULL,
                        FinancialYear INTEGER NOT NULL,
                        InvestmentName TEXT NOT NULL,
                        Note TEXT NOT NULL,
                        Source TEXT NOT NULL,
                        PRIMARY KEY (Category, Date, Value, TransactionType, InvestmentName),
                        FOREIGN KEY (Source) REFERENCES Sources (Source),
                        FOREIGN KEY (InvestmentName) REFERENCES Investments (InvestmentName)
                        );
                    ";
                    command.ExecuteNonQuery();

                    // create the Sources table
                    command.CommandText =
                    @"
                        CREATE TABLE Sources (
                        Source VARCHAR PRIMARY KEY NOT NULL
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
                        INSERT INTO Transactions (Category, InvestmentName, Value, Date, LastModifiedDate, Note, TransactionType, FinancialYear, Source)
                        VALUES 
                            ('Rent', 'Test', 1500, 1706679357, $currentDate, '', 'Income', '2024', ''),
                            ('Rent', 'Test', 1500, 1709184957, $currentDate, '', 'Income', '2024', ''),
                            ('Cleaning Costs', 'Test', 950, 1707973200, $currentDate, 'End of lease cleaning', 'Expense', '2024', ''),
                            ('Water', 'Test', 122.11, 1707973200, $currentDate, '', 'Expense', '2024', ''),
                            ('Gas', 'Test', 57.29, 1707973200, $currentDate, '', 'Expense', '2024', ''),
                            ('Electricity', 'Test', 259.35, 1707973200, $currentDate, '', 'Expense', '2024', '');
                    ";
                    command.Parameters.AddWithValue("$currentDate", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
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
                if (investment == selectedInvestment)
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
            selectedInvestment = investmentsWithSelection[0];

            DisplayTransactions();
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
                    double value = Double.Parse(row.Cells[2].Value.ToString());
                    DateTime date = ((DateTimeOffset)DateTime.Parse(row.Cells[3].Value.ToString())).UtcDateTime;
                    int financialYear = int.Parse(row.Cells[4].Value.ToString());
                    DateTime lastModifiedDate = ((DateTimeOffset)DateTime.Parse(row.Cells[5].Value.ToString())).UtcDateTime;
                    string selectedInvestment = row.Cells[6].Value.ToString();
                    string note = row.Cells[7].Value.ToString();
                    string source = row.Cells[8].Value.ToString();
                    transactionList.Add(new Transaction(category, date, lastModifiedDate, value, transactionType, financialYear, selectedInvestment, note, source));

                }
                Database.DeleteTransactions(transactionList);
                LoadData();
            }
        }
        private void DisplayTransactions()
        {
            System.Diagnostics.Debug.WriteLine($" displaying transactions!");
            List<Transaction> allTransactions = Database.LoadTransactions(selectedInvestment, financialYearString);
            double netValue = 0;
            allTransactions.ForEach(transaction =>
            {
                netValue += transaction.TransactionType == "Income" ? transaction.amount : -transaction.amount;
            });

            TransactionDataGridView.DataSource = allTransactions;

            // reload the selected dates
            allDates = new HashSet<string>(["All"]);
            foreach (Transaction transaction in allTransactions)
            {
                allDates.Add(transaction.financialYear.ToString());
            }
            List<string> dates = new List<string>(allDates);
            FinancialYearComboBox.DataSource = dates;
            FinancialYearComboBox.SelectedItem = financialYearString;

            NetValueLabel.Text = $"The net value for investment {selectedInvestment} is {netValue}";
        }

        private void manageInvestmentsButton_Click(object sender, EventArgs e)
        {
            ManageInvestments manageInvestmentsForm = new ManageInvestments();
            if (manageInvestmentsForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }


        private void createTransactionButton_Click(object sender, EventArgs e)
        {

            CreateTransaction createTransactionForm = new CreateTransaction(selectedInvestment);
            if (createTransactionForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
                createTransactionForm.Close();
            }
        }

        private void investmentComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if (investmentComboBox.SelectedItem.ToString() != selectedInvestment)
            {
                //MessageBox.Show("Selected value changed to: " + investmentComboBox.SelectedItem.ToString());

                System.Diagnostics.Debug.WriteLine($" changing investments");
                selectedInvestment = investmentComboBox.SelectedItem.ToString();


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

        private void createIncomeButton_Click(object sender, EventArgs e)
        {
            CreateTransaction createTransactionForm = new CreateTransaction(selectedInvestment, "Income");
            if (createTransactionForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void createExpenseButton_Click(object sender, EventArgs e)
        {
            CreateTransaction createTransactionForm = new CreateTransaction(selectedInvestment, "Expense");
            if (createTransactionForm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void importCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Transaction> transactions = new List<Transaction>();
            Transaction transaction;
            string category;
            string TransactionType;
            DateTime date;
            DateTime lastModifiedDate = DateTime.UtcNow;
            double amount;
            int financialYear;
            string investmentName = selectedInvestment;
            string note = "";
            string source = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "csv files| *.csv";
            openFileDialog.Title = "Please select a csv file to import.";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                try
                {
                    using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
                    {
                        csv.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;
                        int fieldCount = csv.FieldCount;
                        string[] headers = csv.GetFieldHeaders();
                        if (mandatoryFields.All(headers.Contains))
                        {
                            while (csv.ReadNextRecord())
                            {
                                category = csv[3];
                                TransactionType = csv[1];
                                date = DateTime.Parse(csv[0]);
                                amount = double.Parse(csv[2]);
                                financialYear = Database.ToFinancialYear(date);
                                if (headers.Length > 4 && headers[4] == "note")
                                {
                                    note = csv[4];
                                }
                                transaction = new Transaction(category, date, lastModifiedDate, amount, TransactionType, financialYear, investmentName, note, source);
                                //System.Diagnostics.Debug.WriteLine($" \"{headers[i]} = {csv[i]}\",\r\n");
                                transactions.Add(transaction);

                                System.Diagnostics.Debug.WriteLine("");
                            }
                        }
                        else
                        {
                            throw new Exception("mandatory fields not present in csv file");
                        }

                    }
                    Database.CreateNewTransactions(transactions);
                    LoadData();

                }
                catch (Exception ex)
                {
                    if (ex.Message == "mandatory fields not present in csv file")
                    {
                        MessageBox.Show(ex.Message + ". Please ensure the format is Date, TransactionType, Amount, Category, Notes (optional)");
                    }
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        private void generateReportButton_Click(object sender, EventArgs e)
        {
            List<Tuple<string, string, double>> categorySummary = Database.getSummary(accountName, selectedInvestment, financialYearString);
            //System.Diagnostics.Debug.WriteLine($" \"{headers[i]} = {csv[i]}\",\r\n");
            List<Tuple<string, double>> expenses = new List<Tuple<string, double>>();
            List<Tuple<string, double>> income = new List<Tuple<string, double>>();
            foreach (Tuple<string, string, double> category in categorySummary)
            {
                System.Diagnostics.Debug.WriteLine($" \"{category.Item1} = {category.Item3}\",\r\n");
                if (category.Item2 == "Expense")
                {
                    expenses.Add(new Tuple<string, double>(category.Item1, category.Item3));
                }
                else
                {
                    income.Add(new Tuple<string, double>(category.Item1, category.Item3));
                }
            }
            double incomeTotal = income.Sum(x => x.Item2);
            double expensesTotal = expenses.Sum(x => x.Item2);
            double total = Math.Round(incomeTotal - expensesTotal, 2, MidpointRounding.AwayFromZero);
            string totalString = total > 0 ? "$" + total : "-$" + total * -1;
            

            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "pdf files | *.pdf";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                PdfDocument pdfDoc = new PdfDocument(new PdfWriter(saveFileDialog.FileName));
                Document doc = new Document(pdfDoc);
                PdfFont code = PdfFontFactory.CreateFont(StandardFonts.COURIER);

                
                Style titleStyle = new Style()
                .SetFont(code)
                .SetFontSize(28)
                .SetFontColor(ColorConstants.RED)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                Paragraph title = new Paragraph($"{(financialYearString == "All" ? "Historical" : financialYearString)} summary for {selectedInvestment}").AddStyle(titleStyle);

                doc.Add(title);

                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 80, 10, 10 }));
                table.SetMarginTop(5);
                table.AddCell("Category");
                table.AddCell("Expense");
                table.AddCell("Income");
                foreach (Tuple<string, string, double> item in categorySummary)
                {
                    table.AddCell(item.Item1);
                    table.AddCell(item.Item2 == "Expense" ? item.Item3.ToString() : "");
                    table.AddCell(item.Item2 == "Expense" ? "" : item.Item3.ToString());
                }
                table.AddCell("Total");
                table.AddCell(new Cell(1, 2).Add(new Paragraph(totalString)));
                
                doc.Add(table);

                doc.Close();
            }
        }
    }
}
