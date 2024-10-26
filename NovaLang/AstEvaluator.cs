namespace NovaLang;

public static class AstEvaluator
{
    public static int Evaluate(AstNode node)
    {
        switch (node)
        {
            case LiteralAstNode literal:
                return int.Parse((literal.Value as string)!);
            case BinaryAstNode binary:
            {
                var leftValue = Evaluate(binary.Left);
                var rightValue = Evaluate(binary.Right);
                return binary.Operator switch
                {
                    "*" => leftValue * rightValue,
                    "+" => leftValue + rightValue,
                    "-" => leftValue - rightValue,
                    "/" => leftValue / rightValue,
                    _ => throw new NotSupportedException($"Unsupported operator: {binary.Operator}")
                };
            }
            default:
                throw new NotSupportedException("Unsupported node type.");
        }
    }
}