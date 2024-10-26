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
    
    [Test]
    public void TestMultipleModulesWithContent()
    {
        const string input = @"
    package MyPackage {
        module First {
            let x = 5;
            let add = \a b => a + b;
        }
        module Second {
            let y = x + 10;
            let multiply = \a b => a * b;
        }
    }";

        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    [Test]
    public void TestEmptyPackageWithModules()
    {
        const string input = @"
    package EmptyPackage {
        module ModuleOne {}
        module ModuleTwo {}
    }";

        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    [Explicit("Can't call lambda fns just yet")]
    public void TestLetDeclarationWithLambda()
    {
        const string input = @"
    package MathPackage {
        module Arithmetic {
            let square = \x => x * x;
            let result = square 4;
        }
    }";

        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    [Explicit("Can't call lambda fns just yet")]
    public void TestNestedLambdas()
    {
        const string input = @"
    package FunctionPackage {
        module HigherOrder {
            let applyTwice = \f x => f(f(x));
            let increment = \x => x + 1;
            let result = applyTwice(increment)(2);
        }
    }";

        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    [Explicit("Can't call lambda fns just yet")]
    public void TestMultipleExpressionsInModule()
    {
        const string input = @"
    package DemoPackage {
        module Demo {
            let a = 10;
            let b = 20;
            let sum = \x y => x + y;
            let total = sum a b
        }
    }";

        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    [Test]
    public void TestModuleContainingModule()
    {
        const string input = @"
    package NestedPackage {
        module Outer {
            module Inner {
                let innerValue = 42;
            }
            let outerValue = 100;
        }
    }";

        Assert.IsTrue(GrammarChecker.IsValidSyntax(input), $"Failed to parse: \"{input}\"");
    }
    [Test]
    public void TestIncorrectSyntaxInModule()
    {
        const string input = @"
    package ErrorPackage {
        module Incorrect {
            let x = 5
            let y = x + 10; // Missing semicolon on the previous line
        }
    }";

        Assert.IsFalse(GrammarChecker.IsValidSyntax(input), $"Should have failed to parse: \"{input}\"");
    }
}