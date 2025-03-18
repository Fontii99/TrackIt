using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TrackIt.DataLayer.MySQL;
using TrackIt.DataLayer.Repositories;
namespace TrackIt.UI.ViewModel
{
    public class SummaryMenuViewModel : INotifyPropertyChanged
    {
        private readonly IMySQLConnection _connection;
        private readonly IEntryRepository _entryRepository;
        private ObservableCollection<EntrySummary> _entries;
        private decimal _totalIncome;
        private decimal _totalExpense;
        private decimal _balance;
        private bool _isLoading;

        public ObservableCollection<EntrySummary> Entries
        {
            get => _entries;
            set
            {
                if (_entries != value)
                {
                    _entries = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalIncome
        {
            get => _totalIncome;
            set
            {
                if (_totalIncome != value)
                {
                    _totalIncome = value;
                    OnPropertyChanged();
                    // Balance is dependent on TotalIncome, so update it
                    Balance = TotalIncome - TotalExpense;
                }
            }
        }

        public decimal TotalExpense
        {
            get => _totalExpense;
            set
            {
                if (_totalExpense != value)
                {
                    _totalExpense = value;
                    OnPropertyChanged();
                    // Balance is dependent on TotalExpense, so update it
                    Balance = TotalIncome - TotalExpense;
                }
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RefreshCommand { get; }

        // Constructor that accepts a connection for dependency injection
        public SummaryMenuViewModel(IMySQLConnection connection, IEntryRepository entryRepository)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _entryRepository = entryRepository ?? throw new ArgumentNullException(nameof(entryRepository));
            Entries = new ObservableCollection<EntrySummary>();
            RefreshCommand = new RelayCommand(ExecuteRefresh);

            // Load data when created
            LoadData();
        }

        // Default constructor that creates its own connection
        public SummaryMenuViewModel()
        {
            // Create configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Create connection
            _connection = new MySQLConnection(configuration);

            // Create repository
            _entryRepository = new EntryRepository(_connection);

            Entries = new ObservableCollection<EntrySummary>();
            RefreshCommand = new RelayCommand(ExecuteRefresh);

            // Load data when created
            LoadData();
        }

        private void ExecuteRefresh()
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                IsLoading = true;
                Entries.Clear();

                // Get entries from repository
                var entriesFromDb = _entryRepository.GetAllEntries();

                // Transform to view model objects (EntrySummary)
                var entrySummaries = entriesFromDb.Select(e => new EntrySummary
                {
                    Type = e.Type,
                    Description = e.Description,
                    Amount = e.Amount,
                    Date = e.Date,
                    Category = e.Category,
                    Period = e.Period,
                    DisplayAmount = e.Type ? $"-${e.Amount:N2}" : $"+${e.Amount:N2}",
                    AmountColor = e.Type ? "Red" : "Green",
                    ActualAmount = e.Type ? -e.Amount : e.Amount
                }).ToList();

                // Calculate running balance (this stays in the view model as it's presentation logic)
                decimal totalBalance = entrySummaries.Sum(e => e.ActualAmount);

                // Sort by date (newest first)
                var sortedEntries = entrySummaries.OrderByDescending(e => e.Date).ThenByDescending(e => e.Id).ToList();

                // Add sorted entries and calculate running balance
                decimal runningBalance = totalBalance;

                foreach (var entry in sortedEntries)
                {
                    entry.RunningBalance = runningBalance;
                    entry.DisplayRunningBalance = runningBalance >= 0 ?
                        $"+${runningBalance:N2}" :
                        $"-${Math.Abs(runningBalance):N2}";
                    entry.RunningBalanceColor = runningBalance >= 0 ? "Green" : "Red";

                    runningBalance -= entry.ActualAmount;
                    Entries.Add(entry);
                }

                // Calculate totals
                CalculateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}",
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private void CalculateTotals()
        {
            TotalIncome = Entries.Where(e => !e.Type).Sum(e => e.Amount);
            TotalExpense = Entries.Where(e => e.Type).Sum(e => e.Amount);
            // Balance is automatically calculated via property setters
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Class to represent an entry in the summary view
    public class EntrySummary
    {
        public int Id { get; set; }
        public bool Type { get; set; } // False = Income, True = Expense
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Period { get; set; }

        // Display properties for the UI
        public string DisplayAmount { get; set; }
        public string AmountColor { get; set; }

        // Running balance properties
        public decimal ActualAmount { get; set; } // Positive for income, negative for expense
        public decimal RunningBalance { get; set; }
        public string DisplayRunningBalance { get; set; }
        public string RunningBalanceColor { get; set; }
    }
}