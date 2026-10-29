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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using MinesweeperReborn.Localization;

namespace MinesweeperReborn;

public partial class App : Application
{
    private static readonly Style LightWindowStyle = new(x => x.OfType<Window>())
    {
        Setters =
        {
            new Setter(Window.BackgroundProperty, new SolidColorBrush(Color.Parse("#FFF0F0F0")))
        }
    };
    private static readonly Style LightButtonStyle = new(x => x.OfType<Button>())
    {
        Setters =
        {
            new Setter(Button.BackgroundProperty, new SolidColorBrush(Color.Parse("#FFD0D0D0")))
        }
    };

    public override void Initialize()
    {
        JsonManager.Load();
        ThemeManager.LoadTheme(AppSettings.CurrentTheme);
        LanguageSettings.ApplyAtStartup();
        AvaloniaXamlLoader.Load(this);
        ApplyThemePreference();
    }

    internal static void ApplyThemePreference()
    {
        App app = (App)Current!;
        app.RequestedThemeVariant = AppSettings.ThemePreference switch
        {
            AppThemePreference.Light => ThemeVariant.Light,
            AppThemePreference.Dark => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };

        app.Styles.Remove(LightWindowStyle);
        app.Styles.Remove(LightButtonStyle);
        if (AppSettings.ThemePreference == AppThemePreference.Light)
        {
            app.Styles.Add(LightWindowStyle);
            app.Styles.Add(LightButtonStyle);
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}