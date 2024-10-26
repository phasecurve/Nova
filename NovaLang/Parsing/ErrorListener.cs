using Antlr4.Runtime;

namespace NovaLang.Parsing;

public class ErrorListener : IAntlrErrorListener<IToken>
{
    public bool HasErrors => Errors.Count > 0;
    public List<(int Line, int CharPositionInLine, string Message)> Errors { get; } = [];

    public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
        int charPositionInLine, string msg, RecognitionException e)
    {
        Errors.Add((line, charPositionInLine, msg));
    }
}
