using System;

namespace Harurow.Extensions.Commands.Behaviors
{
    internal static class CommandSet
    {
        public static readonly Guid MenuGroupGuid = new Guid("acf71306-9fc1-4f6b-9bec-7c2500e2dc8b");

        public static class Ids
        {
            public const int SetEncodingToUtf8WithBom = 0x100;
            public const int DuplicationLine = 0x101;
        }
    }
}