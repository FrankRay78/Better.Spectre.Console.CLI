namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class ThrowingCommand : Command<ThrowingCommandSettings>
{
    public override int Execute(CommandContext context, ThrowingCommandSettings settings)
    {
        throw new InvalidOperationException("W00t?");
    }
}