namespace Better.Spectre.Console.CLI;

// Consider removing this in favor for value tuples at some point.
internal sealed class MappedCommandParameter
{
    public CommandParameter Parameter { get; }
    public string? Value { get; }

    public MappedCommandParameter(CommandParameter parameter, string? value)
    {
        Parameter = parameter;
        Value = value;
    }
}