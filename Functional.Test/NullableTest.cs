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

    [Fact]
    public void TestReduce_HandleNullAndThrowWhenReturnNull()
    {
        string nullString = null;

        Assert.Throws<NullReferenceException>(() => nullString.Reduce(default(string)));
        Assert.Throws<InvalidOperationException>(() => nullString.Reduce(() => default));
    }

    [Fact]
    public void TestReduce_HandleNullAndReturnNonNull()
    {
        string nullString = null;
        var result = nullString.Reduce("1234");
        Assert.Equal("1234", result);

        result = nullString.Reduce(() => "1234");
        Assert.Equal("1234", result);
    }

    [Fact]
    public void TestReduce_HandleNonNullAndReturnNonNull()
    {
        string nonNullString = "qwerty";
        var result = nonNullString.Reduce("1234");
        Assert.Equal("qwerty", result);

        result = nonNullString.Reduce(() => "1234");
        Assert.Equal("qwerty", result);

        result = nonNullString.Reduce(() => default);
        Assert.Equal("qwerty", result);

        result = nonNullString.Reduce(default(string));
        Assert.Equal("qwerty", result);
    }
}