using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MinesweeperCore;
using MinesweeperReborn.Localization;
using System;
using System.Diagnostics;

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

            SelectedBoardIcon.Source = AssetPaths.LoadBitmap(ThemeManager.CellStatusImage[CellStatus.Covered]);
            SelectedMineIcon.Source = AssetPaths.LoadBitmap(ThemeManager.CellImage[Cell.Mine]);
            SelectedNumberIcon.Source = AssetPaths.LoadBitmap(ThemeManager.CellImage[Cell.One]);
            SelectedBackgroundIcon.Source = AssetPaths.LoadBitmap(ThemeFinder.GetThemeImagePath(ThemeManager.CurrentTheme));

            BoardSelected.Text = themeName;
            MinesSelected.Text = themeName;
            NumbersSelected.Text = themeName;
            BackgroundSelected.Text = themeName;

            ThemeName.Text = string.Format(Strings.ThemeNameLabel, themeName);
            ThemeCreator.Text = string.Format(Strings.ThemeCreatorLabel, ThemeFinder.GetThemeCreatorFromJson(ThemeManager.CurrentTheme));
            ThemeDescription.Text = string.Format(Strings.ThemeDescriptionLabel, ThemeFinder.GetThemeDescriptionFromJson(ThemeManager.CurrentTheme));
            string licenseName = ThemeFinder.GetThemeLicenseNameFromJson(ThemeManager.CurrentTheme);
            ThemeLicenseName.Text = licenseName;
            ThemeLicenseName.Tag = ThemeManager.CurrentTheme;
            ThemeLicenseRow.IsVisible = !string.IsNullOrWhiteSpace(licenseName);
            string licenseUrl = ThemeFinder.GetThemeLicenseUrlFromJson(ThemeManager.CurrentTheme);
            ThemeLicenseMoreInfo.Tag = licenseUrl;
            ThemeLicenseMoreInfo.IsVisible = !string.IsNullOrWhiteSpace(licenseUrl);

            ThemeList.Children.Clear();
            foreach (string themeFolder in ThemeFinder.GetThemeList())
            {
                Image image = new Image
                {
                    Source = AssetPaths.LoadBitmap(ThemeFinder.GetThemeImagePath(themeFolder)),
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

        private void ThemeLicenseName_Click(object? sender, PointerPressedEventArgs e)
        {
            if (sender is TextBlock { Tag: string themeFolder })
            {
                string licensePath = AssetPaths.GetPath("Themes", themeFolder, "LICENSE.txt");
                Process.Start(new ProcessStartInfo(licensePath) { UseShellExecute = true });
            }
        }

        private void ThemeLicenseMoreInfo_Click(object? sender, PointerPressedEventArgs e)
        {
            if (sender is TextBlock { Tag: string licenseUrl } &&
                Uri.TryCreate(licenseUrl, UriKind.Absolute, out Uri? uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
            }
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