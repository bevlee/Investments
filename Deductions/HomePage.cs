using System.Data.SQLite;
using LumenWorks.Framework.IO.Csv;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Properties;
using System.Globalization;
using System.Data;
namespace Deductions
{
    public partial class HomePage : Form
    {
        private string accountName;
        private string? selectedInvestment = null;
        private string financialYearString = "";
        private DateTime oldestTransactionDate = DateTime.UnixEpoch;
        private HashSet<string> allDates;
        private string[] mandatoryFields = ["Item", "TransactionType", "Amount", "Date"];
        private bool fyChanged = true;
        private DateTime fromDate = DateTime.UnixEpoch;
        private DateTime toDate = DateTime.Now;

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
                        TransactionId INTEGER PRIMARY KEY,
                        Item TEXT NOT NULL,
                        Date INTEGER NOT NULL,
                        LastModifiedDate INTEGER NOT NULL,
                        Value REAL NOT NULL,
                        TransactionType TEXT NOT NULL,
                        FinancialYear INTEGER NOT NULL,
                        InvestmentName TEXT NOT NULL,
                        Note TEXT NOT NULL,
                        Source TEXT NOT NULL,
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
                        INSERT INTO Transactions (Item, InvestmentName, Value, Date, LastModifiedDate, Note, TransactionType, FinancialYear, Source)
                        VALUES 
                            ('Rent', 'Test', 1500, 1706792400, $currentDate, '', 'Income', '2024', ''),
                            ('Rent', 'Test', 1500, 1707915600, $currentDate, '', 'Income', '2024', ''),
                            ('Cleaning Costs', 'Test', 950, 1712322000, $currentDate, 'End of lease cleaning', 'Expense', '2024', ''),
                            ('Water', 'Test', 122.11, 1712322000, $currentDate, '', 'Expense', '2024', ''),
                            ('Gas', 'Test', 57.29, 1712322000, $currentDate, '', 'Expense', '2024', ''),
                            ('Electricity', 'Test', 259.35, 1707955200, $currentDate, '', 'Expense', '2024', '');
                    ";
                    command.Parameters.AddWithValue("$currentDate", DateTimeOffset.Now.ToUnixTimeSeconds());
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

            investmentComboBox.DataSource = investmentsWithSelection;
            investmentComboBox.SelectedIndex = 0;
            selectedInvestment = investmentsWithSelection[0];

            DisplayTransactions();
        }

        private void DeleteTransactions(object sender, EventArgs e)
        {

            System.Diagnostics.Debug.WriteLine($" deleting row(s)!");
            if (TransactionDataGridView.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show($"Are you sure you wish to delete the selected {TransactionDataGridView.SelectedRows.Count} transactions?", "Delete Selected Rows", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    List<int?> transactionIds = new List<int?>();
                    List<Transaction> allTransactions = (List<Transaction>) TransactionDataGridView.DataSource;
                    foreach (DataGridViewRow row in TransactionDataGridView.SelectedRows)
                    {
                        int rowId = row.Index;
                        transactionIds.Add(allTransactions[rowId].getTransactionId());


                        //DateTime date = DateTime.Parse(row.Cells[0].Value.ToString());
                        //string Item = row.Cells[1].Value.ToString();
                        //decimal value = currencyToDecimal(row.Cells[2].Value.ToString());
                        //string transactionType = row.Cells[3].Value.ToString();
                        //int financialYear = int.Parse(row.Cells[4].Value.ToString());
                        //DateTime lastModifiedDate = DateTime.Parse(row.Cells[5].Value.ToString());
                        //string note = row.Cells[6].Value.ToString();
                        //string source = "";
                        //transactionList.Add(new Transaction(Item, date, lastModifiedDate, value, transactionType, financialYear, selectedInvestment, note, source, 1));

                    }
                    Database.DeleteTransactions(transactionIds);
                    LoadData();
                }
            }
        }
        private void DisplayTransactions()
        {
            //System.Diagnostics.Debug.WriteLine($" displaying transactions!");

            List<Transaction> allTransactions;
            if (fyChanged)
            {
                allTransactions = Database.LoadTransactions(selectedInvestment, financialYearString);
            }
            else
            {
                allTransactions = Database.LoadTransactions(selectedInvestment, fromDate, toDate);
            }

            decimal netValue = 0;
            allTransactions.ForEach(transaction =>
            {
                netValue += transaction.TransactionType == "Income" ? transaction.Amount : -transaction.Amount;
            });
            string netValueString = netValue < 0 ? "-$" + Math.Abs(netValue) : "$" + netValue;
            //List<TransactionDisplay> transactionDisplays = new List<TransactionDisplay>();
            //var transactionDisplays = allTransactions.Select(transaction => new TransactionDisplay(transaction)).ToList();
            TransactionDataGridView.DataSource = allTransactions;

            // reload the selected dates
            allDates = new HashSet<string>([""]);
            List<string> years = Database.getAllFinancialYears(selectedInvestment);
            oldestTransactionDate = Database.getOldestTransaction(selectedInvestment);
            //fromDatePicker.MinDate = oldestTransactionDate;
            fromDate = fromDatePicker.Value < DateTime.UnixEpoch ? oldestTransactionDate : fromDatePicker.Value;
            fromDatePicker.Value = fromDate;
            //toDatePicker.MinDate = oldestTransactionDate;
            foreach (string year in years)
            {
                allDates.Add(year);
            }
            FinancialYearComboBox.DataSource = new List<string>(allDates);
            FinancialYearComboBox.SelectedItem = financialYearString;

            fyChanged = false;
            NetValueLabel.Text = $"The net value for investment {selectedInvestment} is {netValueString}";
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


                financialYearString = "";
                LoadData();
            }
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
            string Item;
            string TransactionType;
            DateTime date;
            DateTime lastModifiedDate = DateTime.Now;
            decimal amount;
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
                        //csv.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;
                        int fieldCount = csv.FieldCount;
                        string[] headers = csv.GetFieldHeaders();
                        if (mandatoryFields.All(headers.Contains))
                        {
                            while (csv.ReadNextRecord())
                            {
                                Item = csv[3];
                                TransactionType = csv[1];
                                date = DateTime.Parse(csv[0]);
                                amount = decimal.Parse(csv[2]);
                                financialYear = Database.ToFinancialYear(date);
                                if (headers.Length > 4 && headers[4] == "note")
                                {
                                    note = csv[4];
                                }
                                transaction = new Transaction(Item, date, lastModifiedDate, amount, TransactionType, financialYear, investmentName, note, source, 1);
                                //System.Diagnostics.Debug.WriteLine($" \"{headers[i]} = {csv[i]}\",\r\n");
                                transactions.Add(transaction);

                                System.Diagnostics.Debug.WriteLine(transaction.ToString());
                            }
                        }
                        else
                        {
                            throw new Exception("mandatory fields not present in csv file");
                        }

                    }
                    Database.UpsertTransactions(transactions);
                    LoadData();

                }
                catch (Exception ex)
                {
                    if (ex.Message == "mandatory fields not present in csv file")
                    {
                        MessageBox.Show(ex.Message + ". Please ensure the format is Date, TransactionType, Amount, Item, Notes (optional)");
                    }
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        private void generateReportButton_Click(object sender, EventArgs e)
        {
            List<Tuple<string, string, decimal>> ItemSummary = Database.getSummary(accountName, selectedInvestment, financialYearString, fromDate, toDate);
            //System.Diagnostics.Debug.WriteLine($" \"{headers[i]} = {csv[i]}\",\r\n");
            List<Tuple<string, decimal>> expenses = new List<Tuple<string, decimal>>();
            List<Tuple<string, decimal>> income = new List<Tuple<string, decimal>>();
            foreach (Tuple<string, string, decimal> Item in ItemSummary)
            {
                System.Diagnostics.Debug.WriteLine($" \"{Item.Item1} = {Item.Item3}\",\r\n");
                if (Item.Item2 == "Expense")
                {
                    expenses.Add(new Tuple<string, decimal>(Item.Item1, Item.Item3));
                }
                else
                {
                    income.Add(new Tuple<string, decimal>(Item.Item1, Item.Item3));
                }
            }
            decimal incomeTotal = income.Sum(x => x.Item2);
            decimal expensesTotal = expenses.Sum(x => x.Item2);
            decimal total = Math.Round(incomeTotal - expensesTotal, 2, MidpointRounding.AwayFromZero);
            string totalString = total > 0 ? "$" + total : "-$" + total * -1;

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
                .SetFontColor(ColorConstants.BLUE)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                Paragraph title = new Paragraph($"Summary for {selectedInvestment}").AddStyle(titleStyle);
                string timePeriod = financialYearString == "" ? $"{fromDate.Date.ToShortDateString()} to {toDate.Date.ToShortDateString()}" : $"Financial year {financialYearString}";
                Paragraph year = new Paragraph(timePeriod).AddStyle(titleStyle);

                doc.Add(title);
                doc.Add(year);

                Table expenseTable = new Table(UnitValue.CreatePercentArray(new float[] { 80, 10, 10 }));
                expenseTable.SetMarginTop(5);
                expenseTable.AddCell("Expense Item");
                expenseTable.AddCell("Expense");
                expenseTable.AddCell("Income");
                foreach (Tuple<string, decimal> Item in expenses)
                {
                    expenseTable.AddCell(Item.Item1);
                    expenseTable.AddCell(Item.Item2.ToString());
                    expenseTable.AddCell("");
                }
                expenseTable.AddCell(new Paragraph("Expenses Total").SetBold());
                expenseTable.AddCell(new Cell(1, 2).Add(new Paragraph("$" + expensesTotal.ToString()).SetBold()));

                doc.Add(expenseTable);
                Table incomeTable = new Table(UnitValue.CreatePercentArray(new float[] { 80, 10, 10 }));
                incomeTable.SetMarginTop(5);
                incomeTable.AddCell("Income Item");
                incomeTable.AddCell("Expense");
                incomeTable.AddCell("Income");
                foreach (Tuple<string, decimal> Item in income)
                {
                    incomeTable.AddCell(Item.Item1);
                    incomeTable.AddCell("");
                    incomeTable.AddCell(Item.Item2.ToString());
                }
                incomeTable.AddCell(new Paragraph("Income Total").SetBold());
                incomeTable.AddCell(new Cell(1, 2).Add(new Paragraph("$" + incomeTotal.ToString()).SetBold()));

                doc.Add(incomeTable);

                Table summaryTable = new Table(UnitValue.CreatePercentArray(new float[] { 80, 10, 10 }));
                summaryTable.SetMarginTop(5);
                summaryTable.AddCell("Net Income");
                summaryTable.AddCell("Expense");
                summaryTable.AddCell("Income");

                summaryTable.AddCell("Expenses");
                summaryTable.AddCell(expensesTotal.ToString());
                summaryTable.AddCell("");
                summaryTable.AddCell("Income");
                summaryTable.AddCell("");
                summaryTable.AddCell(incomeTotal.ToString());
                summaryTable.AddCell(new Paragraph("Total").SetBold());
                summaryTable.AddCell(new Cell(1, 2).Add(new Paragraph(totalString)).SetBold());
                doc.Add(summaryTable);
                doc.Close();
            }
        }

        private void FromDatePicker_SelectionChanged(object sender, EventArgs e)
        {
            if (fromDatePicker.Value.Date != fromDate.Date && !fyChanged)
            {
                FinancialYearComboBox.SelectedItem = "N/A";
                FinancialYearComboBox.Text = "N/A";
                financialYearString = "N/A";
                if (toDatePicker.Value < fromDatePicker.Value)
                {
                    toDatePicker.Value = fromDatePicker.Value;
                }
                fromDate = fromDatePicker.Value;
                LoadData();
            }

        }

        private void ToDatePicker_SelectionChanged(object sender, EventArgs e)
        {
            if (toDatePicker.Value.Date != toDate.Date && !fyChanged)
            {
                FinancialYearComboBox.SelectedItem = "N/A";
                FinancialYearComboBox.Text = "N/A";
                financialYearString = "N/A";
                if (fromDatePicker.Value > toDatePicker.Value)
                {
                    fromDatePicker.Value = toDatePicker.Value;
                }
                toDate = toDatePicker.Value;
                LoadData();
            }
        }
        private void financialYearComboBox_SelectionChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Selected value changed to: " + investmentComboBox.SelectedItem.ToString());
            string fy = FinancialYearComboBox.SelectedItem.ToString();
            if (fy != financialYearString)
            {

                fyChanged = true;
                financialYearString = fy;

                Tuple<DateTime, DateTime> bounds = GetDateBoundsByFinancialYear(financialYearString);
                fromDate = bounds.Item1;
                fromDatePicker.Value = fromDate;
                toDate = bounds.Item2;
                toDatePicker.Value = toDate;
                LoadData();
            }
        }

        public Tuple<DateTime, DateTime> GetDateBoundsByFinancialYear(string financialYear)
        {
            DateTime start = oldestTransactionDate;
            DateTime end = DateTime.Now;
            if (financialYear != "")
            {
                int fy = int.Parse(financialYear);
                start = DateTime.ParseExact($"{fy - 1}-07-01", "yyyy-MM-dd", CultureInfo.CurrentCulture);
                end = DateTime.ParseExact($"{fy}-06-30", "yyyy-MM-dd", CultureInfo.CurrentCulture);
            }
            return new Tuple<DateTime, DateTime>(start, end);
        }

        private void resetDatesButton_Click(object sender, EventArgs e)
        {
            fyChanged = true;
            financialYearString = "";
            fromDatePicker.Value = oldestTransactionDate;
            toDatePicker.Value = DateTime.Now;
            LoadData();
        }

        public void TransactionDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            if (e.RowIndex > -1)
            {
                List<Transaction> dt = (List<Transaction>)TransactionDataGridView.DataSource;
                Transaction row = dt[e.RowIndex];

                DateTime date = row.Date;
                string Item = row.Item;
                decimal amount = row.Amount;
                string transactionType = row.TransactionType;
                int financialYear = row.FinancialYear;
                DateTime lastModifiedDate = row.LastModifiedDate;
                string note = row.Note;
                string source = row.Source;
                int? transactionId = row.getTransactionId();
                Transaction selectedTransaction = new Transaction(Item, date, lastModifiedDate, amount, transactionType, financialYear, selectedInvestment, note, source, transactionId);
                
                CreateTransaction editTransactionForm = new CreateTransaction(selectedTransaction);
                if (editTransactionForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        public decimal currencyToDecimal(string currency)
        {
            return decimal.Parse(currency.Split("$")[1]);
        }
    }
}
