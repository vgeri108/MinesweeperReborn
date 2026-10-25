using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using MinesweeperCore;

namespace MinesweeperReborn.Views
{
    public partial class FirstStartDifficulty : Window
    {
        public FirstStartDifficulty()
        {
            InitializeComponent();
        }
        private void NewBeginner(object sender, RoutedEventArgs e)
        {
            GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Beginner).Height;
            GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Beginner).Width;
            GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Beginner).Mines;
            Close();
        }
        private void NewIntermediate(object sender, RoutedEventArgs e)
        {
            GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Intermediate).Height;
            GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Intermediate).Width;
            GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Intermediate).Mines;
            Close();
        }
        private void NewAdvanced(object sender, RoutedEventArgs e)
        {
            GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Expert).Height;
            GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Expert).Width;
            GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Expert).Mines;
            Close();
        }
    }
}