namespace NovaLang.Tests;

public class SyntaxTests
{
    [Test]
    public void TestEmptyPackage()
    {
        const string input = @"package Name{}";
        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    
    [Test]
    public void TestEmptyModule()
    {
        const string input = @"package Name{

    module Empty {}
}
";
        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    
    [Test]
    public void TestMultipleModules()
    {
        const string input = @"package Name{
    module First {}
    module Second {}
}
";
        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
}