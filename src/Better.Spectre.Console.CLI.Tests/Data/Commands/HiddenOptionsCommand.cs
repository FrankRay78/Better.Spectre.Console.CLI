namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class HiddenOptionsCommand : Command<HiddenOptionSettings>
{
    public override int Execute(CommandContext context, HiddenOptionSettings settings)
    {
        return 0;
    }
}
