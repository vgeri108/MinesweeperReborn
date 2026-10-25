using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MinesweeperCore;

namespace MinesweeperReborn.Views
{
    public partial class SaveGame_CloseDialog : Window
    {
        public bool IsCanceled { get; private set; } =true;
        public SaveGame_CloseDialog()
        {
            InitializeComponent();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            Close();
        }
        private void DontSave_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = true;
            Close();
        }
    }
}