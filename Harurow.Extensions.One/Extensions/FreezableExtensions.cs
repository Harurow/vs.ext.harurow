using System.Windows;

namespace Harurow.Extensions.One.Extensions
{
    internal static class FreezableExtensions
    {
        public static T FreezeAnd<T>(this T self)
            where T : Freezable
        {
            self.Freeze();
            return self;
        }
    }
}