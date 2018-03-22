using Harurow.Extensions.Options;

namespace Harurow.Extensions.Commands.Options
{
    internal static class StoreManagerFactory
    {
        private const string SubDir = "Commands";

        public static StoreManager Create()
            => new StoreManager(SubDir);
    }
}