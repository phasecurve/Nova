using Antlr4.Runtime.Tree;

namespace NovaLang;

public class SemanticAnalysisVisitor : NovaBaseVisitor<object>
{
    public override object Visit(IParseTree tree)
    {
        if (tree is NovaParser.PackageContext packageContext)
        {
            return VisitPackage(packageContext);
        }
        
        if (tree is NovaParser.ModuleContext module)
        {
            return VisitModule(module);
        }

        if (tree is NovaParser.BinaryContext binary)
        {
            return VisitBinary(binary);
        }

        if (tree is NovaParser.AtomContext atom)
        {
            return VisitAtom(atom);
        }

        if (tree is NovaParser.LiteralContext literal)
        {
            return VisitLiteral(literal);
        }
        return base.Visit(tree);
    }

    public override object VisitPackage(NovaParser.PackageContext context)
    {
        var packageName = context.ID().ToString();
        var packageAstNode = new PackageAstNode(packageName);

        foreach (var moduleContext in context.module())
        {
            packageAstNode.AddChild((AstNode)Visit(moduleContext));
        }

        return packageAstNode;
    }

    public override object VisitModule(NovaParser.ModuleContext context)
    {
        var moduleName = context.ID().ToString();
        var moduleAstNode = new ModuleAstNode(moduleName);

        foreach (var exprContext in context.container())
        {
            moduleAstNode.AddChild((AstNode)Visit(exprContext));
        }
        
        return moduleAstNode;
    }

    public override object VisitExpr(NovaParser.ExprContext context)
    {
        if (context.let() != null) 
        {
            return Visit(context.let());
        }

        throw new NotSupportedException("Unsupported expression type.");
    }

    public override object VisitLet(NovaParser.LetContext context)
    {
        var letName = context.ID().ToString();
        var letAstNode = new LetAstNode(letName);

        var parseTree = context.GetChild(3);
        var visit = Visit(parseTree);
        var exprAstNode = (AstNode)visit;
        letAstNode.AddChild(exprAstNode);

        return letAstNode;
    }

    public override object VisitIdentifier(NovaParser.IdentifierContext context)
    {
        return new IdentifierAstNode(context.GetText());
    }

    public override object VisitLiteral(NovaParser.LiteralContext context)
    {
        var literalValue = int.Parse(context.I16().GetText());
        return new LiteralAstNode(literalValue.ToString());
    }

    public override object VisitBinary(NovaParser.BinaryContext context)
    {
        return Visit(context.addition());
    }

    public override object VisitAddition(NovaParser.AdditionContext context)
    {
        var left = Visit(context.multiplication(0)); // Visit the first operand (multiplication node)
    
        for (int i = 1; i < context.multiplication().Length; i++)
        {
            var right = Visit(context.multiplication(i));
            var operatorNode = context.GetChild(i * 2 - 1).GetText(); // Get "+" or "-" operator
        
            left = new BinaryAstNode(operatorNode, left as AstNode, right as AstNode);
        }
    
        return left;
    }

    public override object VisitMultiplication(NovaParser.MultiplicationContext context)
    {
        var left = Visit(context.atom(0)); // Visit the first operand (atom node)
    
        for (int i = 1; i < context.atom().Length; i++)
        {
            var right = Visit(context.atom(i));
            var operatorNode = context.GetChild(i * 2 - 1).GetText(); // Get "*" or "/" operator
        
            left = new BinaryAstNode(operatorNode, left as AstNode, right as AstNode);
        }
    
        return left;
    }

    public override object VisitAtom(NovaParser.AtomContext context)
    {
        if (context.I16() != null)
        {
            return new LiteralAstNode(context.I16().GetText());
        }
        else if (context.LPAR() != null)
        {
            return Visit(context.binary()); // Visit the nested expression inside parentheses
        }

        throw new InvalidOperationException("Unexpected atom type.");
    }


    // public override object VisitAtom(SimpleParser.AtomContext context)
    // {
    //     if (context.binary() != null)
    //     {
    //         return Visit(context.binary());
    //     }
    //     var atom = int.Parse(context.I16().GetText());
    //     return new LiteralAstNode(atom.ToString());
    // }
    
    // public override object VisitBinary(SimpleParser.BinaryContext context)
    // {
    //     if (context.atom() != null)
    //     {
    //         return VisitAtom(context.atom());
    //     }
    //     if (context.addition() != null)
    //     {
    //         return VisitAddition(context.addition());
    //     }
    //     if (context.subtraction() != null)
    //     {
    //         return VisitSubtraction(context.subtraction());
    //     }
    //     if (context.multiplication() != null)
    //     {
    //         return VisitMultiplication(context.multiplication());
    //     }
    //     if (context.division() != null)
    //     {
    //         return VisitDivision(context.division());
    //     }
    //
    //     throw new NotSupportedException("Unsupported binary operation.");
    // }
    //
    // public override object VisitAddition(SimpleParser.AdditionContext context)
    // {
    //     if (context.ChildCount == 3)
    //     {
    //         var leftNode = (AstNode)Visit(context.GetChild(0));
    //         var operatorNode = context.GetChild(1).GetText();
    //         var rightNode = (AstNode)Visit(context.GetChild(2));
    //     
    //         return new BinaryAstNode(operatorNode, leftNode, rightNode);
    //     }
    //
    //     return Visit(context.GetChild(0)); 
    // }
    //
    // public override object VisitMultiplication(SimpleParser.MultiplicationContext context)
    // {
    //     if (context.ChildCount == 3)
    //     {
    //         var leftNode = (AstNode)Visit(context.GetChild(0));
    //         var operatorNode = context.GetChild(1).GetText();
    //         var rightNode = (AstNode)Visit(context.GetChild(2));
    //
    //         var binaryAstNode = new BinaryAstNode(operatorNode, leftNode, rightNode);
    //         return binaryAstNode;
    //     }
    //
    //     return Visit(context.GetChild(0)); 
    // }
}

