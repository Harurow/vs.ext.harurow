using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.EncodingInfo.Options
{
    [Export(typeof(IOptionValues))]
    internal sealed class OptionDialogPage : DialogPage, IOptionValues
    {
        [Category("EncodingInfo")]
        [DisplayName("BOMを推奨する")]
        [Description("BOM付きUTF-8を推奨する")]
        [DefaultValue(DefaultValues.IsEnabledRecommendUtf8Bom)]
        public bool IsEnabledRecommendUtf8Bom { get; set; } = DefaultValues.IsEnabledRecommendUtf8Bom;

        [Category("EncodingInfo")]
        [DisplayName("UTF-8以外を警告")]
        [Description("UTF-8以外のエンコーディングを警告表示する")]
        [DefaultValue(DefaultValues.IsEnabledWarningOtherEncoding)]
        public bool IsEnabledWarningOtherEncoding { get; set; } = DefaultValues.IsEnabledWarningOtherEncoding;

        [Category("EncodingInfo")]
        [DisplayName("自動的に閉じる")]
        [Description("エンコーディング情報を自動的に閉じます")]
        [DefaultValue(DefaultValues.IsEnabledWarningOtherEncoding)]
        public bool IsEnabledAutoHide { get; set; } = DefaultValues.IsEnabledAutoHide;

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = OptionValues.ReadFromStore();

            IsEnabledRecommendUtf8Bom = opt.IsEnabledRecommendUtf8Bom;
            IsEnabledWarningOtherEncoding = opt.IsEnabledWarningOtherEncoding;
            IsEnabledAutoHide = opt.IsEnabledAutoHide;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var store = StoreManagerFactory.Create();
            store.SetPropertyValue(nameof(IOptionValues.IsEnabledRecommendUtf8Bom), IsEnabledRecommendUtf8Bom);
            store.SetPropertyValue(nameof(IOptionValues.IsEnabledWarningOtherEncoding), IsEnabledWarningOtherEncoding);
            store.SetPropertyValue(nameof(IOptionValues.IsEnabledAutoHide), IsEnabledAutoHide);

            OptionValues.OnNext(this);
        }
    }
}