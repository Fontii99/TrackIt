using System.Windows;
namespace TrackIt
{
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void ExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            ExpenseMenu expenseWindow = new ExpenseMenu();

            expenseWindow.ShowDialog();
        }

        private void IncomeButton_Click(object sender, RoutedEventArgs e)
        { 
            IncomeMenu incomeWindow = new IncomeMenu();

            incomeWindow.ShowDialog();
        }

        private void SummaryButton_Click(object sender, RoutedEventArgs e)
        {            
            SummaryMenu summaryWindow = new SummaryMenu();

            summaryWindow.ShowDialog();
        }
    }
}