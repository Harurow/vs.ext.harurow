using System.ComponentModel.Composition;
using System.Windows.Media;
using Harurow.Extensions.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.EncodingInfo.Options.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [Name(Name)]
    public class InfoColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.EncodingInfo.Info";

        public override string ResourceName => Name;

        public InfoColorDefinition()
        {
            DisplayName = "Harurow.EncodingInfo - 情報";
            ForegroundColor = Color.FromRgb(0x00, 0x00, 0x00);
            BackgroundColor = Color.FromRgb(0xFF, 0xFF, 0xFF);
        }
    }
}