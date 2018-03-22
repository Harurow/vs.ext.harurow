using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.CaretIndicator.Options
{
    [Export(typeof(IOptionValues))]
    internal sealed class OptionDialogPage : DialogPage, IOptionValues
    {
        [Category("CaretIndicator")]
        [DisplayName("現在行の水平線")]
        [Description("現在の行の下に水平線を引く")]
        [DefaultValue(DefaultValues.IsEnabledLineIndicator)]
        public bool IsEnabledLineIndicator { get; set; } = DefaultValues.IsEnabledLineIndicator;

        [Category("CaretIndicator")]
        [DisplayName("現在列の垂直線")]
        [Description("現在の列の左に垂直線を引く")]
        [DefaultValue(DefaultValues.IsEnabledColumnIndicator)]
        public bool IsEnabledColumnIndicator { get; set; } = DefaultValues.IsEnabledColumnIndicator;

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = OptionValues.ReadFromStore();

            IsEnabledLineIndicator = opt.IsEnabledLineIndicator;
            IsEnabledColumnIndicator = opt.IsEnabledColumnIndicator;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var store = StoreManagerFactory.Create();
            store.SetPropertyValue(nameof(IOptionValues.IsEnabledLineIndicator), IsEnabledLineIndicator);
            store.SetPropertyValue(nameof(IOptionValues.IsEnabledColumnIndicator), IsEnabledColumnIndicator);

            OptionValues.OnNext(this);
        }
    }
}