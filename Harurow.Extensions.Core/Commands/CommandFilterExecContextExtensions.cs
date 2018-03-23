using Microsoft.VisualStudio;

namespace Harurow.Extensions.Commands
{
    public static class CommandFilterExecContextExtensions
    {
        public static bool IsCancel(this ICommandFilterExecContext self)
            => self.MenuGroupGuid == VSConstants.VSStd2K && self.CommandId == (uint) VSConstants.VSStd2KCmdID.CANCEL;

        public static bool IsCopy(this ICommandFilterExecContext self)
            => self.MenuGroupGuid == VSConstants.VSStd2K && self.CommandId == (uint) VSConstants.VSStd2KCmdID.COPY ||
               self.MenuGroupGuid == VSConstants.GUID_VSStandardCommandSet97 &&
               self.CommandId == (uint) VSConstants.VSStd97CmdID.Copy;

        public static bool IsCut(this ICommandFilterExecContext self)
            => self.MenuGroupGuid == VSConstants.VSStd2K && self.CommandId == (uint) VSConstants.VSStd2KCmdID.CUT ||
               self.MenuGroupGuid == VSConstants.GUID_VSStandardCommandSet97 &&
               self.CommandId == (uint) VSConstants.VSStd97CmdID.Cut;

        public static bool IsPaste(this ICommandFilterExecContext self)
        {
            if (self.MenuGroupGuid == VSConstants.VSStd2K)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd2KCmdID.PASTE:
                    case (uint) VSConstants.VSStd2KCmdID.PASTEASHTML:
                        return true;
                }
            }
            else if (self.MenuGroupGuid == VSConstants.GUID_VSStandardCommandSet97)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd97CmdID.Paste:
                        return true;
                }
            }

            return false;
        }

        public static bool IsClipboard(this ICommandFilterExecContext self)
        {
            if (self.MenuGroupGuid == VSConstants.VSStd2K)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd2KCmdID.COPY:
                    case (uint) VSConstants.VSStd2KCmdID.CUT:
                    case (uint) VSConstants.VSStd2KCmdID.PASTE:
                    case (uint) VSConstants.VSStd2KCmdID.PASTEASHTML:
                        return true;
                }
            }
            else if (self.MenuGroupGuid == VSConstants.GUID_VSStandardCommandSet97)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd97CmdID.Copy:
                    case (uint) VSConstants.VSStd97CmdID.Cut:
                    case (uint) VSConstants.VSStd97CmdID.Paste:
                        return true;
                }
            }

            return false;
        }

        public static bool IsNotSupport(this ICommandFilterExecContext self)
        {
            if (self.MenuGroupGuid == VSConstants.VSStd2K)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd2KCmdID.HOME:
                    case (uint) VSConstants.VSStd2KCmdID.END:
                    case (uint) VSConstants.VSStd2KCmdID.PAGEDN:
                    case (uint) VSConstants.VSStd2KCmdID.PAGEUP:
                    case (uint) VSConstants.VSStd2KCmdID.SELECTALL:
                    case (uint) VSConstants.VSStd2KCmdID.UP_EXT_COL:
                    case (uint) VSConstants.VSStd2KCmdID.DOWN_EXT_COL:
                    case (uint) VSConstants.VSStd2KCmdID.LEFT_EXT_COL:
                    case (uint) VSConstants.VSStd2KCmdID.RIGHT_EXT_COL:
                    case (uint) VSConstants.VSStd2KCmdID.BOL_EXT_COL:
                    case (uint) VSConstants.VSStd2KCmdID.EOL_EXT_COL:
                    case (uint) VSConstants.VSStd2KCmdID.WORDPREV_EXT_COL:
                    case (uint) VSConstants.VSStd2KCmdID.WORDNEXT_EXT_COL:
                        return true;
                }
            }
            else if (self.MenuGroupGuid == VSConstants.GUID_VSStandardCommandSet97)
            {
                switch (self.CommandId)
                {
                    case (uint)VSConstants.VSStd97CmdID.Undo:
                    case (uint)VSConstants.VSStd97CmdID.MultiLevelUndo:
                    case (uint)VSConstants.VSStd97CmdID.MultiLevelUndoList:
                    case (uint)VSConstants.VSStd97CmdID.MultiLevelRedo:
                    case (uint)VSConstants.VSStd97CmdID.MultiLevelRedoList:
                        return true;
                }
            }

            return false;
        }

        public static bool IsEdit(this ICommandFilterExecContext self)
        {
            if (self.MenuGroupGuid == VSConstants.VSStd2K)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd2KCmdID.TYPECHAR:
                    case (uint) VSConstants.VSStd2KCmdID.RETURN:
                    case (uint) VSConstants.VSStd2KCmdID.TAB:
                    case (uint) VSConstants.VSStd2KCmdID.BACKTAB:
                    case (uint) VSConstants.VSStd2KCmdID.BACKSPACE:
                    case (uint) VSConstants.VSStd2KCmdID.DELETE:
                    case (uint) VSConstants.VSStd2KCmdID.DELETEWORDLEFT:
                    case (uint) VSConstants.VSStd2KCmdID.DELETEWORDRIGHT:
                    case (uint) VSConstants.VSStd2KCmdID.DELETELINE:
                    case (uint) VSConstants.VSStd2KCmdID.DELETETOBOL:
                    case (uint) VSConstants.VSStd2KCmdID.DELETETOEOL:
                    case (uint) VSConstants.VSStd2KCmdID.DELETEWHITESPACE:
                    case (uint) VSConstants.VSStd2KCmdID.SELUPCASE:
                    case (uint) VSConstants.VSStd2KCmdID.SELLOWCASE:
                        return true;
                }
            }
            else if (self.MenuGroupGuid == VSConstants.GUID_VSStandardCommandSet97)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd97CmdID.Delete:
                        return true;
                }
            }

            return false;
        }

        public static bool IsMove(this ICommandFilterExecContext self)
        {
            if (self.MenuGroupGuid == VSConstants.VSStd2K)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd2KCmdID.LEFT:
                    case (uint) VSConstants.VSStd2KCmdID.RIGHT:
                    case (uint) VSConstants.VSStd2KCmdID.UP:
                    case (uint) VSConstants.VSStd2KCmdID.DOWN:
                    case (uint) VSConstants.VSStd2KCmdID.BOL:
                    case (uint) VSConstants.VSStd2KCmdID.EOL:
                    case (uint) VSConstants.VSStd2KCmdID.WORDPREV:
                    case (uint) VSConstants.VSStd2KCmdID.WORDNEXT:
                    case (uint) VSConstants.VSStd2KCmdID.FIRSTCHAR:
                    case (uint) VSConstants.VSStd2KCmdID.LASTCHAR:
                        return true;
                }
            }

            return false;
        }

        public static bool IsExtendSelection(this ICommandFilterExecContext self)
        {
            if (self.MenuGroupGuid == VSConstants.VSStd2K)
            {
                switch (self.CommandId)
                {
                    case (uint) VSConstants.VSStd2KCmdID.LEFT_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.RIGHT_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.UP_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.DOWN_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.BOL_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.EOL_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.WORDPREV_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.WORDNEXT_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.FIRSTCHAR_EXT:
                    case (uint) VSConstants.VSStd2KCmdID.LASTCHAR_EXT:
                        return true;
                }
            }

            return false;
        }
    }
}