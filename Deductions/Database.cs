using System.Data.SQLite;

namespace Deductions
{
    internal class Database
    {
        public static SQLiteConnection CreateConnection()
        {
            string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Deductions");
            var db_location = Path.Combine(appDir, "Deductions.db");
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=" + db_location + "; Version = 3; FailIfMissing=True");
             // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("exception found" + ex);
            }
            return sqlite_conn;
        }
        
        public static List<String> getAllInvestments()
        {
            List<String> investments = new List<String>();

            using (SQLiteConnection conn = CreateConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"
                    SELECT InvestmentName
                    FROM Investments;
                    ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var investment = reader.GetString(0);
                        investments.Add(investment);
                    }
                }

            }
            return investments;
        }

        public static List<String> getInvestments(string accountName)
        {
            List<String> investments = new List<String>();

            using (SQLiteConnection conn = CreateConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"
                    SELECT InvestmentName
                    FROM Investments
                    WHERE Accountname = @accountName;
                    ";
                command.Parameters.AddWithValue(@accountName, accountName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var investment = reader.GetString(0);
                        investments.Add(investment);
                    }
                }
            }
            return investments;
        }

        internal static List<Transaction> LoadTransactions(string investmentName, string financialYear)
        {
            
            List<Transaction> transactions = new List<Transaction>();
            using (SQLiteConnection conn = CreateConnection())
            {
                var command = conn.CreateCommand();
                int fy;
                System.Diagnostics.Debug.WriteLine($" loading transactions");
                if (!int.TryParse(financialYear, out fy))
                {

                    System.Diagnostics.Debug.WriteLine($" fy is not set");
                    command.CommandText =
                   @"
                    SELECT Category, Date, LastModifiedDate, Value, TransactionType, FinancialYear, Notes, SourceId
                    FROM Transactions
                    WHERE InvestmentName = @investmentName;
                    ";
                } else
                {
                    System.Diagnostics.Debug.WriteLine($" fy is  set to {fy}");
                    command.CommandText =
                   @"
                    SELECT Category, Date, LastModifiedDate, Value, TransactionType, FinancialYear, Notes, SourceId
                    FROM Transactions
                    WHERE InvestmentName = @investmentName
                    AND FinancialYear = @financialYear;
                    ";
                   command.Parameters.AddWithValue("@financialYear", financialYear);
                }

                command.Parameters.AddWithValue("@investmentName", investmentName);


                using (var reader = command.ExecuteReader())
                {
                    Transaction? transaction = null;
                    while (reader.Read())
                    {

                        string Category = reader.GetString(0);
                        DateTime date = UnixTimeStampToDateTime(reader.GetInt64(1));
                        DateTime lastModifiedDate = UnixTimeStampToDateTime(reader.GetInt64(2));
                        double value = reader.GetDouble(3);
                        string transactionType = reader.GetString(4);
                        fy = reader.GetInt32(5);
                        string notes = reader.GetString(6);
                        string source = reader.GetString(7);

                        transaction = new Transaction(Category, date, DateTime.UtcNow, value, transactionType, fy, investmentName, notes, source);
                        transactions.Add(transaction);

                    }
                }

            }
            return transactions;
        }
        internal static List<string> GetAccounts()
        {
            List<string> accounts = new List<string>();
            using (SQLiteConnection conn = CreateConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"
                    SELECT AccountName
                    FROM Accounts;
                    ";
                using (var reader = command.ExecuteReader())
                {
                    Transaction? transaction = null;
                    while (reader.Read())
                    {
                        accounts.Add(reader.GetString(0));
                        //transaction = reader.GetString(0);

                        //System.Diagnostics.Debug.WriteLine($"Hello, {name}!");

                    }
                }

            }
            return accounts;
        }

        internal static void CreateNewInvestment(string investmentName, string accountName)
            {
                using (SQLiteConnection conn = CreateConnection())
                {

                    var createCommand = conn.CreateCommand();
                    createCommand.CommandText =
                        @"
                            INSERT INTO Investments (InvestmentName, AccountName)
                            VALUES ($investmentName, $accountName)
                        ";
                    createCommand.Parameters.AddWithValue("$investmentName", investmentName);
                    createCommand.Parameters.AddWithValue("$accountName", accountName);

                    createCommand.ExecuteNonQuery();

                 return;
            }
        }

        internal static void CreateNewTransaction(Transaction transaction)
        {   

            using (SQLiteConnection conn = CreateConnection())
            {

                var createCommand = conn.CreateCommand();
                createCommand.CommandText =
                    @"
                        INSERT INTO Transactions (Category, InvestmentName, Value, Date, TransactionType, FinancialYear)
                        VALUES (@Category, @investmentName, @value, @Date, @transactionType, @FinancialYear);
                    ";
                createCommand.Parameters.AddWithValue("@Category", transaction.category);
                createCommand.Parameters.AddWithValue("@investmentName", transaction.investmentName);
                createCommand.Parameters.AddWithValue("@value", transaction.amount);
                createCommand.Parameters.AddWithValue("@Date", ((DateTimeOffset)transaction.date).ToUnixTimeSeconds());
                createCommand.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
                createCommand.Parameters.AddWithValue("@FinancialYear", ToFinancialYear(transaction.date));

                createCommand.ExecuteNonQuery();

                return;
            }
        }
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static int ToFinancialYear(DateTime dateTime)
        {
            return dateTime.Month >= 7 ? dateTime.Year + 1 : dateTime.Year;
        }

        public static void DeleteTransactions(List<Transaction> transactionList)
        {
            using (SQLiteConnection conn = CreateConnection())
            {
                var createCommand = conn.CreateCommand();
                foreach (Transaction transaction in transactionList)
                {
                    createCommand.CommandText =
                    @"
                        DELETE FROM Transactions 
                        WHERE Category = @Category
                        AND InvestmentName = @investmentName
                        AND Value = @value
                        AND Date = @Date
                        AND TransactionType = @transactionType;
                    ";
                    createCommand.Parameters.AddWithValue("@Category", transaction.category);
                    createCommand.Parameters.AddWithValue("@investmentName", transaction.investmentName);
                    createCommand.Parameters.AddWithValue("@value", transaction.amount);
                    createCommand.Parameters.AddWithValue("@Date", ((DateTimeOffset)transaction.date).ToUnixTimeSeconds());
                    createCommand.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
                    createCommand.Parameters.AddWithValue("@FinancialYear", ToFinancialYear(transaction.date));
                    int changed = createCommand.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine($"{changed}!");
                }

                return;
            }
        }
    }
}
