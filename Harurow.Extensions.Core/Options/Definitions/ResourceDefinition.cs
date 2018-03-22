using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.Options.Definitions
{
    public abstract class ResourceDefinition : EditorFormatDefinition
    {
        public abstract string ResourceName { get; }
    }
}
