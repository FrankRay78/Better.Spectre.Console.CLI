using System.IO;

namespace Better.Spectre.Console.CLI.Tests.Data;

public class HorseSettings : MammalSettings
{
    [CommandOption("-d|--day <Mon|Tue>")]
    public DayOfWeek Day { get; set; }

    [CommandOption("--file")]
    [DefaultValue("food.txt")]
    public FileInfo File { get; set; }

    [CommandOption("--directory")]
    public DirectoryInfo Directory { get; set; }
}