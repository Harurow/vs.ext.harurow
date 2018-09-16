using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace Harurow.Extensions.One.Options
{
    public sealed class StoreManager
    {
        private const string BasePath = @"ApplicationPrivateSettings\Harurow.Extensions.One";

        private WritableSettingsStore Store { get; }

        private string CollectionPath { get; }

        public StoreManager()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            CollectionPath = BasePath;

            var settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            Store = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!Store.CollectionExists(CollectionPath))
            {
                Store.CreateCollection(CollectionPath);
            }
        }

        public bool GetPropertyValue(string propertyName, bool defaultValue)
            => Store.GetBoolean(CollectionPath, propertyName, defaultValue);

        public int GetPropertyValue(string propertyName, int defaultValue)
            => Store.GetInt32(CollectionPath, propertyName, defaultValue);

        public void SetPropertyValue(string propertyName, bool defaultValue)
            => Store.SetBoolean(CollectionPath, propertyName, defaultValue);

        public void SetPropertyValue(string propertyName, int defaultValue)
            => Store.SetInt32(CollectionPath, propertyName, defaultValue);
    }
}
