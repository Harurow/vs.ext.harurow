using System.ComponentModel.Design;
using Harurow.Extensions.Commands.Behaviors.Commands;

namespace Harurow.Extensions.Commands.Behaviors
{
    internal static class Bootstrap
    {
        public static void Initialize(IMenuCommandService commandService)
        {
            // ReSharper disable ObjectCreationAsStatement
            new DuplicateLineCommand(commandService);
            new SetUtf8WithBomCommand(commandService);
            // ReSharper disable restore ObjectCreationAsStatement
        }
    }
}