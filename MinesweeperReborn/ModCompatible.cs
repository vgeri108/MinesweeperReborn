using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MinesweeperCore;

namespace MinesweeperReborn
{
    public static class ModCompatible
    {
        public static dynamic? LoadModLoader(string modsDir, Game game, Action draw, Action gameOver, Action<string> loadGame)
        {
            string dllPath = Path.Combine(AppContext.BaseDirectory, "MinesweeperModLoader.dll");
            if (!File.Exists(dllPath))
            {
                Debug.WriteLine("ModLoader DLL not found – running without mods.");
                return null;
            }

            try
            {
                var asm = Assembly.LoadFrom(dllPath);
                var type = asm.GetType("MinesweeperModLoader.ModLoader");
                if (type == null) return null;

                var instance = Activator.CreateInstance(type);
                var loadMethod = type.GetMethod("LoadMods", BindingFlags.Public | BindingFlags.Instance);
                loadMethod?.Invoke(instance, new object[] { modsDir, game, draw, gameOver, loadGame });

                return instance;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load ModLoader: {ex.Message}");
                return null;
            }
        }
    }
}
