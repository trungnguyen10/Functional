namespace Functional;

/// <summary>
/// A simple implementation of functional core functions to handle null reference.
/// </summary>
public static class NullableExtension
{
    public static TResult? Map<T, TResult>(this T? t, Func<T, TResult> f) where T : notnull where TResult : notnull
    => t switch
    {
        null => default,
        _ => f(t)
    };

    public static TResult? Bind<T, TResult>(this T? t, Func<T, TResult?> f) where T : notnull where TResult : notnull
    => t switch
    {
        null => default,
        _ => f(t)
    };
}
