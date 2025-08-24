namespace Functional.Tasks;

public static class TaskExtensions
{
    public static async Task<TResult> Apply<T, TResult>(this Task<Func<T, TResult>> funcTask, Task<T> valueTask) =>
        (await funcTask)(await valueTask);

    public static Task<Func<T2, TResult>> Apply<T1, T2, TResult>(this Task<Func<T1, T2, TResult>> task,
        Task<T1> value) =>
        Apply(task.Select(FuncExtensions.Curry), value);


    public static async Task<TResult> Select<T, TResult>(this Task<T> task, Func<T, TResult> selector) =>
        selector(await task);

    public static Task<TResult> SelectMany<TSource, TResult>(
        this Task<TSource> source,
        Func<TSource, Task<TResult>> bind)
    {
        return SelectMany(source, bind, (s, t) => t);
    }

    public static async Task<TResult> SelectMany<TSource, TBind, TResult>(
        this Task<TSource> source,
        Func<TSource, Task<TBind>> bind,
        Func<TSource, TBind, TResult> project)
    {
        return project(await source, await bind(await source));
    }
}