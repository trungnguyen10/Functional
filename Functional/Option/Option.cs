namespace Functional.Option;

/// <summary>
/// An abstraction to model a value that may or may not be present (aka optional) of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOption<T>
{
    TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none);
}

/// <summary>
/// Model a value of type <typeparamref name="T"/> that is present (not null)
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Some<T>(T value) : IOption<T>
{
    private readonly T? _value = value switch
    {
        null => throw new ArgumentNullException(nameof(value), $"cannot construct {nameof(Some<T>)} with null value"),
        _ => value
    };

    public T Reduce() => _value ?? throw new InvalidOperationException($"{nameof(Some<T>)} with null value");

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Some<T> other => Reduce()!.Equals(other.Reduce()),
            _ => false
        };
    }

    public override int GetHashCode() => Reduce()!.GetHashCode();

    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
    {
        return some(Reduce());
    }

    public static bool operator ==(Some<T> left, Some<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Some<T> left, Some<T> right)
    {
        return !(left == right);
    }
}

/// <summary>
/// Model a value of type <typeparamref name="T"/> that is absent (null)
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct None<T> : IOption<T>
{
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            None<T> other => true,
            _ => false
        };
    }

    public override int GetHashCode() => 0;

    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
    {
        return none();
    }

    public static bool operator ==(None<T> left, None<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(None<T> left, None<T> right)
    {
        return !(left == right);
    }
}


