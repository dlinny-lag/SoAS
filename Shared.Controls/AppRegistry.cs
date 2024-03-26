using System;
using Microsoft.Win32;

namespace Shared.Controls
{
    public class AppRegistry
    {
        public const string Manufacturer = "Dlinny_Lag";

        private readonly string appKey;
        public AppRegistry(string appKey)
        {
            this.appKey = appKey ?? throw new ArgumentNullException(nameof(appKey));
        }
        public RegistryKey GetMyKey()
        {
            RegistryKey software = Registry.CurrentUser.OpenSubKey("Software", true);
            var dl = software.OpenSubKey(Manufacturer, true) ?? software.CreateSubKey(Manufacturer);
            var appRegKey = dl.OpenSubKey(appKey, true) ?? dl.CreateSubKey(appKey);
            return appRegKey;
        }

        
    }
}