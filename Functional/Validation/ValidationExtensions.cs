namespace Functional.Validation;

public static class ValidationExtensions
{
    public static IValidation<TResult> Select<TSource, TResult>(this IValidation<TSource> source, Func<TSource, TResult> selector)
    {
        return source.Match<IValidation<TResult>>(
            onInvalid: e => new Invalid<TResult>(e),
            onValid: v => new Valid<TResult>(selector(v))
        );
    }

    public static IValidation<TResult> SelectMany<TSource, TResult>(this IValidation<TSource> source, Func<TSource, IValidation<TResult>> selector)
    {
        return source.Match(
            onInvalid: (e) => new Invalid<TResult>(e),
            onValid: s => selector(s)
        );
    }

    public static IValidation<TResult> SelectMany<TSource, TOption, TResult>(this IValidation<TSource> source, Func<TSource, IValidation<TOption>> validationSelector, Func<TSource, TOption, TResult> resultSelector)
    {
        return source.Match(
            onInvalid: e => new Invalid<TResult>(e),
            onValid: s => validationSelector(s).Select(o => resultSelector(s, o))
        );
    }

    public static Func<T, IValidation<T>> Validate<T>(IEnumerable<Func<T, IEnumerable<ValidationError>>> validators)
    {
        return t =>
        {
            var errors = validators.SelectMany(validator => validator(t)).ToArray();
            return errors switch
            {
            [] => new Valid<T>(t),
                _ => new Invalid<T>(errors)
            };
        };
    }
}
