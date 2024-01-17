namespace Functional.Option;

public static class OptionExtensions
{
    /// <summary>
    /// A utility method that create <see cref="Some{T}"/> of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">the value</param>
    /// <returns><see cref="Some{T}"/> of <typeparamref name="T"/></returns>
    public static Option<T> SomeOf<T>(T t) => new Some<T>(t);

    /// <summary>
    /// A utility method that create <see cref="None{T}"/> of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns><see cref="None{T}"/> of <typeparamref name="T"/></returns>
    public static Option<T> NoneOf<T>() => new None<T>();

    /// <summary>
    /// A utility method that create <see cref="Option{T}"/> of <typeparamref name="T"/>.
    /// It can only be <see cref="Some{T}"/> of <typeparamref name="T"/> or <see cref="None{T}"/> of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">the value</param>
    /// <returns><see cref="Some{T}"/> of <typeparamref name="T"/> or <see cref="None{T}"/> of <typeparamref name="T"/> depends on
    /// the nullability of <paramref name="t"/></returns>
    public static Option<T> OptionOf<T>(T? t) => t switch
    {
        null => NoneOf<T>(),
        _ => SomeOf(t)
    };

    /// <summary>
    /// Maps <see cref="Option{TSource}"/> of type <typeparamref name="TSource"/> to <see cref="Option{TResult}"/> of type <typeparamref name="TResult"/>
    ///  using the transform function
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="source">A Option of type <typeparamref name="TSource"/> to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to the value of the <paramref name="source"/></param>
    /// <returns>An <see cref="Option{TResult}"/> whose the value are the result of invoking the transform
    /// function on the value of <paramref name="source"/>.</returns>
    public static Option<TResult> Select<TSource, TResult>(this Option<TSource> source, Func<TSource, TResult> selector)
    => source.Match<Option<TResult>>(
        some: s => new Some<TResult>(selector(s)),
        none: () => new None<TResult>()
    );

    /// <summary>
    /// Maps <see cref="Option{TSource}"/> to new form of <see cref="Option{Option{TResult}}"/> 
    ///  using the transform function, and flatten the result into a non-nested <see cref="Option{TResult}"/>
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="source">A Option of type <typeparamref name="TSource"/> to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to the value of the <paramref name="source"/></param>
    /// <returns>An non-nested <see cref="Option{TResult}"/> whose the value are the result of flattening the result of invoking the transform
    /// function on the value of <paramref name="source"/>.</returns>
    public static Option<TResult> SelectMany<T, TResult>(this Option<T> optionT, Func<T, Option<TResult>> selector)
    => optionT.Match(
        some: s => selector(s),
        none: () => new None<TResult>()
    );

    /// <summary>
    /// Convert the <see cref="Option{T}"/> to <see cref="IEnumerable{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <returns>The <see cref="IEnumerable{T}"/> that has either one or no element</returns>
    public static IEnumerable<T> AsEnumerable<T>(this Option<T> option) => option.Match(
        some: s => [s],
        none: () => Array.Empty<T>()
        );
}
