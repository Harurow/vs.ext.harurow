
using Harurow.Extensions.One.StatusBars.Models;

namespace Harurow.Extensions.One.ListenerServices
{
    partial class HarurowExtensionOneService
    {
        private void AttachEncodingInfo()
        {
            new StatusBarInfoModel(TextView);
        }
    }
}
