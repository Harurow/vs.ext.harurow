using System;

namespace Harurow.Extensions.MultipleSelection.Behaviors
{
    internal static class CommandSet
    {
        public static readonly Guid MenuGroupGuid = new Guid("6ddf1cac-4b42-42c5-8d75-17aa2c43197a");

        public static class Ids
        {
            public const int AddSelectionToNextFindMatch = 0x100;
            public const int MoveSelectionToNextFindMatch = 0x101;
            public const int CursorUndo = 0x102;
        }
    }
}