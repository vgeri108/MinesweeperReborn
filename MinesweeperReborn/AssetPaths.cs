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

using Avalonia.Media.Imaging;
using System;
using System.IO;

namespace MinesweeperReborn
{
    internal static class AssetPaths
    {
        private static readonly string AssetsDirectory = Path.Combine(AppContext.BaseDirectory, "Assets");

        public static string GetPath(params string[] relativeSegments)
        {
            return Path.Combine(AssetsDirectory, Path.Combine(relativeSegments));
        }

        public static Bitmap LoadBitmap(string path)
        {
            using Stream imageStream = File.OpenRead(path);
            return new Bitmap(imageStream);
        }
    }
}
