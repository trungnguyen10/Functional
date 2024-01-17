using Functional.Option;

namespace Functional;

public static class IEnumerableExtension
{
    /// <summary>
    /// Projects each element of a sequence to an <see cref="Option{TResult}"/>
    /// then flattens and filters out the <see cref="None{TResult}"/> values in the results to a new sequence.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="f">A transform function to apply to each element</param>
    /// <returns>An <see cref="IEnumerable{TResult}"/> whose elements are the result of invoking the transform function on each element of the source 
    /// and filtering out <see cref="None{TResult}"/> values</returns>
    public static IEnumerable<TResult> SelectMany<T, TResult>(this IEnumerable<T> source, Func<T, Option<TResult>> f)
    => source.SelectMany(i => f(i).AsEnumerable());
}
