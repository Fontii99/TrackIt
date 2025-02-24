using System.Windows;
using TrackIt.UI.Model;

namespace TrackIt
{
    public partial class IncomeMenu : Window
    {
        public IncomeMenu()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RecurringCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PeriodComboBox.IsEnabled = true;
        }

        private void RecurringCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PeriodComboBox.IsEnabled = false;
            PeriodComboBox.SelectedIndex = -1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
