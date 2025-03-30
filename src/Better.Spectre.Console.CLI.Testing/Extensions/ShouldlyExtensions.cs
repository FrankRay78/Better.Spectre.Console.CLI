namespace Better.Spectre.Console.CLI.Testing;

/// <summary>
/// Provides extension methods to improve the readability of inline assertions using Shouldly.
/// </summary>
public static class ShouldlyExtensions
{
    /// <summary>
    /// Invokes the specified action on the given object and then returns the object.
    /// This is useful for chaining assertions or other operations fluently without breaking the chain.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="item">The object on which to invoke the action.</param>
    /// <param name="action">The action to perform on the object.</param>
    /// <returns>The original object, after the action has been performed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <c>null</c>.</exception>
    [DebuggerStepThrough]
    public static T And<T>(this T item, Action<T> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action(item);
        return item;
    }
}