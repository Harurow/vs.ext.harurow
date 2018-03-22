using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.LineBreak.Options
{
    [Export(typeof(IOptionValues))]
    internal sealed class OptionDialogPage : DialogPage, IOptionValues
    {
        [Category("LineBreak")]
        [DisplayName("改行の表示")]
        [Description("改行を表示します")]
        [DefaultValue(DefaultValues.VisibleLineBreakMode)]
        public LineBreakMode VisibleLineBreakMode { get; set; } = DefaultValues.VisibleLineBreakMode;

        [Category("LineBreak")]
        [DisplayName("異なる改行コードを強調")]
        [Description("改行コードが異なる場合に強調表示します")]
        [DefaultValue(DefaultValues.LineBreakWarningMode)]
        public LineBreakMode LineBreakWarningMode { get; set; } = DefaultValues.LineBreakWarningMode;

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = OptionValues.ReadFromStore();

            VisibleLineBreakMode = opt.VisibleLineBreakMode;
            LineBreakWarningMode = opt.LineBreakWarningMode;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var store = StoreManagerFactory.Create();
            store.SetPropertyValue(nameof(IOptionValues.VisibleLineBreakMode), (int)VisibleLineBreakMode);
            store.SetPropertyValue(nameof(IOptionValues.LineBreakWarningMode), (int)LineBreakWarningMode);

            OptionValues.OnNext(this);
        }
    }
}