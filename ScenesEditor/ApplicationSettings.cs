using System;
using System.IO;
using Shared.Controls;

namespace ScenesEditor
{
    internal static class ApplicationSettings
    {
        public const string AppName = "AnimationScenes";
        public static readonly AppRegistry AppRegistry = new AppRegistry(AppName);

        public static readonly string ApplicationDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppRegistry.Manufacturer, AppName);

        public static readonly FurnitureLibrary FurnitureLibrary = new FurnitureLibrary(); // TODO: move to appropriate place
    }
}