using System.Windows;
using System.Windows.Controls;
using TrackIt.UI.ViewModel;

namespace TrackIt
{
    public partial class SummaryMenu : Window
    {
        private readonly SummaryMenuViewModel _viewModel;

        public SummaryMenu()
        {
            InitializeComponent();

            // Create the ViewModel
            _viewModel = new SummaryMenuViewModel();

            // Set DataContext
            DataContext = _viewModel;

            // Bind the DataGrid to the Entries collection
            DataGridSummary.ItemsSource = _viewModel.Entries;

            // Bind the summary text blocks
            TotalIncomeText.DataContext = _viewModel;
            TotalIncomeText.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("TotalIncome")
            {
                StringFormat = "${0:N2}"
            });

            TotalExpensesText.DataContext = _viewModel;
            TotalExpensesText.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("TotalExpense")
            {
                StringFormat = "${0:N2}"
            });

            BalanceText.DataContext = _viewModel;
            BalanceText.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("Balance")
            {
                StringFormat = "${0:N2}"
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}