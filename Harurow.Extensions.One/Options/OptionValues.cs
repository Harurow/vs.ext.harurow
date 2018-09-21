using System.ComponentModel.Composition;

namespace Harurow.Extensions.One.Options
{
    // HACK: オプションを増やす
    internal interface IOptionValues
    {
        int RightMargin { get; set; }
        RedundantWhiteSpaceMode RedundantWhiteSpaceMode { get; set; }

        void LoadSettingsFromStorage();
        void SaveSettingsToStorage();
    }

    [Export(typeof(IOptionValues))]
    internal sealed class OptionValues : IOptionValues
    {
        internal sealed class Defaults
        {
            public const int RightMargin = 120;

            public const RedundantWhiteSpaceMode RedundantWhiteSpaceMode =
                One.RedundantWhiteSpaceMode.UseVisibleWhiteSpace;
        }

        public int RightMargin { get; set; }
        public RedundantWhiteSpaceMode RedundantWhiteSpaceMode { get; set; }

        public void LoadSettingsFromStorage()
        {
            var store = new StoreManager();

            RightMargin = store.GetPropertyValue(nameof(RightMargin), Defaults.RightMargin);
            RedundantWhiteSpaceMode = (RedundantWhiteSpaceMode)store.GetPropertyValue(nameof(RedundantWhiteSpaceMode),
                (int)Defaults.RedundantWhiteSpaceMode);
        }

        public void SaveSettingsToStorage()
        {
            var store = new StoreManager();

            store.SetPropertyValue(nameof(RightMargin), RightMargin);
            store.SetPropertyValue(nameof(RedundantWhiteSpaceMode), (int)RedundantWhiteSpaceMode);
        }
    }
}