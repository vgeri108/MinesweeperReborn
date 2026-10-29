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

using System;
using System.Globalization;
using System.IO;

namespace MinesweeperReborn.Localization;

public enum LanguagePreference
{
    System,
    Hungarian,
    English
}

public static class LanguageSettings
{
    private const string SettingsFileName = "language.txt";
    private const string ApplicationDirectoryName = "MinesweeperReborn";

    public static LanguagePreference Preference { get; private set; }

    public static void ApplyAtStartup()
    {
        Preference = LoadPreference();
        Strings.Culture = ResolveCulture(Preference);
    }

    public static void SetPreference(LanguagePreference preference)
    {
        if (!Enum.IsDefined(preference))
        {
            throw new ArgumentOutOfRangeException(nameof(preference));
        }

        var settingsPath = GetSettingsPath();
        Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
        File.WriteAllText(settingsPath, preference.ToString());
        Preference = preference;
    }

    private static LanguagePreference LoadPreference()
    {
        var settingsPath = GetSettingsPath();
        if (!File.Exists(settingsPath))
        {
            return LanguagePreference.System;
        }

        var value = File.ReadAllText(settingsPath).Trim();
        if (Enum.TryParse<LanguagePreference>(value, ignoreCase: true, out var preference) &&
            Enum.IsDefined(preference))
        {
            return preference;
        }

        throw new InvalidDataException($"Invalid language preference in '{settingsPath}'.");
    }

    private static CultureInfo ResolveCulture(LanguagePreference preference)
    {
        return preference switch
        {
            LanguagePreference.English => CultureInfo.GetCultureInfo("en-US"),
            LanguagePreference.Hungarian => CultureInfo.GetCultureInfo("hu-HU"),
            _ => ResolveSystemCulture()
        };
    }

    private static CultureInfo ResolveSystemCulture()
    {
        return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName switch
        {
            "en" => CultureInfo.GetCultureInfo("en-US"),
            "hu" => CultureInfo.GetCultureInfo("hu-HU"),
            _ => CultureInfo.GetCultureInfo("hu-HU")
        };
    }

    private static string GetSettingsPath()
    {
        var applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (string.IsNullOrWhiteSpace(applicationDataPath))
        {
            throw new InvalidOperationException("The local application data directory is unavailable.");
        }

        return Path.Combine(applicationDataPath, ApplicationDirectoryName, SettingsFileName);
    }
}
