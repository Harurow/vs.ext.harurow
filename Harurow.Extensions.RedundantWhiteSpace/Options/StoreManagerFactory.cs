using Harurow.Extensions.Options;

namespace Harurow.Extensions.RedundantWhiteSpace.Options
{
    internal static class StoreManagerFactory
    {
        private const string SubDir = "RedundantWhiteSpace";

        public static StoreManager Create()
            => new StoreManager(SubDir);
    }
}