using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TrackIt.DataLayer.MySQL;
using TrackIt.UI.Model;

namespace TrackIt.DataLayer.Repositories
{
    public interface IEntryRepository
    {
        // Query methods
        List<Entry> GetAllEntries();
        List<Entry> GetEntriesByDateRange(DateTime startDate, DateTime endDate);
        List<Entry> GetEntriesByType(bool isExpense);
        Entry GetEntryById(int id);

        // Statistics methods
        decimal GetTotalIncome();
        decimal GetTotalExpense();
        decimal GetBalance();

        // CRUD operations
        bool AddEntry(Entry entry);
        bool UpdateEntry(Entry entry);
        bool DeleteEntry(int id);
    }

    public class EntryRepository : IEntryRepository
    {
        private readonly IMySQLConnection _connection;

        public EntryRepository(IMySQLConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public List<Entry> GetAllEntries()
        {
            var entries = new List<Entry>();

            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    connection.Open();

                    const string sql = @"SELECT Id, Type, Description, Amount, Date, Category, Period FROM Entry";

                    using (var command = new MySqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new Entry
                            {
                                Type = reader.GetBoolean("Type"),
                                Description = reader.GetString("Description"),
                                Amount = reader.GetDecimal("Amount"),
                                Date = reader.GetDateTime("Date"),
                                Category = reader.GetString("Category"),
                                Period = reader.IsDBNull(reader.GetOrdinal("Period")) ? null : reader.GetString("Period")
                            };

                            entries.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow it
                throw new Exception("Error retrieving entries from database", ex);
            }

            return entries;
        }

        public List<Entry> GetEntriesByDateRange(DateTime startDate, DateTime endDate)
        {
            // Implementation will go here
            throw new NotImplementedException();
        }

        public List<Entry> GetEntriesByType(bool isExpense)
        {
            // Implementation will go here
            throw new NotImplementedException();
        }

        public Entry GetEntryById(int id)
        {
            // Implementation will go here
            throw new NotImplementedException();
        }

        public decimal GetTotalIncome()
        {
            // Implementation will go here
            throw new NotImplementedException();
        }

        public decimal GetTotalExpense()
        {
            // Implementation will go here
            throw new NotImplementedException();
        }

        public decimal GetBalance()
        {
            // Implementation will go here
            throw new NotImplementedException();
        }

        public bool AddEntry(Entry entry)
        {
            const string sql = @"
                INSERT INTO Entry (Type, Description, Amount, Date, Category, Period) 
                VALUES (@Type, @Description, @Amount, @Date, @Category, @Period)";

            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Type", entry.Type);
                        command.Parameters.AddWithValue("@Description", entry.Description);
                        command.Parameters.AddWithValue("@Amount", entry.Amount);
                        command.Parameters.AddWithValue("@Date", entry.Date);
                        command.Parameters.AddWithValue("@Category", entry.Category);
                        command.Parameters.AddWithValue("@Period", entry.Period ?? (object)DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving entry: {ex.Message}");
                throw; // Rethrow to be handled by caller
            }
        }

        public bool UpdateEntry(Entry entry)
        {
            // Implementation will go here
            throw new NotImplementedException();
        }

        public bool DeleteEntry(int id)
        {
            // Implementation will go here
            throw new NotImplementedException();
        }
    }
}