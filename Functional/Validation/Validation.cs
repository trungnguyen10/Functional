using System.Collections;

namespace Functional.Validation;

public interface IValidation<T>
{
    TResult Match<TResult>(Func<IEnumerable<ValidationError>, TResult> onInvalid, Func<T, TResult> onValid);
}

public readonly struct Valid<T>(T value) : IValidation<T>
{
    private readonly T? _value = value switch
    {
        null => throw new ArgumentNullException(nameof(value), $"cannot construct {nameof(Valid<T>)} with null value"),
        _ => value
    };

    public T Reduce() => _value ?? throw new InvalidOperationException($"{nameof(Valid<T>)} with null value");

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            Valid<T> other => Reduce()!.Equals(other.Reduce()),
            _ => false
        };
    }

    public override int GetHashCode() => Reduce()!.GetHashCode();

    public TResult Match<TResult>(Func<IEnumerable<ValidationError>, TResult> onInvalid, Func<T, TResult> onValid)
    {
        return onValid(Reduce());
    }

    public static bool operator ==(Valid<T> left, Valid<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Valid<T> left, Valid<T> right)
    {
        return !(left == right);
    }
}

public readonly struct Invalid<T>(IEnumerable<ValidationError> errors) : IValidation<T>, IEnumerable<ValidationError>
{
    private readonly ValidationError[] _errors = errors.Any() ? errors.ToArray() : throw new InvalidOperationException($"sequence {nameof(errors)} contains no element");

    public Invalid(ValidationError error) : this([error]) { }

    public IEnumerable<ValidationError> AsEnumerable()
    {
        return _errors.Length > 0 ? errors.ToArray() : throw new InvalidOperationException($"sequence {nameof(errors)} contains no element");
    }

    public IEnumerator<ValidationError> GetEnumerator()
    {
        return AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return AsEnumerable().GetEnumerator();
    }

    public TResult Match<TResult>(Func<IEnumerable<ValidationError>, TResult> onInvalid, Func<T, TResult> onValid)
    {
        return onInvalid(AsEnumerable());
    }
}