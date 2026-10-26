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
