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
using System.Diagnostics;
using System.Linq;
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

    private void MoreInfo_Click(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/vgeri108/MinesweeperReborn")
        {
            UseShellExecute = true
        });
    }

    private void FillInfo()
    {
        Assembly assembly = Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException(Strings.AppVersionUnavailable);
        var version = assembly.GetName().Version
            ?? throw new InvalidOperationException(Strings.AppVersionUnavailable);
        string commitHash = assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(attribute => attribute.Key == "CommitHash")
            .Value;
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

        InfoText.Text =
            $"{string.Format(Strings.AboutGameVersion, version)}\n{operatingSystem}\n\n{Strings.AboutCredits}\n{string.Format(Strings.AboutCommitHash, commitHash)}";
    }
}
