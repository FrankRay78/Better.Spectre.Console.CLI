using static Better.Spectre.Console.CLI.CommandTreeTokenizer;

namespace Better.Spectre.Console.CLI;

internal class CommandTreeParser
{
    private readonly CommandModel _configuration;
    private readonly ParsingMode _parsingMode;
    private readonly CommandOptionAttribute _help;
    private readonly bool _convertFlagsToRemainingArguments;

    public CaseSensitivity CaseSensitivity { get; }

    public enum State
    {
        Normal = 0,
        Remaining = 1,
    }

    public CommandTreeParser(CommandModel configuration, CaseSensitivity caseSensitivity, ParsingMode? parsingMode = null, bool? convertFlagsToRemainingArguments = null)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _parsingMode = parsingMode ?? _configuration.ParsingMode;
        _help = new CommandOptionAttribute("-?|-h|--help");
        _convertFlagsToRemainingArguments = convertFlagsToRemainingArguments ?? false;

        CaseSensitivity = caseSensitivity;
    }

    public CommandTreeParserResult Parse(IEnumerable<string> args)
    {
        var parserContext = new CommandTreeParserContext(args, _parsingMode);
        var tokenizerResult = CommandTreeTokenizer.Tokenize(args);

        return Parse(parserContext, tokenizerResult);
    }

    public CommandTreeParserResult Parse(CommandTreeParserContext context, CommandTreeTokenizerResult tokenizerResult)
    {
        var tokens = tokenizerResult.Tokens;
        var rawRemaining = tokenizerResult.Remaining;

        var result = default(CommandTree);
        if (tokens.Count > 0)
        {
            // Not a command?
            var token = tokens.Current;
            if (token == null)
            {
                // Should not happen, but the compiler isn't
                // smart enough to realize this...
                throw new CommandRuntimeException("Could not get current token.");
            }

            if (token.TokenKind != CommandTreeToken.Kind.String)
            {
                // Got a default command?
                if (_configuration.DefaultCommand != null)
                {
                    result = ParseCommandParameters(context, _configuration.DefaultCommand, null, tokens);
                    return new CommandTreeParserResult(
                        result, new RemainingArguments(context.GetRemainingArguments(), rawRemaining));
                }

                // Show help?
                if (_help?.IsMatch(token.Value) == true)
                {
                    return new CommandTreeParserResult(
                        null, new RemainingArguments(context.GetRemainingArguments(), rawRemaining));
                }

                // Unexpected option.
                throw CommandParseException.UnexpectedOption(context.Arguments, token);
            }

            // Does the token value match a command?
            var command = _configuration.FindCommand(token.Value, CaseSensitivity);
            if (command == null)
            {
                if (_configuration.DefaultCommand != null)
                {
                    result = ParseCommandParameters(context, _configuration.DefaultCommand, null, tokens);
                    return new CommandTreeParserResult(
                        result, new RemainingArguments(context.GetRemainingArguments(), rawRemaining));
                }
            }

            // Parse the command.
            result = ParseCommand(context, _configuration, null, tokens);
        }
        else
        {
            // Is there a default command?
            if (_configuration.DefaultCommand != null)
            {
                result = ParseCommandParameters(context, _configuration.DefaultCommand, null, tokens);
            }
        }

        return new CommandTreeParserResult(
            result, new RemainingArguments(context.GetRemainingArguments(), rawRemaining));
    }

    private CommandTree ParseCommand(
        CommandTreeParserContext context,
        ICommandContainer current,
        CommandTree? parent,
        CommandTreeTokenStream stream)
    {
        // Find the command.
        var commandToken = stream.Consume(CommandTreeToken.Kind.String);
        if (commandToken == null)
        {
            throw new CommandRuntimeException("Could not consume token when parsing command.");
        }

        var command = current.FindCommand(commandToken.Value, CaseSensitivity);
        if (command == null)
        {
            throw CommandParseException.UnknownCommand(_configuration, parent, context.Arguments, commandToken);
        }

        return ParseCommandParameters(context, command, parent, stream);
    }

    private CommandTree ParseCommandParameters(
        CommandTreeParserContext context,
        CommandInfo command,
        CommandTree? parent,
        CommandTreeTokenStream stream)
    {
        context.ResetArgumentPosition();

        var node = new CommandTree(parent, command);
        while (stream.Peek() != null)
        {
            var token = stream.Peek();
            if (token == null)
            {
                // Should not happen, but the compiler isn't
                // smart enough to realize this...
                throw new CommandRuntimeException("Could not get the next token.");
            }

            switch (token.TokenKind)
            {
                case CommandTreeToken.Kind.LongOption:
                    // Long option
                    ParseOption(context, stream, token, node, true);
                    break;
                case CommandTreeToken.Kind.ShortOption:
                    // Short option
                    ParseOption(context, stream, token, node, false);
                    break;
                case CommandTreeToken.Kind.String:
                    // Command
                    ParseString(context, stream, node);
                    break;
                case CommandTreeToken.Kind.Remaining:
                    // Remaining
                    stream.Consume(CommandTreeToken.Kind.Remaining);
                    context.State = State.Remaining;
                    break;
                default:
                    throw new InvalidOperationException($"Encountered unknown token ({token.TokenKind}).");
            }
        }

        // Add unmapped parameters.
        foreach (var parameter in node.Command.Parameters)
        {
            if (node.Mapped.All(m => m.Parameter != parameter))
            {
                node.Unmapped.Add(parameter);
            }
        }

        return node;
    }

    private void ParseString(
        CommandTreeParserContext context,
        CommandTreeTokenStream stream,
        CommandTree node)
    {
        if (context.State == State.Remaining)
        {
            stream.Consume(CommandTreeToken.Kind.String);
            return;
        }

        var token = stream.Expect(CommandTreeToken.Kind.String);

        // Command?
        var command = node.Command.FindCommand(token.Value, CaseSensitivity);
        if (command != null)
        {
            if (context.State == State.Normal)
            {
                node.Next = ParseCommand(context, node.Command, node, stream);
            }

            return;
        }

        // Current command has no arguments?
        if (!node.HasArguments())
        {
            throw CommandParseException.UnknownCommand(_configuration, node, context.Arguments, token);
        }

        // Argument?
        var parameter = node.FindArgument(context.CurrentArgumentPosition);
        if (parameter == null)
        {
            // No parameters left. Any commands after this?
            if (node.Command.Children.Count > 0 || node.Command.IsDefaultCommand)
            {
                throw CommandParseException.UnknownCommand(_configuration, node, context.Arguments, token);
            }

            throw CommandParseException.CouldNotMatchArgument(context.Arguments, token);
        }

        // Yes, this was an argument.
        if (parameter.ParameterKind == ParameterKind.Vector)
        {
            // Vector
            var current = stream.Current;
            while (current?.TokenKind == CommandTreeToken.Kind.String)
            {
                var value = stream.Consume(CommandTreeToken.Kind.String)?.Value;
                node.Mapped.Add(new MappedCommandParameter(parameter, value));
                current = stream.Current;
            }
        }
        else
        {
            // Scalar
            var value = stream.Consume(CommandTreeToken.Kind.String)?.Value;
            node.Mapped.Add(new MappedCommandParameter(parameter, value));
            context.IncreaseArgumentPosition();
        }
    }

    private void ParseOption(
        CommandTreeParserContext context,
        CommandTreeTokenStream stream,
        CommandTreeToken token,
        CommandTree node,
        bool isLongOption)
    {
        // Consume the option token.
        stream.Consume(isLongOption ? CommandTreeToken.Kind.LongOption : CommandTreeToken.Kind.ShortOption);

        if (context.State == State.Normal)
        {
            // Find the option.
            var option = node.FindOption(token.Value, isLongOption, CaseSensitivity);
            if (option != null)
            {
                ParseOptionValue(context, stream, token, node, option);
                return;
            }

            // Help?
            if (_help?.IsMatch(token.Value) == true)
            {
                node.ShowHelp = true;
                return;
            }
        }

        if (context.State == State.Remaining)
        {
            ParseOptionValue(context, stream, token, node);
            return;
        }

        if (context.ParsingMode == ParsingMode.Strict)
        {
            throw CommandParseException.UnknownOption(context.Arguments, token);
        }
        else
        {
            ParseOptionValue(context, stream, token, node);
        }
    }

    private void ParseOptionValue(
        CommandTreeParserContext context,
        CommandTreeTokenStream stream,
        CommandTreeToken token,
        CommandTree current,
        CommandParameter? parameter = null)
    {
        bool addToMappedCommandParameters = parameter != null;

        var value = default(string);

        // Parse the value of the token (if any).
        var valueToken = stream.Peek();
        if (valueToken?.TokenKind == CommandTreeToken.Kind.String)
        {
            bool parseValue = token is not { TokenKind: CommandTreeToken.Kind.ShortOption, IsGrouped: true };

            if (context.State == State.Normal && parseValue)
            {
                // Is this a command?
                if (current.Command.FindCommand(valueToken.Value, CaseSensitivity) == null)
                {
                    if (parameter != null)
                    {
                        if (parameter.ParameterKind == ParameterKind.Flag)
                        {
                            if (!CliConstants.AcceptedBooleanValues.Contains(valueToken.Value, StringComparer.OrdinalIgnoreCase))
                            {
                                if (!valueToken.HadSeparator)
                                {
                                    // Do nothing
                                    // - assume valueToken is unrelated to the flag parameter (ie. we've parsed it unnecessarily)
                                    // - rely on the "No value?" code below to set the flag to its default value
                                    // - valueToken will be handled on the next pass of the parser
                                }
                                else
                                {
                                    // Flags cannot be assigned a value.
                                    if (_convertFlagsToRemainingArguments)
                                    {
                                        value = stream.Consume(CommandTreeToken.Kind.String)?.Value;

                                        context.AddRemainingArgument(token.Representation, value);

                                        // Prevent the option and it's non-boolean value from being added to
                                        // mapped parameters (otherwise an exception will be thrown later
                                        // when binding the value to the flag in the comand settings)
                                        addToMappedCommandParameters = false;
                                    }
                                    else
                                    {
                                        throw CommandParseException.CannotAssignValueToFlag(context.Arguments, token);
                                    }
                                }
                            }
                            else
                            {
                                value = stream.Consume(CommandTreeToken.Kind.String)?.Value;
                            }
                        }
                        else
                        {
                            value = stream.Consume(CommandTreeToken.Kind.String)?.Value;
                        }
                    }
                    else
                    {
                        // Unknown parameter value.
                        value = stream.Consume(CommandTreeToken.Kind.String)?.Value;

                        // In relaxed parsing mode?
                        if (context.ParsingMode == ParsingMode.Relaxed)
                        {
                            context.AddRemainingArgument(token.Representation, value);
                        }
                    }
                }
            }
            else
            {
                context.AddRemainingArgument(token.Representation, parseValue ? valueToken.Value : null);
            }
        }
        else
        {
            if (parameter == null && // Only add tokens which have not been matched to a command parameter
                (context.State == State.Remaining || context.ParsingMode == ParsingMode.Relaxed))
            {
                context.AddRemainingArgument(token.Representation, null);
            }
        }

        // No value?
        if (context.State == State.Normal)
        {
            if (value == null && parameter != null)
            {
                if (parameter.ParameterKind == ParameterKind.Flag)
                {
                    value = "true";
                }
                else
                {
                    if (parameter is CommandOption option)
                    {
                        if (parameter.IsFlagValue())
                        {
                            value = null;
                        }
                        else
                        {
                            throw CommandParseException.OptionHasNoValue(context.Arguments, token, option);
                        }
                    }
                    else
                    {
                        // This should not happen at all. If it does, it's because we've added a new
                        // option type which isn't a CommandOption for some reason.
                        throw new InvalidOperationException($"Found invalid parameter type '{parameter.GetType().FullName}'.");
                    }
                }
            }
        }

        if (parameter != null && addToMappedCommandParameters)
        {
            current.Mapped.Add(new MappedCommandParameter(parameter, value));
        }
    }
}