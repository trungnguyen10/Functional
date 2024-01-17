using Functional.Option;

namespace Functional.Test;

public class OptionTest
{
    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    [InlineData("foo")]
    public void TestEquality(object? o)
    {
        var foo = OptionOf(o);
        var bar = OptionOf(o);
        Assert.Equal(foo, bar);
    }

    [Theory]
    [InlineData("")]
    [InlineData("foo")]
    [InlineData("bar")]
    public void TestFunctorLaw_1(string s)
    {
        var id = (string s) => s;
        Option<string> option = OptionOf(s);
        Assert.Equal(option, option.Select(id));
    }

    [Theory]
    [InlineData("")]
    [InlineData("foo")]
    [InlineData("bar")]
    public void TestFunctorLaw_2(string s)
    {
        var g = (string s) => s.Length;
        var f = (int x) => x % 2 == 0;
        var option = OptionOf(s);
        Assert.Equal(option.Select(g).Select(f), option.Select(s => f(g(s))));
    }

    [Theory]
    [InlineData("")]
    [InlineData("foo")]
    [InlineData("bar")]
    [InlineData("1234")]
    public void Test_LeftIdentity(string s)
    {
        Func<string, Option<string>> f = (string s) => s.All(char.IsLetter)
                                                        ? SomeOf(s.ToUpper())
                                                        : NoneOf<string>();

        Assert.Equal(OptionOf(s).SelectMany(f), f(s));
    }

    [Theory]
    [InlineData("")]
    [InlineData("foo")]
    [InlineData("bar")]
    [InlineData("1234")]
    public void Test_RightIdentity(string s)
    {
        Func<string, Option<string>> f = (string s) => s.All(char.IsLetter)
                                                        ? SomeOf(s.ToUpper())
                                                        : NoneOf<string>();

        Assert.Equal(f(s).SelectMany(s => OptionOf(s)), f(s));
    }

    [Theory]
    [InlineData("bar")]
    [InlineData("-1")]
    [InlineData("0")]
    [InlineData("4")]
    public void Test_Associativity(string s)
    {
        Option<int> f(string s)
        {
            if (int.TryParse(s, out var i))
                return SomeOf(i);
            else
                return NoneOf<int>();
        }

        Func<int, Option<double>> g = i => i >= 0
                                            ? SomeOf(Math.Sqrt(i))
                                            : NoneOf<double>();

        Func<double, Option<double>> h = d => d == 0
                                                ? NoneOf<double>()
                                                : SomeOf(1 / d);

        Assert.Equal(f(s).SelectMany(g).SelectMany(h), f(s).SelectMany(x => g(x).SelectMany(h)));
    }

    [Fact]
    public void TestSelectManyWithIEnumerableAndOption()
    {
        var ages = new List<int?> { 10, null, 20 };
        var avg = ages.SelectMany(a => OptionOf(a)).Average();
        Assert.Equal((10 + 20) / 2, avg);
    }
}
