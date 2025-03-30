namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class InvalidCommand : Command<InvalidSettings>
{
    public override int Execute(CommandContext context, InvalidSettings settings)
    {
        return 0;
    }
}
