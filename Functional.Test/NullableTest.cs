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

    [Fact]
    public void TestBind_HandleNull()
    {
        string nullString = null;

        Func<string, string?> strictToUpper = (string s) =>
            s.All(char.IsLetter)
            ? s.ToUpper()
            : null
        ;

        var result = nullString
                        .Bind(strictToUpper);

        Assert.Null(result);
    }

    [Fact]
    public void TestBind_HandleNonNullAndReturnNull()
    {
        string nullString = "1234";

        Func<string, string?> strictToUpper = (string s) =>
            s.All(char.IsLetter)
            ? s.ToUpper()
            : null
        ;

        var result = nullString
                        .Bind(strictToUpper);

        Assert.Null(result);
    }

    [Fact]
    public void TestBind_HandleNonNullAndReturnNonNull()
    {
        string nullString = "qwerty";

        Func<string, string?> strictToUpper = (string s) =>
            s.All(char.IsLetter)
            ? s.ToUpper()
            : null
        ;

        var result = nullString
                        .Bind(strictToUpper);

        Assert.NotNull(result);
        Assert.Equal("QWERTY", result);
    }
}