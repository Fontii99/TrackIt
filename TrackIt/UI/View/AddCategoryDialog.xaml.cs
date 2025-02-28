using System.Windows;

namespace TrackIt.UI.View
{
    public partial class AddCategoryDialog : Window
    {
        public string CategoryName { get; private set; }

        public AddCategoryDialog()
        {
            InitializeComponent();
            CategoryTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CategoryTextBox.Text))
            {
                CategoryName = CategoryTextBox.Text.Trim();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a category name.", "Input Required",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}