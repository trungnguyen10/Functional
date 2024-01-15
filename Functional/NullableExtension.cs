namespace Functional;

/// <summary>
/// A simple implementation of functional core functions to handle null reference.
/// </summary>
public static class NullableExtension
{
    public static TResult? Select<T, TResult>(this T? t, Func<T, TResult> f) where T : notnull where TResult : notnull
    => t switch
    {
        null => default,
        _ => f(t)
    };

    public static TResult? SelectMany<T, TResult>(this T? t, Func<T, TResult?> f) where T : notnull where TResult : notnull
    => t switch
    {
        null => default,
        _ => f(t)
    };

    public static T Reduce<T>(this T? t, T orElse) where T : notnull
    => t switch
    {
        null => orElse ?? throw new NullReferenceException(nameof(orElse)),
        _ => t
    };

    public static T Reduce<T>(this T? t, Func<T> f) where T : notnull
    => t switch
    {
        null => f() ?? throw new InvalidOperationException($"{nameof(f)} yields to null"),
        _ => t
    };
}
