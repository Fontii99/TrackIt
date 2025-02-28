using System.Windows;
using TrackIt.UI.ViewModel;

namespace TrackIt
{
    public partial class IncomeMenu : Window
    {
        private IncomeMenuViewModel _viewModel;

        public IncomeMenu()
        {
            InitializeComponent();
            _viewModel = Resources["IncomeViewModel"] as IncomeMenuViewModel;
            _viewModel.RequestClose += () => Close();
        }
    }
}