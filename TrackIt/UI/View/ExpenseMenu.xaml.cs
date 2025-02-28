using System.Windows;
using TrackIt.UI.ViewModel;

namespace TrackIt
{
    public partial class ExpenseMenu : Window
    {
        private ExpenseMenuViewModel _viewModel;

        public ExpenseMenu()
        {
            InitializeComponent();
            _viewModel = Resources["ExpenseViewModel"] as ExpenseMenuViewModel;
            _viewModel.RequestClose += () => Close();
        }
    }
}