using Antlr4.Runtime;
using System;

namespace NovaLang.Tests
{
    public class VisitorExecutorHarness
    {
        public static AstNode Initialise(string program)
        {
            ICharStream input = new AntlrInputStream(program);
            var lexer = new NovaLexer(input);
            var parser = new NovaParser(new CommonTokenStream(lexer));

            try
            {
                var package = parser.package();
                var v = new SemanticAnalysisVisitor();
                var ast = (AstNode)v.Visit(package);
                return ast;
            }
            catch (RecognitionException ex)
            {
                Console.WriteLine($"Lexing/Parsing issue: {ex.Message}. Input: {program}");
                throw new Exception($"Lexing/Parsing issue: {ex.Message}. Input: {program}", ex);
            }
        }
    }
}