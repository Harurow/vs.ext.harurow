using Harurow.Extensions.Options;

namespace Harurow.Extensions.CaretIndicator.Options
{
    internal static class StoreManagerFactory
    {
        private const string SubDir = "CaretIndicator";

        public static StoreManager Create()
            => new StoreManager(SubDir);
    }
}