using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MinesweeperReborn.Localization;

namespace MinesweeperReborn.Views;

public partial class About : Window
{
    public About()
    {
        InitializeComponent();
        FillInfo();
    }

    private void OK_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void FillInfo()
    {
        var version = Assembly.GetEntryAssembly()?.GetName().Version
            ?? throw new InvalidOperationException(Strings.AppVersionUnavailable);
        AppLogo.Source = AssetPaths.LoadBitmap(ThemeManager.Gui.AboutLogo);

        string operatingSystem;
        if (OperatingSystem.IsWindows())
        {
            var windowsVersion = Environment.OSVersion.Version;
            operatingSystem = string.Format(
                Strings.AboutWindowsVersion,
                windowsVersion.Major,
                windowsVersion.Minor,
                windowsVersion.Build);
        }
        else
        {
            operatingSystem = string.Format(Strings.AboutOperatingSystem, RuntimeInformation.OSDescription);
        }

        string logoPath = Path.Combine(AppContext.BaseDirectory, "logo.png");
        Logo.Source = File.Exists(logoPath) ? AssetPaths.LoadBitmap(logoPath) : null;

        InfoText.Text =
$@"
{operatingSystem}
{string.Format(Strings.AboutGameVersion, version)}
";
    }
}
