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
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MinesweeperReborn.Localization;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace MinesweeperReborn.Views
{
    public partial class AppSettingsMenu : Window
    {
        public bool RestartRequested { get; private set; }

        public AppSettingsMenu()
        {
            InitializeComponent();

            UpdateCheck.IsChecked = AppSettings.AutomaticUpdateSearch;
            ContinueSaved.IsChecked = AppSettings.AlwaysContinueSavedGame;
            SaveExit.IsChecked = AppSettings.AlwaysSaveGameOnExit;
            LanguagePreferenceComboBox.SelectedIndex = (int)LanguageSettings.Preference;
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.AutomaticUpdateSearch = (bool)UpdateCheck.IsChecked;
            AppSettings.AlwaysContinueSavedGame = (bool)ContinueSaved.IsChecked;
            AppSettings.AlwaysSaveGameOnExit = (bool)SaveExit.IsChecked;

            LanguagePreference preference = (LanguagePreference)LanguagePreferenceComboBox.SelectedIndex;
            bool languageChanged = preference != LanguageSettings.Preference;
            if (languageChanged)
            {
                LanguageSettings.SetPreference(preference);
            }

            JsonManager.Save();

            if (languageChanged)
            {
                string? result = await MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ContentTitle = Strings.Language,
                    ContentMessage = Strings.RestartNowPrompt,
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = Strings.Yes, IsDefault = true },
                        new ButtonDefinition { Name = Strings.No, IsCancel = true }
                    }
                }).ShowWindowDialogAsync(this);
                RestartRequested = result == Strings.Yes;
            }

            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            await Update.CheckAndPrompt(this);
        }
    }
}