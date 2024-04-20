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
        public static List<string> getAllFinancialYears(string investmentName)
        {
            HashSet<string> years = new HashSet<string>();

            using (SQLiteConnection conn = CreateConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"
                    SELECT FinancialYear
                    FROM Transactions
                    WHERE InvestmentName=@investmentName
                    ORDER BY FinancialYear;
                    ";
                command.Parameters.AddWithValue("@investmentName", investmentName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var year = reader.GetInt16(0);
                        years.Add(year.ToString());
                    }
                }
            }
            return new List<string>(years);
        }
        public static DateTime getOldestTransaction(string investmentName)
        {
            DateTime oldestTransactionDate = DateTime.UnixEpoch;

            using (SQLiteConnection conn = CreateConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"
                    SELECT Date
                    FROM Transactions
                    WHERE InvestmentName=@investmentName
                    ORDER BY Date ASC LIMIT 1;
                    ";
                command.Parameters.AddWithValue("@investmentName", investmentName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oldestTransactionDate = UnixTimeStampToDateTime(reader.GetInt64(0));
                    }
                }
            }
            return oldestTransactionDate;
        }
        public static List<(string, string)> getAllInvestmentsAndAccounts()
        {
            List<(string, string)> investments = new List<(string, string)>();

            using (SQLiteConnection conn = CreateConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"
                    SELECT InvestmentName, AccountName
                    FROM Investments;
                    ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var investment = reader.GetString(0);
                        var accountName = reader.GetString(1);
                        investments.Add((investment, accountName));
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
                    SELECT Item, Date, LastModifiedDate, Value, TransactionType, FinancialYear, Note, Source, TransactionId
                    FROM Transactions
                    WHERE InvestmentName = @investmentName;
                    ";
                } else
                {
                    System.Diagnostics.Debug.WriteLine($" fy is  set to {fy}");
                    command.CommandText =
                   @"
                    SELECT Item, Date, LastModifiedDate, Value, TransactionType, FinancialYear, Note, Source, TransactionId
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

                        string item = reader.GetString(0);
                        DateTime date = UnixTimeStampToDateTime(reader.GetInt64(1));
                        DateTime lastModifiedDate = UnixTimeStampToDateTime(reader.GetInt64(2));
                        decimal value = reader.GetDecimal(3);
                        string transactionType = reader.GetString(4);
                        fy = reader.GetInt32(5);
                        string note = reader.GetString(6);
                        string source = reader.GetString(7);
                        int transactionId = reader.GetInt32(8);

                        transaction = new Transaction(item, date, lastModifiedDate, value, transactionType, fy, investmentName, note, source, transactionId);
                        transactions.Add(transaction);

                    }
                }

            }
            return transactions;
        }

        internal static List<Transaction> LoadTransactions(string investmentName, DateTime startDate, DateTime endDate)
        {

            List<Transaction> transactions = new List<Transaction>();
            using (SQLiteConnection conn = CreateConnection())
            {
                var command = conn.CreateCommand();
                int fy;
                System.Diagnostics.Debug.WriteLine($" loading transactions");
               
                System.Diagnostics.Debug.WriteLine($" start date {startDate} to end date {endDate} ");
                command.CommandText =
                @"
                SELECT Item, Date, LastModifiedDate, Value, TransactionType, FinancialYear, Note, Source, TransactionId
                FROM Transactions
                WHERE InvestmentName = @investmentName
                AND @startDate <= Date
                AND @endDate >= Date;
                ";
                command.Parameters.AddWithValue("@startDate", ((DateTimeOffset) startDate).ToUnixTimeSeconds());
                command.Parameters.AddWithValue("@endDate", ((DateTimeOffset) endDate).ToUnixTimeSeconds());
                command.Parameters.AddWithValue("@investmentName", investmentName);

                using (var reader = command.ExecuteReader())
                {
                    Transaction? transaction = null;
                    while (reader.Read())
                    {
                        string item = reader.GetString(0);
                        DateTime date = UnixTimeStampToDateTime(reader.GetInt64(1));
                        DateTime lastModifiedDate = UnixTimeStampToDateTime(reader.GetInt64(2));
                        decimal value = reader.GetDecimal(3);
                        string transactionType = reader.GetString(4);
                        fy = reader.GetInt32(5);
                        string note = reader.GetString(6);
                        string source = reader.GetString(7);
                        int transactionId = reader.GetInt32(8);

                        transaction = new Transaction(item, date, lastModifiedDate, value, transactionType, fy, investmentName, note, source, transactionId);
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

        internal static void UpsertTransactions(List<Transaction> transactions)
        {
            using (SQLiteConnection conn = CreateConnection())
            {
                foreach (Transaction transaction in transactions)
                {
                    var createCommand = conn.CreateCommand();
                    createCommand.CommandText =
                        @"
                        INSERT INTO Transactions (Item, InvestmentName, Value, Date, LastModifiedDate, TransactionType, FinancialYear, Note, Source)
                        VALUES (@Item, @investmentName, @value, @Date, @LastModifiedDate, @transactionType, @FinancialYear, @note, '');
                    ";
                    createCommand.Parameters.AddWithValue("@Item", transaction.Item);
                    createCommand.Parameters.AddWithValue("@investmentName", transaction.getInvestmentName());
                    createCommand.Parameters.AddWithValue("@value", transaction.Amount);
                    createCommand.Parameters.AddWithValue("@Date", ((DateTimeOffset)transaction.Date).ToUnixTimeSeconds());
                    createCommand.Parameters.AddWithValue("@LastModifiedDate", ((DateTimeOffset)transaction.LastModifiedDate).ToUnixTimeSeconds());
                    createCommand.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
                    createCommand.Parameters.AddWithValue("@note", transaction.Note);
                    createCommand.Parameters.AddWithValue("@FinancialYear", ToFinancialYear(transaction.Date));

                    createCommand.ExecuteNonQuery();
                }

                return;
            }
        }

        internal static bool UpsertTransaction(Transaction transaction)
        {
            try
            {
                using (SQLiteConnection conn = CreateConnection())
                {
                    var createCommand = conn.CreateCommand();
                    createCommand.CommandText =
                        @"
                        UPDATE Transactions 
                        SET 
                            Value=@value,
                            LastModifiedDate=@lastModifiedDate, 
                            TransactionType=@transactionType, 
                            Note=@note,         
                            Source='',
                            Item = @item,
                            InvestmentName = @investmentName,
                            Date=@date,
                            FinancialYear=@financialYear
                        WHERE 
                            TransactionId = @transactionId;
                    ";
                    createCommand.Parameters.AddWithValue("@item", transaction.Item);
                    createCommand.Parameters.AddWithValue("@investmentName", transaction.getInvestmentName());
                    createCommand.Parameters.AddWithValue("@value", transaction.Amount);
                    createCommand.Parameters.AddWithValue("@date", ((DateTimeOffset)transaction.Date).ToUnixTimeSeconds());
                    createCommand.Parameters.AddWithValue("@lastModifiedDate", ((DateTimeOffset)transaction.LastModifiedDate).ToUnixTimeSeconds());
                    createCommand.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
                    createCommand.Parameters.AddWithValue("@note", transaction.Note);
                    createCommand.Parameters.AddWithValue("@financialYear", ToFinancialYear(transaction.Date));
                    createCommand.Parameters.AddWithValue("@transactionId", transaction.getTransactionId());


                    int updatedRows = createCommand.ExecuteNonQuery();
                    if (updatedRows == 0)
                    {
                        createCommand.CommandText =
                        @"
                        INSERT INTO Transactions (Item, InvestmentName, Value, Date, LastModifiedDate, TransactionType, FinancialYear, Note, Source)
                        VALUES (@Item, @investmentName, @value, @Date, @LastModifiedDate, @transactionType, @FinancialYear, @note, '');
                    ";
                        createCommand.Parameters.AddWithValue("@Item", transaction.Item);
                        createCommand.Parameters.AddWithValue("@investmentName", transaction.getInvestmentName());
                        createCommand.Parameters.AddWithValue("@value", transaction.Amount);
                        createCommand.Parameters.AddWithValue("@Date", ((DateTimeOffset)transaction.Date).ToUnixTimeSeconds());
                        createCommand.Parameters.AddWithValue("@LastModifiedDate", ((DateTimeOffset)transaction.LastModifiedDate).ToUnixTimeSeconds());
                        createCommand.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
                        createCommand.Parameters.AddWithValue("@note", transaction.Note);
                        createCommand.Parameters.AddWithValue("@FinancialYear", ToFinancialYear(transaction.Date));

                        createCommand.ExecuteNonQuery();
                    }
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString() + Environment.NewLine + e.StackTrace);
                return false;
            }

            return true;
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
                        WHERE Item = @Item
                        AND InvestmentName = @investmentName
                        AND Value = @value
                        AND Date = @Date
                        AND TransactionType = @transactionType;
                    ";
                    createCommand.Parameters.AddWithValue("@Item", transaction.Item);
                    createCommand.Parameters.AddWithValue("@investmentName", transaction.getInvestmentName());
                    createCommand.Parameters.AddWithValue("@value", transaction.Amount);
                    createCommand.Parameters.AddWithValue("@Date", ((DateTimeOffset)transaction.Date).ToUnixTimeSeconds());
                    createCommand.Parameters.AddWithValue("@transactionType", transaction.TransactionType);
                    createCommand.Parameters.AddWithValue("@FinancialYear", ToFinancialYear(transaction.Date));
                    int changed = createCommand.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine($"{changed}!");
                }

                return;
            }
        }

        public static void DeleteTransactions(List<int?> ids)
        {
            using (SQLiteConnection conn = CreateConnection())
            {
                var createCommand = conn.CreateCommand();
                foreach (int? id in ids)
                {
                    createCommand.CommandText =
                    @"
                        DELETE FROM Transactions 
                        WHERE TransactionId = @id;
                    ";
                    createCommand.Parameters.AddWithValue("@id", id);
                    int changed = createCommand.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine($"{changed}!");
                }

                return;
            }
        }
        public static void DeleteInvestment(string investmentName)
        {
            using (SQLiteConnection conn = CreateConnection())
            {
                var createCommand = conn.CreateCommand();
                createCommand.CommandText =
                @"
                    DELETE FROM Investments 
                    WHERE InvestmentName=@investmentName;
                ";
                createCommand.Parameters.AddWithValue("@investmentName", investmentName);
                int changed = createCommand.ExecuteNonQuery();     
                if (changed > 0 )
                {
                    // delete the transactions related to the investment
                    createCommand.CommandText =
                    @"
                        DELETE FROM Transactions
                        WHERE InvestmentName=@investmentName;
                    ";
                    createCommand.ExecuteNonQuery();
                }

                return;
            }
        }

        public static List<Tuple<string, string, decimal>> getSummary(string accountName, string investmentName, string financialYear, DateTime start, DateTime end)
        {
            List<Tuple<string, string, decimal>> ItemSummary = new List<Tuple<string, string, decimal>>();

            using (SQLiteConnection conn = CreateConnection())
            {

                var command = conn.CreateCommand();

                int fy;
                System.Diagnostics.Debug.WriteLine($" loading transactions");
                if (!int.TryParse(financialYear, out fy))
                {
                    command.CommandText =
                    @"
                        SELECT Item, TransactionType, sum(value)
                        from transactions
                        WHERE InvestmentName=@investmentName
                        group by Item;
                    ";
                    command.Parameters.AddWithValue("@investmentName", investmentName);
                } else
                {
                    command.CommandText =
                    @"
                        SELECT Item, TransactionType, sum(value)
                        from transactions
                        WHERE InvestmentName=@investmentName
                        AND FinancialYear = @fy
                        AND @startDate <= Date
                        AND @endDate >= Date;
                        group by Item;
                    ";
                    command.Parameters.AddWithValue("@investmentName", investmentName);
                    command.Parameters.AddWithValue("@fy", fy);
                    command.Parameters.AddWithValue("@startDate", start);
                    command.Parameters.AddWithValue("@endDate", end);

                }
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Item = reader.GetString(0);
                        string transactionType = reader.GetString(1);
                        decimal value = reader.GetDecimal(2);

                        ItemSummary.Add(new Tuple<string, string, decimal>(Item, transactionType, value));

                    }
                }
            }
            return ItemSummary;
        }
    }
}
