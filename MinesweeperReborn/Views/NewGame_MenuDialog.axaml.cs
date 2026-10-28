using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MinesweeperReborn.Views
{
    public partial class NewGame_MenuDialog : Window
    {
        public string Selected { get; private set; } = "Cancel";
        public NewGame_MenuDialog()
        {
            InitializeComponent();
        }

        private void StartNew_Click(object sender, RoutedEventArgs e)
        {
            Selected = "StartNew";
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Selected = "Cancel";
            Close();
        }
    }
}