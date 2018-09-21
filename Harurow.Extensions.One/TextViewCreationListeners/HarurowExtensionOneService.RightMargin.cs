using Harurow.Extensions.One.Adornments;

namespace Harurow.Extensions.One.TextViewCreationListeners
{
    partial class HarurowExtensionOneService
    {
        private RightMarginAdornment RightMarginAdornment { get; set; }

        private void CreateRightMarginAdornment()
        {
            var layer = TextView.GetBeforeDifferenceChangesAdornmentLayer();
            RightMarginAdornment = new RightMarginAdornment(TextView, layer, Values.RightMargin, Resources.RightMarginBrush);
            RightMarginAdornment.OnInitialized();
        }
    }
}
