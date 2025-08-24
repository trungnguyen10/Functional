namespace Functional;

public static class FuncExtensions
{
    public static Func<T1,Func<T2,TResult>> Curry<T1,T2,TResult>(this Func<T1,T2,TResult> f) => t1 => t2 => f(t1, t2);
}