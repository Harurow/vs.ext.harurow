using Harurow.Extensions.Options;

namespace Harurow.Extensions.EncodingInfo.Options
{
    internal static class StoreManagerFactory
    {
        private const string SubDir = "EncodingInfo";

        public static StoreManager Create()
            => new StoreManager(SubDir);
    }
}