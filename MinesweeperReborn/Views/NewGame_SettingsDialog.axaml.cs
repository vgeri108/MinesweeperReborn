using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MinesweeperReborn.Views
{
    public partial class NewGame_SettingsDialog : Window
    {
        public string Selected { get; private set; } = "Cancel";
        public NewGame_SettingsDialog()
        {
            InitializeComponent();
        }

        private void StartNew_Click(object sender, RoutedEventArgs e)
        {
            Selected = "StartNew";
            Close();
        }

        private void OnNextGame_Click(object sender, RoutedEventArgs e)
        {
            Selected = "OnNextGame";
            Close();
        }
    }
}