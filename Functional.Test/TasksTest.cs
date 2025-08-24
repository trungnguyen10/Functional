using System.Diagnostics;
using Functional.Tasks;
using Xunit.Abstractions;

namespace Functional.Test;

public class TasksTest
{
    private readonly ITestOutputHelper _output;

    public TasksTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Test()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var foo =
            await Task.FromResult(Print)
                .Apply(GetFooAsync())
                .Apply(GetBarAsync());
        stopWatch.Stop();
        _output.WriteLine(stopWatch.ElapsedMilliseconds.ToString());

        stopWatch.Restart();
        var query =
            from f in GetFooAsync()
            from b in GetBarAsync()
            select Print(f, b);
        var bar = await query;
        stopWatch.Stop();
        _output.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
    }

    public async Task<string> GetFooAsync()
    {
        await Task.Delay(1000);
        return "foo";
    }

    public async Task<string> GetBarAsync()
    {
        await Task.Delay(1000);
        return "bar";
    }

    public string Print(string s, string t)
    {
        return s + t;
    }
}