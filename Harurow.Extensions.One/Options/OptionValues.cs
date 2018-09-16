using System.ComponentModel.Composition;

namespace Harurow.Extensions.One.Options
{
    internal interface IOptionValues
    {
        int RightMargin { get; set; }

        void LoadSettingsFromStorage();
        void SaveSettingsToStorage();
    }

    [Export(typeof(IOptionValues))]
    internal sealed class OptionValues : IOptionValues
    {
        internal sealed class Defaults
        {
            public const int RightMargin = 120;
        }

        public int RightMargin { get; set; }

        public void LoadSettingsFromStorage()
        {
            var store = new StoreManager();

            RightMargin = store.GetPropertyValue(nameof(RightMargin), Defaults.RightMargin);
        }

        public void SaveSettingsToStorage()
        {
            var store = new StoreManager();

            store.SetPropertyValue(nameof(RightMargin), RightMargin);

        }
    }
}