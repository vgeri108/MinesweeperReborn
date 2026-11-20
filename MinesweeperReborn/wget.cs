using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MinesweeperReborn
{
    internal class Wget
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static Task<bool> DownloadAsync(string link)
        {
            Uri uri = new Uri(link);
            string fileName = Path.GetFileName(uri.LocalPath);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("You cannot download an empty link.");
            }

            string fullPath = Path.Combine(AppContext.BaseDirectory, fileName);
            return DownloadAsync(link, fullPath);
        }


        public static async Task<bool> DownloadAsync(string link, string output)
        {
            try
            {
                string? directory = Path.GetDirectoryName(output);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using Stream remoteStream = await _httpClient.GetStreamAsync(link);
                using FileStream fileStream = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None);

                await remoteStream.CopyToAsync(fileStream);

                return true;
            }
            catch
            {
                if (File.Exists(output))
                {
                    try { File.Delete(output); } catch { }
                }
                return false;
            }
        }
    }
}
