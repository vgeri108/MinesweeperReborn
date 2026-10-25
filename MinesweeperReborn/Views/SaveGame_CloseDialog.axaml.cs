using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MinesweeperCore;
using System;
using System.IO;

namespace MinesweeperReborn.Views
{
    public partial class SaveGame_CloseDialog : Window
    {
        public bool IsCanceled { get; private set; } =true;
        private Game game;
        public SaveGame_CloseDialog(Game _game)
        {
            InitializeComponent();
            game = _game;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            game.Save(Path.Combine(AppContext.BaseDirectory, "saves", "exit.mine"));
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