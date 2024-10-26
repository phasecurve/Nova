namespace NovaLang;

public class PackageAstNode(string? name) : AstNode(name);

public class ModuleAstNode(string? name) : AstNode(name)
{
    public override string ToString()
    {
        if (ChildNodes.Count == 0)
        {
            return $"module {Name} {{ }}";
        }

        var expressions = string.Join("\n", ChildNodes);
        return $"module {Name} {{\n{expressions}\n}}";
    }
}

public class ExprAstNode(string? name, string? value = null) : AstNode(name, value);

public class LetAstNode(string? name) : ExprAstNode(name)
{
    public override string ToString()
    {
        if (ChildNodes.Count > 1)
        {
            var expressions = string.Join("\n", ChildNodes);
            return $"let {Name} = {{\n{expressions}\n}}";
        }
        var rightHandSide = ChildNodes.Count > 0 ? string.Join(" ", ChildNodes) : string.Empty;
        return $"let {Name} = {rightHandSide}";
    }
}

public class LiteralAstNode(string value) : AstNode(null, value)
{
    public override string ToString() => value;
}

public class BinaryAstNode(string @operator, AstNode left, AstNode right) : AstNode(null)
{
    public string Operator { get; set; } = @operator;
    public AstNode Left { get; set; } = left;
    public AstNode Right { get; set; } = right;

    public BinaryAstNode() : this(null!, null!, null!) {}

    private int GetPrecedence(string op) =>
        op switch
        {
            "+" or "-" => 1,
            "*" or "/" => 2,
            _ => 0
        };

    public override string ToString()
    {
        var leftStr = Left is BinaryAstNode leftBinary
                      && GetPrecedence(leftBinary.Operator) <= GetPrecedence(Operator)
            ? $"({Left})" : Left.ToString();

        var rightStr = Right is BinaryAstNode rightBinary
                       && (GetPrecedence(rightBinary.Operator) < GetPrecedence(Operator)
                           || (GetPrecedence(rightBinary.Operator) == GetPrecedence(Operator)
                               && IsLeftAssociative(Operator)))
            ? $"({Right})" : Right.ToString();

        return $"{leftStr} {Operator} {rightStr}";
    }
    private bool IsLeftAssociative(string op) => op is "+" or "-" or "*" or "/";
}


public abstract class AstNode(string? name, object? value = null)
{
    public string? Name => name;

    public List<AstNode> ChildNodes { get; } = [];
    public object? Value { get; private set; } = value;

    public void AddChild(AstNode node)
    {
        ChildNodes.Add(node);
    }

    public void AttachValue(AstNode value)
    {
        Value = value;
    }
    
    public override string ToString()
    {
        if (ChildNodes.Count == 0)
        {
            return Value?.ToString() ?? string.Empty;
        }

        return string.Join(" ", ChildNodes);
    }
}