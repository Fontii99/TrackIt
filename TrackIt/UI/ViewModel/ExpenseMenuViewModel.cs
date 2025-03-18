using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using TrackIt.DataLayer.MySQL;
using TrackIt.DataLayer.Repositories;
using TrackIt.UI.Model;

namespace TrackIt.UI.ViewModel
{
    public class ExpenseMenuViewModel : INotifyPropertyChanged
    {
        private readonly IMySQLConnection _connection;
        private readonly IEntryRepository _entryRepository;

        private string _description;
        private string _amount;
        private string _selectedCategory;
        private bool _isRecurring;
        private DateTime? _startDate = DateTime.Today;
        private DateTime? _endDate;
        private string _selectedPeriod;

        // Properties with INotifyPropertyChanged
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public string Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public bool IsRecurring
        {
            get => _isRecurring;
            set
            {
                if (_isRecurring != value)
                {
                    _isRecurring = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public string SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                if (_selectedPeriod != value)
                {
                    _selectedPeriod = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        // Categories list for binding
        public List<string> Categories { get; } = new List<string> { "Food", "Transport", "Entertainment", "Bills", "Add..." };
        // Periods list for binding
        public List<string> Periods { get; } = new List<string> { "Day", "Week", "1 Month", "3 Months", "6 Months", "Year" };
        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor that accepts a connection for dependency injection
        public ExpenseMenuViewModel(IMySQLConnection connection, IEntryRepository entryRepository)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _entryRepository = entryRepository ?? throw new ArgumentNullException(nameof(entryRepository));
            SaveCommand = new RelayCommand(ExecuteSave, CanSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        // Default constructor that creates its own connection
        public ExpenseMenuViewModel()
        {
            // Create configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Create connection
            _connection = new MySQLConnection(configuration);
            _entryRepository = new EntryRepository(_connection);

            SaveCommand = new RelayCommand(ExecuteSave, CanSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private bool CanSave()
        {
            // Basic validation
            return !string.IsNullOrWhiteSpace(Description) &&
                   !string.IsNullOrWhiteSpace(Amount) &&
                   !string.IsNullOrWhiteSpace(SelectedCategory) &&
                   StartDate.HasValue &&
                   (!IsRecurring || (IsRecurring && !string.IsNullOrEmpty(SelectedPeriod)));
        }

        private void ExecuteSave()
        {
            // Parse amount as int (you might want to add validation)
            if (int.TryParse(Amount, out int amountValue))
            {
                try
                {
                    // Create a new expense entry
                    var entry = new Entry
                    {
                        Type = true, // Expense
                        Description = Description,
                        Amount = amountValue,
                        Date = StartDate.Value,
                        Category = SelectedCategory,
                        Period = IsRecurring ? SelectedPeriod : null
                    };

                    // Save to the database directly
                    bool success = _entryRepository.AddEntry(entry);

                    if (success)
                    {
                        // Close the window
                        RequestClose?.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save expense entry.",
                            "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid amount.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteCancel()
        {
            RequestClose?.Invoke();
        }

        // Event to close the window
        public event Action RequestClose;

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}