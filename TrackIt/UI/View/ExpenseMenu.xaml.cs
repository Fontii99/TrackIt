using System.Windows;

namespace TrackIt
{
    public partial class ExpenseMenu : Window
    {
        public ExpenseMenu()
        {
            InitializeComponent();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}