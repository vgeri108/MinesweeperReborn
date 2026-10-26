using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MinesweeperReborn.Views
{
    public partial class AppSettingsMenu : Window
    {
        public AppSettingsMenu()
        {
            InitializeComponent();

            UpdateCheck.IsChecked = AppSettings.AutomaticUpdateSearch;
            ContinueSaved.IsChecked = AppSettings.AlwaysContinueSavedGame;
            SaveExit.IsChecked = AppSettings.AlwaysSaveGameOnExit;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.AutomaticUpdateSearch = (bool)UpdateCheck.IsChecked;
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
            await Update.CheckAndPrompt(this);
        }
    }
}