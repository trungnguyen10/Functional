namespace Functional.Option;

/// <summary>
/// An abstraction to model a value that may or may not be present (aka optional) of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Option<T>
{
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
    => this switch
    {
        Some<T> aSome => some(aSome.Reduce()),
        None<T> _ => none(),
        _ => throw new InvalidOperationException($"{nameof(Option<T>)} can onlt be either {nameof(Some<T>)} or {nameof(None<T>)}")
    };
}

/// <summary>
/// Model a value of type <typeparamref name="T"/> that is present (not null)
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class Some<T>(T value) : Option<T>
{
    private readonly T? _value = value switch
    {
        null => throw new ArgumentNullException($"cannot construct {nameof(Some<T>)} with null value", nameof(value)),
        _ => value
    };

    public T Reduce() => _value!;

    public override bool Equals(object? obj)
    => obj switch
    {
        Some<T> other => object.Equals(this._value, other._value),
        _ => false
    };

    public override int GetHashCode() => _value!.GetHashCode();
}

/// <summary>
///Model a value of type <typeparamref name="T"/> that is absent (null)
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class None<T> : Option<T>
{
    public override bool Equals(object? obj)
    => obj switch
    {
        None<T> other => true,
        _ => false
    };

    public override int GetHashCode() => 0;
}


