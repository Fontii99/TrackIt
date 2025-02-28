using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrackIt.UI.Model;

namespace TrackIt.UI.ViewModel
{
    public class IncomeMenuViewModel : INotifyPropertyChanged
    {
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
        public List<string> Categories { get; } = new List<string> { "Salary", "Investment", "Others" , "Add..."};

        // Periods list for binding
        public List<string> Periods { get; } = new List<string> { "Day", "1 Month", "3 Months", "6 Months", "Year" };

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public IncomeMenuViewModel()
        {
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
                // Create a new income entry
                var entry = new Entry
                {
                    Type = false, // Income
                    Description = Description,
                    Amount = amountValue,
                    Date = StartDate.Value,
                    Category = SelectedCategory,
                    Period = IsRecurring ? SelectedPeriod : null
                };

                // TODO: Save entry to data store

                // Close the window - will be handled by the view
                RequestClose?.Invoke();
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