using Antlr4.Runtime;
using NovaLang.Parsing;

namespace NovaLang.Tests;

public class GrammarChecker
{
    public static bool IsValidSyntax(string input)
    {
        var inputStream = new AntlrInputStream(input);
        var lexer = new NovaLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new NovaParser(tokenStream);

        var errorListener = new ErrorListener();
        parser.RemoveErrorListeners();
        parser.AddErrorListener(errorListener);

        parser.package();

        var isValidSyntax = !errorListener.HasErrors;

        if (!isValidSyntax)
        {
            foreach (var error in errorListener.Errors)
            {
                Console.WriteLine($"Line {error.Line}:{error.CharPositionInLine} - {error.Message}");
            }
        }
        return isValidSyntax;
    }

}