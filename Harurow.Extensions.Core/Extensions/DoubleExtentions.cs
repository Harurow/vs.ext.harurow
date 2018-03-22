namespace Harurow.Extensions.Extensions
{
    public static class DoubleExtentions
    {
        public static bool IsNotEquals(this double self, double b)
            => self > b || self < b;

        public static bool IsEquals(this double self, double b)
            => !self.IsNotEquals(b);
    }
}