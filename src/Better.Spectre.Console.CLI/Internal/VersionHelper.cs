namespace Better.Spectre.Console.CLI;

internal static class VersionHelper
{
    public static string GetVersion(Assembly? assembly)
    {
        return assembly?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "?";
    }
}