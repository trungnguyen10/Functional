using Functional;
namespace Functional.Test;

public class NullableTest
{
    [Fact]
    public void TestMap_HandleNull()
    {
        string nullString = null;

        var count = nullString
                        .Map(s => s.ToUpper())
                        .Map(s => s.Length);

        Assert.Equal(0, count);
    }

    [Fact]
    public void TestMap_HandleNoneNull()
    {
        string nullString = "12345";

        var count = nullString
                        .Map(s => s.ToUpper())
                        .Map(s => s.Length);

        Assert.Equal(5, count);
    }
}