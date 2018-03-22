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
    public class WarningColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.EncodingInfo.Warning";

        public override string ResourceName => Name;

        public WarningColorDefinition()
        {
            DisplayName = "Harurow.EncodingInfo - 警告";
            ForegroundColor = Color.FromRgb(0xFF, 0xFF, 0xFF);
            BackgroundColor = Color.FromRgb(0xFF, 0x70, 0x30);
        }
    }
}