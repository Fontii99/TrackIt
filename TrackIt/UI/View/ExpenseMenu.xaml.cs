﻿using System.Windows;
using TrackIt.UI.Model;

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

        private void RecurringCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PeriodComboBox.IsEnabled = true;
            EndDatePicker.IsEnabled = true;
            
        }

        private void RecurringCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PeriodComboBox.IsEnabled = false;
            EndDatePicker.IsEnabled = false;
            EndDatePicker.SelectedDate = null;
            PeriodComboBox.SelectedIndex = -1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}