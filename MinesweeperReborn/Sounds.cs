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

using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Providers;
using SoundFlow.Structs;
using System;
using System.IO;

namespace MinesweeperReborn
{
    internal static class Sounds
    {
        private static MiniAudioEngine? engine;
        private static AudioPlaybackDevice? playbackDevice;
        private static bool playbackDeviceStarted;

        public static void PlayStart() => Play(ThemeManager.Sounds.Start);
        public static void PlayClick() => Play(ThemeManager.Sounds.Click);
        public static void PlayFlag() => Play(ThemeManager.Sounds.FlagPlace);
        public static void PlayEveryClick() => Play(ThemeManager.Sounds.EveryClick);

        private static void Play(string path)
        {
            if (!AppSettings.Sounds || string.IsNullOrWhiteSpace(path))
                return;

            if (!File.Exists(path))
                throw new FileNotFoundException("A témában megadott hangfájl nem található.", path);

            engine ??= new MiniAudioEngine();
            playbackDevice ??= engine.InitializePlaybackDevice(null, AudioFormat.DvdHq);

            if (!playbackDeviceStarted)
            {
                playbackDevice.Start();
                playbackDeviceStarted = true;
            }

            StreamDataProvider dataProvider = new(engine, AudioFormat.DvdHq, File.OpenRead(path));
            SoundPlayer player = new(engine, playbackDevice.Format, dataProvider);
            playbackDevice.MasterMixer.AddComponent(player);
            player.PlaybackEnded += (_, _) =>
            {
                playbackDevice.MasterMixer.RemoveComponent(player);
                player.Dispose();
                dataProvider.Dispose();
            };
            player.Play();
        }
    }
}
