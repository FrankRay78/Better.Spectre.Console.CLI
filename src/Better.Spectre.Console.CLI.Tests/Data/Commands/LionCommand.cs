namespace Better.Spectre.Console.CLI.Tests.Data;

[Description("The lion command.")]
public class LionCommand : AnimalCommand<LionSettings>
{
    public override int Execute(CommandContext context, LionSettings settings)
    {
        return 0;
    }
}
