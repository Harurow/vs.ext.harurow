using System;
using Harurow.Extensions.One.Adornments;
using Harurow.Extensions.One.Adornments.LineAdornments;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.AdornmentServices
{
    partial class HarurowExtensionOneService
    {
        private RedundantWhiteSpaceAdornment RedundantWhiteSpaceAdornment { get; set; }

        private void CreateRedundantWhiteSpacesAdornment()
        {
            RedundantWhiteSpacePainter CreatePainter()
            {
                var layer = TextView.GetAfterSelectionAdornmentLayer();
                return new RedundantWhiteSpacePainter(TextView, layer,
                    Resources.RedundantWhiteSpacesBrush, Resources.RedundantWhiteSpacesPen);
            }

            bool IsEnabled(RedundantWhiteSpaceMode mode, bool useVisibleWhitespace)
            {
                switch (mode)
                {
                    case RedundantWhiteSpaceMode.True:
                        return true;
                    case RedundantWhiteSpaceMode.False:
                        return false;
                    case RedundantWhiteSpaceMode.UseVisibleWhiteSpace:
                        return useVisibleWhitespace;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
            }

            var useWhitespace = TextView.Options.GetOptionValue(new UseVisibleWhitespace().Key);

            if (IsEnabled(Values.RedundantWhiteSpaceMode, useWhitespace))
            {
                var lineAdornment = new RedundantWhiteSpacesLineAdornment(TextView, CreatePainter());
                RedundantWhiteSpaceAdornment = new RedundantWhiteSpaceAdornment(TextView, lineAdornment);
                RedundantWhiteSpaceAdornment.OnInitialized();
            }
            else
            {
                RedundantWhiteSpaceAdornment = null;
            }
        }
    }
}
