using System.ComponentModel.Design;
using Harurow.Extensions.MultipleSelection.Behaviors.Filters;

namespace Harurow.Extensions.MultipleSelection.Behaviors
{
    internal static class Bootstrap
    {
        public static void Initialize(IMenuCommandService commandService)
        {
            commandService.AddCommand(new EmptyMenuCommand(CommandSet.Ids.AddSelectionToNextFindMatch));
            commandService.AddCommand(new EmptyMenuCommand(CommandSet.Ids.MoveSelectionToNextFindMatch));
        }
    }
}