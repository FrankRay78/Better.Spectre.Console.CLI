namespace Better.Spectre.Console.CLI;

internal sealed class StringWriterWithEncoding : StringWriter
{
    public override Encoding Encoding { get; }

    public StringWriterWithEncoding(Encoding encoding)
    {
        Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
    }
}