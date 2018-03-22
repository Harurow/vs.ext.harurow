using Harurow.Extensions.Options;

namespace Harurow.Extensions.RightMargin.Options
{
    internal static class StoreManagerFactory
    {
        private const string SubDir = "RightMargin";

        public static StoreManager Create()
            => new StoreManager(SubDir);
    }
}