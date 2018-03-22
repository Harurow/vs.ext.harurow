using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.Commands.Options
{
    [Export(typeof(IOptionValues))]
    internal sealed class OptionDialogPage : DialogPage, IOptionValues
    {
        [Category("Commands")]
        [DisplayName("マウス・ホイールのズームを抑制")]
        [Description("有効にするとマウスホイールで拡大・縮小ができないようになります")]
        [DefaultValue(DefaultValues.IsLockedMouseWheelZoom)]
        public bool IsLockedMouseWheelZoom { get; set; } = DefaultValues.IsLockedMouseWheelZoom;

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = OptionValues.ReadFromStore();

            IsLockedMouseWheelZoom = opt.IsLockedMouseWheelZoom;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var store = StoreManagerFactory.Create();
            store.SetPropertyValue(nameof(IOptionValues.IsLockedMouseWheelZoom), IsLockedMouseWheelZoom);

            OptionValues.OnNext(this);
        }
    }
}