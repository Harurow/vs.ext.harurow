using Harurow.Extensions.One.Adornments.RightMargin;

namespace Harurow.Extensions.One.ListenerServices
{
    partial class HarurowExtensionOneService
    {
        private TextViewAdornment RightMarginAdornment { get; set; }

        private void CreateRightMarginAdornment()
        {
            if (0 < Values.RightMargin)
            {
                var layer = TextView.GetBeforeDifferenceChangesAdornmentLayer();
                RightMarginAdornment = new TextViewAdornment(TextView, layer, Values.RightMargin, Resources.RightMarginBrush);
                RightMarginAdornment.OnInitialized();
            }
        }
    }
}
