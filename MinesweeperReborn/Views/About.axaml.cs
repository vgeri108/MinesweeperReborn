/*
 * Copyright (C) 2026 vgeri108
 *
 * This file is part of MinesweeperReborn.
 *
 * MinesweeperReborn is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3 only.
 *
 * MinesweeperReborn is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with MinesweeperReborn. If not, see <https://www.gnu.org/licenses/>.
 */

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
