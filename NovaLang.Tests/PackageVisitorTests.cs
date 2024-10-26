using FluentAssertions;

namespace NovaLang.Tests
{
    [TestFixture]
    public class PackageVisitorTests
    {
        private void ExecuteAndValidate(string program, Action<AstNode> validation)
        {
            // Print input program
            Console.WriteLine($"Running test with input program:\r\n```\r\n{program}\r\n```\r\n");

            try
            {
                AstNode packageNode = VisitorExecutorHarness.Initialise(program);
                validation(packageNode);
            }
            catch (Exception ex)
            {
                // Print the exception details
                Console.WriteLine($"Exception occurred: {ex.Message}");
                Assert.Fail($"Test failed for input: {program}. Exception: {ex.Message}");
            }
        }

        [Test]
        public void ShouldParsePackageToAst()
        {
            string program = "package Name{}";

            ExecuteAndValidate(program, packageNode => { packageNode.Name.Should().Be("Name"); });
        }

        [Test]
        public void ShouldParseModuleInPackageToAst()
        {
            string program = "package Name{module Name{}}";

            ExecuteAndValidate(program, packageNode =>
            {
                var child = packageNode.ChildNodes.Single();
                child.Name.Should().Be("Name");
            });
        }

        [Test]
        public void ShouldParseMultipleModulesInPackageToAst()
        {
            string program = "package Name{module FirstModule{} module SecondModule{}}";

            ExecuteAndValidate(program, packageNode =>
            {
                var modules = packageNode.ChildNodes.ToList();
                modules.Count.Should().Be(2);
                modules[0].Name.Should().Be("FirstModule");
                modules[1].Name.Should().Be("SecondModule");
            });
        }

        [Test]
        public void ShouldParseNestedModulesInPackageToAst()
        {
            string program = "package Name{module OuterModule{module InnerModule{}}}";

            ExecuteAndValidate(program, packageNode =>
            {
                var outerModule = packageNode.ChildNodes.Single();
                outerModule.Name.Should().Be("OuterModule");

                var innerModule = outerModule.ChildNodes.Single();
                innerModule.Name.Should().Be("InnerModule");
            });
        }

        [Test]
        public void ShouldParseLetExprToAst()
        {
            var program = "package Name{module ModuleName{ let y = 10 }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("y");
                
                var literal = letExpr.ChildNodes.Single();
                literal.Value.Should().Be("10");
            });
        }

        [Test]
        public void ShouldParseBinaryExprToAst()
        {
            string program = "package Name{module ModuleName{ let result = 5 + 3 }}";
        
            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");
        
                var binaryExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                var left = binaryExpr.Left;
                var right = binaryExpr.Right;
                left.Value.Should().Be("5");
                right.Value.Should().Be("3");
            });
        }

        [Test]
        public void ShouldParseNestedBinaryExprToAst()
        {
            string program = "package Name{module ModuleName{ let result = (5 + 3) * 2 }}";
        
            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");
        
                var binaryExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                var leftNested = (BinaryAstNode)binaryExpr.Left;
                var right = binaryExpr.Right;
                right.Value.Should().Be("2");
        
                
                var left = leftNested.Left;
                var rightInner = leftNested.Right;
                left.Value.Should().Be("5");
                rightInner.Value.Should().Be("3");
            });
        }

        [Test]
        public void ShouldParseNestedWithPrecedenceBinaryExprToAst()
        {
            const string program = "package Name{module ModuleName{ let result = 2 * (5 + 3) }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");
        
                var binaryExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                var rightNested = (BinaryAstNode)binaryExpr.Right;
                var right = binaryExpr.Left;
                right.Value.Should().Be("2");
        
                
                var leftInner = rightNested.Left;
                var rightInner = rightNested.Right;
                leftInner.Value.Should().Be("5");
                rightInner.Value.Should().Be("3");
            });
        }

        [Test]
        public void ShouldParseWithPrecedenceBinaryExprToAst()
        {
            const string program = "package Name{module ModuleName{ let result = 2 * 5 + 3 }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");
        
                var binaryExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                var leftNested = (BinaryAstNode)binaryExpr.Left;
                var right = binaryExpr.Right;
                right.Value.Should().Be("3");
        
                
                var leftInner = leftNested.Left;
                var rightInner = leftNested.Right;
                leftInner.Value.Should().Be("2");
                rightInner.Value.Should().Be("5");
            });
        }

        [Test]
        public void ShouldParseLiteralExprToAst()
        {
            string program = "package Name{module ModuleName{ let result = 42 }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");

                var literal = letExpr.ChildNodes.Single();
                literal.Value.Should().Be("42");
            });
        }
        
        [Test]
        public void ShouldParseComplexExprWithPrecedenceToAst()
        {
            const string program = "package Name{module ModuleName{ let result = 2 * (5 + 3) - 4 / 2 }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");

                // Start with the root of the AST: the subtraction node
                var subtractionExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                subtractionExpr.Operator.Should().Be("-");

                // Check the right side of the subtraction: should be division (4 / 2)
                var divisionExpr = (BinaryAstNode)subtractionExpr.Right;
                divisionExpr.Operator.Should().Be("/");
                divisionExpr.Left.Value.Should().Be("4");
                divisionExpr.Right.Value.Should().Be("2");

                // Check the left side of the subtraction: should be multiplication (2 * (5 + 3))
                var multiplicationExpr = (BinaryAstNode)subtractionExpr.Left;
                multiplicationExpr.Operator.Should().Be("*");

                // Left side of the multiplication is the literal 2
                multiplicationExpr.Left.Value.Should().Be("2");

                // Right side of the multiplication is the addition (5 + 3)
                var additionExpr = (BinaryAstNode)multiplicationExpr.Right;
                additionExpr.Operator.Should().Be("+");
                additionExpr.Left.Value.Should().Be("5");
                additionExpr.Right.Value.Should().Be("3");
            });
        }
        
        [Test]
        public void ShouldParseExprWithMixedOperatorsToAst()
        {
            const string program = "package Name{module ModuleName{ let result = 3 + 4 * 2 - 5 }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");

                // Root is a subtraction node representing (3 + (4 * 2)) - 5
                var subtractionExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                subtractionExpr.Operator.Should().Be("-");

                // Right child of subtraction is the literal 5
                subtractionExpr.Right.Value.Should().Be("5");

                // Left child of subtraction is the addition (3 + (4 * 2))
                var additionExpr = (BinaryAstNode)subtractionExpr.Left;
                additionExpr.Operator.Should().Be("+");

                // Left of addition is the literal 3
                additionExpr.Left.Value.Should().Be("3");

                // Right of addition is the multiplication (4 * 2)
                var multiplicationExpr = (BinaryAstNode)additionExpr.Right;
                multiplicationExpr.Operator.Should().Be("*");
                multiplicationExpr.Left.Value.Should().Be("4");
                multiplicationExpr.Right.Value.Should().Be("2");
            });
        }
        [Test]
        public void ShouldParseExprWithParenthesesToAst()
        {
            const string program = "package Name{module ModuleName{ let result = (3 + 4) * (2 - 5) }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");

                // Root is a multiplication node
                var multiplicationExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                multiplicationExpr.Operator.Should().Be("*");

                // Left side is the addition (3 + 4)
                var additionExpr = (BinaryAstNode)multiplicationExpr.Left;
                additionExpr.Operator.Should().Be("+");
                additionExpr.Left.Value.Should().Be("3");
                additionExpr.Right.Value.Should().Be("4");

                // Right side is the subtraction (2 - 5)
                var subtractionExpr = (BinaryAstNode)multiplicationExpr.Right;
                subtractionExpr.Operator.Should().Be("-");
                subtractionExpr.Left.Value.Should().Be("2");
                subtractionExpr.Right.Value.Should().Be("5");
            });
        }
        [Test]
        public void ShouldParseExprWithDivisionAndParenthesesToAst()
        {
            const string program = "package Name{module ModuleName{ let result = 7 / (3 + 4) * 2 }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");

                // Root is a multiplication node representing (7 / (3 + 4)) * 2
                var multiplicationExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                multiplicationExpr.Operator.Should().Be("*");

                // Right child of multiplication is the literal 2
                multiplicationExpr.Right.Value.Should().Be("2");

                // Left child of multiplication is the division (7 / (3 + 4))
                var divisionExpr = (BinaryAstNode)multiplicationExpr.Left;
                divisionExpr.Operator.Should().Be("/");

                // Left side of division is the literal 7
                divisionExpr.Left.Value.Should().Be("7");

                // Right side of division is the addition (3 + 4)
                var additionExpr = (BinaryAstNode)divisionExpr.Right;
                additionExpr.Operator.Should().Be("+");
                additionExpr.Left.Value.Should().Be("3");
                additionExpr.Right.Value.Should().Be("4");
            });
        }
        [Test]
        public void ShouldParseComplexExprWithParenthesesAndDivisionToAst()
        {
            const string program = "package Name{module ModuleName{ let result = (8 - 2) * (3 + 1) / 2 }}";

            ExecuteAndValidate(program, packageNode =>
            {
                var module = packageNode.ChildNodes.Single();
                var letExpr = module.ChildNodes.Single();
                letExpr.Name.Should().Be("result");

                // Root node is division representing ((8 - 2) * (3 + 1)) / 2
                var divisionExpr = (BinaryAstNode)letExpr.ChildNodes.Single();
                divisionExpr.Operator.Should().Be("/");

                // Right side of division is the literal 2
                divisionExpr.Right.Value.Should().Be("2");

                // Left side of division is multiplication (8 - 2) * (3 + 1)
                var multiplicationExpr = (BinaryAstNode)divisionExpr.Left;
                multiplicationExpr.Operator.Should().Be("*");

                // Left side of multiplication is the subtraction (8 - 2)
                var subtractionExpr = (BinaryAstNode)multiplicationExpr.Left;
                subtractionExpr.Operator.Should().Be("-");
                subtractionExpr.Left.Value.Should().Be("8");
                subtractionExpr.Right.Value.Should().Be("2");

                // Right side of multiplication is the addition (3 + 1)
                var additionExpr = (BinaryAstNode)multiplicationExpr.Right;
                additionExpr.Operator.Should().Be("+");
                additionExpr.Left.Value.Should().Be("3");
                additionExpr.Right.Value.Should().Be("1");
            });
        }

    }
}