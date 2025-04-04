namespace Better.Spectre.Console.CLI;

internal static class TypeRegistrarExtensions
{
    public static void RegisterDependencies(this ITypeRegistrar registrar, CommandModel model)
    {
        var stack = new Stack<CommandInfo>();
        model.Commands.ForEach(c => stack.Push(c));

        while (stack.Count > 0)
        {
            var command = stack.Pop();

            if (command.SettingsType == null)
            {
                // TODO: Error message
                throw new InvalidOperationException("Command setting type cannot be null.");
            }

            if (command.SettingsType is { IsAbstract: false, IsClass: true })
            {
                // Register the settings type
                registrar?.Register(command.SettingsType, command.SettingsType);
            }

            if (command.CommandType != null)
            {
                registrar?.Register(command.CommandType, command.CommandType);
            }

            foreach (var parameter in command.Parameters)
            {
                var pairDeconstructor = parameter?.PairDeconstructor?.Type;
                if (pairDeconstructor != null)
                {
                    registrar?.Register(pairDeconstructor, pairDeconstructor);
                }

                var typeConverterTypeName = parameter?.Converter?.ConverterTypeName;
                if (!string.IsNullOrWhiteSpace(typeConverterTypeName))
                {
                    var typeConverterType = Type.GetType(typeConverterTypeName);
                    Debug.Assert(typeConverterType != null, "Could not create type");
                    registrar?.Register(typeConverterType, typeConverterType);
                }
            }

            foreach (var child in command.Children)
            {
                stack.Push(child);
            }
        }
    }
}