using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.RedundantWhiteSpace.Options
{
    [Export(typeof(IOptionValues))]
    internal sealed class OptionDialogPage : DialogPage, IOptionValues
    {
        [Category("RedundantWhiteSpace")]
        [DisplayName("改行前の連続した空白文字")]
        [Description("改行の前の連続した空白文字を強調表示します")]
        [DefaultValue(DefaultValues.RedundantWhiteSpacesHighlightMode)]
        public RedundantWhiteSpaceMode RedundantWhiteSpacesHighlightMode { get; set; } =
            DefaultValues.RedundantWhiteSpacesHighlightMode;

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = OptionValues.ReadFromStore();

            RedundantWhiteSpacesHighlightMode = opt.RedundantWhiteSpacesHighlightMode;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var store = StoreManagerFactory.Create();
            store.SetPropertyValue(nameof(IOptionValues.RedundantWhiteSpacesHighlightMode),
                (int) RedundantWhiteSpacesHighlightMode);

            OptionValues.OnNext(this);
        }
    }
}