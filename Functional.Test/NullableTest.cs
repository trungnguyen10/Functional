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

        var result = nullString
                        .Bind(StrictToUpper);

        Assert.Null(result);
    }

    [Fact]
    public void TestBind_HandleNonNullAndReturnNull()
    {
        string nonNullString = "1234";

        var result = nonNullString
                        .Bind(StrictToUpper);

        Assert.Null(result);
    }

    [Fact]
    public void TestBind_HandleNonNullAndReturnNonNull()
    {
        string nonNullString = "qwerty";



        var result = nonNullString
                        .Bind(StrictToUpper);

        Assert.NotNull(result);
        Assert.Equal("QWERTY", result);
    }

    static string? StrictToUpper(string s) =>
        s.All(char.IsLetter)
        ? s.ToUpper()
        : null;
}