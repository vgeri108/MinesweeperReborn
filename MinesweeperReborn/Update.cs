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
using MinesweeperReborn.Localization;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinesweeperReborn
{
    internal static class Update
    {
        private const string LatestReleaseUrl = "https://api.github.com/repos/vgeri108/MinesweeperReborn/releases/latest";
        private const string ReleasesPageUrl = "https://github.com/vgeri108/MinesweeperReborn/releases/latest";

        public static async Task CheckAndPrompt(Window owner, bool automatic = false)
        {
            try
            {
                using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(5) };
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MinesweeperReborn");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

                using HttpResponseMessage response = await client.GetAsync(LatestReleaseUrl);
                response.EnsureSuccessStatusCode();
                using JsonDocument release = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                JsonElement root = release.RootElement;

                string tag = root.GetProperty("tag_name").GetString()
                    ?? throw new InvalidDataException(Strings.MissingReleaseTag);
                Version installed = Assembly.GetEntryAssembly()?.GetName().Version
                    ?? throw new InvalidOperationException(Strings.AppVersionUnavailable);
                if (!Version.TryParse(tag.TrimStart('v', 'V'), out Version? latest))
                {
                    throw new InvalidDataException(string.Format(Strings.InvalidReleaseVersion, tag));
                }

                if (latest <= installed)
                {
                    if (!automatic)
                    {
                        await ShowMessage(owner, Strings.UpdateTitle, Strings.LatestVersionInstalled);
                    }

                    return;
                }

                string description = root.TryGetProperty("body", out JsonElement body)
                    ? body.GetString() ?? string.Empty
                    : string.Empty;
                var result = await MessageBoxManager.GetMessageBoxStandard(
                    Strings.UpdateAvailable,
                    string.Format(Strings.UpdatePrompt, tag, description),
                    ButtonEnum.YesNo).ShowWindowDialogAsync(owner);

                if (result == ButtonResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(ReleasesPageUrl) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                if (!automatic)
                {
                    await ShowMessage(owner, Strings.UpdateError, ex.Message);
                }
            }
        }

        private static Task ShowMessage(Window owner, string title, string text) =>
            MessageBoxManager.GetMessageBoxStandard(title, text, ButtonEnum.Ok).ShowWindowDialogAsync(owner);
    }
}
