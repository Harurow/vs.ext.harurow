using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.RightMargin.Options
{
    [Export(typeof(IOptionValues))]
    internal sealed class OptionDialogPage : DialogPage, IOptionValues
    {
        [Category("RightMargin")]
        [DisplayName("推奨桁数")]
        [Description("推奨桁数を指定します。0を指定すると無効化されます。")]
        [DefaultValue(DefaultValues.RightMargin)]
        public int RightMargin { get; set; } = DefaultValues.RightMargin;

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = OptionValues.ReadFromStore();

            RightMargin = opt.RightMargin;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var store = StoreManagerFactory.Create();
            store.SetPropertyValue(nameof(IOptionValues.RightMargin), RightMargin);

            OptionValues.OnNext(this);
        }
    }
}