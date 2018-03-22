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
    public class HintColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.EncodingInfo.Hint";

        public override string ResourceName => Name;

        public HintColorDefinition()
        {
            DisplayName = "Harurow.EncodingInfo - ヒント";
            ForegroundColor = Color.FromRgb(0xFF, 0xFF, 0xFF);
            BackgroundColor = Color.FromRgb(0x50, 0xB0, 0x10);
        }
    }
}