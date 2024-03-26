using System;
using System.IO;
using Microsoft.Win32;

namespace Shared.Controls
{
    public static class FoldersExtension
    {
        public static string FindFo4Folder()
        {
            string expectedPath = TryGetFromRegistry();
            if (!string.IsNullOrWhiteSpace(expectedPath) && Directory.Exists(expectedPath))
                return expectedPath;

            expectedPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "Steam",
                "steamapps",
                "common",
                "Fallout 4");
            if (Directory.Exists(expectedPath))
                return expectedPath;
            return null;
        }

        private static string TryGetFromRegistry()
        {
            // HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Bethesda Softworks\Fallout4
            var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            var f4Key = localMachine.OpenSubKey(@"SOFTWARE\Bethesda Softworks\Fallout4", false);
            if (f4Key == null)
                return null;
            return f4Key.GetValue("Installed Path", null) as string;
        }


        public static string FindAAFFolder()
        {
            string fo4Folder = FindFo4Folder();
            if (string.IsNullOrWhiteSpace(fo4Folder))
                return null;

            string expectedPath = Path.Combine(fo4Folder, "Data", "AAF");
            if (Directory.Exists(expectedPath))
                return expectedPath;
            return null;
        }

        public static string StoredFolder(this AppRegistry appRegistry, string folderKey)
        {
            string path = appRegistry.GetMyKey().GetValue(folderKey) as string;
            if (path != null && Directory.Exists(path))
                return path;
            return null;
        }

        public static void StoreFolder(this AppRegistry appRegistry, string folderKey, string folderPath)
        {
            appRegistry.GetMyKey().SetValue(folderKey, folderPath, RegistryValueKind.String);
        }

        public static void StoreFo4Path(this AppRegistry appRegistry, string fo4Folder)
        {
            appRegistry.StoreFolder("Fo4Path", fo4Folder);
        }
        public static string StoredF4Path(this AppRegistry appRegistry)
        {
            return appRegistry.StoredFolder("Fo4Path");
        }

        public static string StoredAAFFolder(this AppRegistry appRegistry)
        {
            return appRegistry.StoredFolder("AAFPath");
        }

        public static void StoreAAFFolder(this AppRegistry appRegistry, string aafFolder)
        {
            appRegistry.StoreFolder("AAFPath", aafFolder);
        }
    }
}