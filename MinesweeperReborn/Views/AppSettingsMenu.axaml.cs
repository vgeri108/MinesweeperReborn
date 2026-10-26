using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Threading.Tasks;

namespace MinesweeperReborn.Views
{
    public partial class AppSettingsMenu : Window
    {
        public AppSettingsMenu()
        {
            InitializeComponent();

            ContinueSaved.IsChecked = AppSettings.AlwaysContinueSavedGame;
            SaveExit.IsChecked = AppSettings.AlwaysSaveGameOnExit;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.AlwaysContinueSavedGame = (bool)ContinueSaved.IsChecked;
            AppSettings.AlwaysSaveGameOnExit = (bool)SaveExit.IsChecked;
            JsonManager.Save();
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}