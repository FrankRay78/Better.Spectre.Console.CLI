namespace Better.Spectre.Console.CLI.Tests.Unit.Cli.Annotations;

[ExpectationPath("Arguments")]
public sealed partial class CommandArgumentAttributeTests
{
    public sealed class ArgumentCannotContainOptions
    {
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "--foo <BAR>")]
            public string Foo { get; set; }
        }

        [Fact]
        [Expectation("ArgumentCannotContainOptions")]
        public Task Should_Return_Correct_Text()
        {
            // Given, When
            var result = Fixture.Run<Settings>();

            // Then
            return Verifier.Verify(result);
        }
    }

    public sealed class MultipleValuesAreNotSupported
    {
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<FOO> <BAR>")]
            public string Foo { get; set; }
        }

        [Fact]
        [Expectation("MultipleValuesAreNotSupported")]
        public Task Should_Return_Correct_Text()
        {
            // Given, When
            var result = Fixture.Run<Settings>();

            // Then
            return Verifier.Verify(result);
        }
    }

    public sealed class ValuesMustHaveName
    {
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<>")]
            public string Foo { get; set; }
        }

        [Fact]
        [Expectation("ValuesMustHaveName")]
        public Task Should_Return_Correct_Text()
        {
            // Given, When
            var result = Fixture.Run<Settings>();

            // Then
            return Verifier.Verify(result);
        }
    }

    private static class Fixture
    {
        public static string Run<TSettings>(params string[] args)
            where TSettings : CommandSettings
        {
            using (var writer = new TestConsole())
            {
                var app = new CommandApp();
                app.Configure(c => c.ConfigureConsole(writer));
                app.Configure(c => c.AddCommand<GenericCommand<TSettings>>("foo"));
                app.Run(args);

                return writer.Output
                    .NormalizeLineEndings()
                    .TrimLines()
                    .Trim();
            }
        }
    }
}
