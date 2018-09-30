using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.One.Options
{
    internal abstract class ColorDefinition : EditorFormatDefinition
    {
        public string ResourceName
            => "Harurow.Extensions.One." + GetType().Name;
    }
}