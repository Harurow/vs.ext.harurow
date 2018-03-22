using Harurow.Extensions.Options;

namespace Harurow.Extensions.LineBreak.Options
{
    internal static class StoreManagerFactory
    {
        private const string SubDir = "LineBreak";

        public static StoreManager Create()
            => new StoreManager(SubDir);
    }
}