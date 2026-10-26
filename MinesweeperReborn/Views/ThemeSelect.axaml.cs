using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MinesweeperCore;
using System;
using System.IO;

namespace MinesweeperReborn.Views
{
    public partial class ThemeSelect : Window
    {
        private readonly string StartTheme;

        public ThemeSelect()
        {
            InitializeComponent();
            StartTheme = ThemeManager.CurrentTheme;
            Load();
        }

        private void Load()
        {
            string themeName = ThemeFinder.GetThemeNameFromJson(ThemeManager.CurrentTheme);

            SelectedBoardIcon.Source = LoadBitmap(ThemeManager.CellStatusImage[CellStatus.Covered]);
            SelectedMineIcon.Source = LoadBitmap(ThemeManager.CellImage[Cell.Mine]);
            SelectedNumberIcon.Source = LoadBitmap(ThemeManager.CellImage[Cell.One]);
            SelectedBackgroundIcon.Source = LoadBitmap(ThemeFinder.GetThemeImageUri(ThemeManager.CurrentTheme));

            BoardSelected.Text = themeName;
            MinesSelected.Text = themeName;
            NumbersSelected.Text = themeName;
            BackgroundSelected.Text = themeName;

            ThemeName.Text = $"Téma neve: {themeName}";
            ThemeCreator.Text = $"Téma készítője: {ThemeFinder.GetThemeCreatorFromJson(ThemeManager.CurrentTheme)}";
            ThemeDescription.Text = $"Téma leírása: {ThemeFinder.GetThemeDescriptionFromJson(ThemeManager.CurrentTheme)}";

            ThemeList.Children.Clear();
            foreach (string themeFolder in ThemeFinder.GetThemeList())
            {
                Image image = new Image
                {
                    Source = LoadBitmap(ThemeFinder.GetThemeImageUri(themeFolder)),
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                TextBlock text = new TextBlock
                {
                    Text = ThemeFinder.GetThemeNameFromJson(themeFolder),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                };

                StackPanel panel = new StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Vertical,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };
                panel.Children.Add(image);
                panel.Children.Add(text);

                Button button = new Button
                {
                    Content = panel,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    Background = null,
                    BorderThickness = new Thickness(0),
                    Tag = themeFolder
                };
                button.Click += ThemeButton_Click;

                ThemeList.Children.Add(button);
            }
        }

        private static Bitmap LoadBitmap(Uri assetUri)
        {
            using Stream imageStream = AssetLoader.Open(assetUri);
            return new Bitmap(imageStream);
        }

        private void ThemeButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string themeFolder)
            {
                ThemeManager.LoadTheme(themeFolder);
                Load();
            }
        }

        public void Save_Click(object? sender, RoutedEventArgs e)
        {
            AppSettings.CurrentTheme = ThemeManager.CurrentTheme;
            JsonManager.Save();
            Close();
        }

        public void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            ThemeManager.LoadTheme(StartTheme);
            AppSettings.CurrentTheme = StartTheme;
            Close();
        }
    }
}