using System.Windows;

namespace TrackIt
{
    public partial class SummaryMenu : Window
    {
        public SummaryMenu()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}