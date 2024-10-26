namespace SimpleLang.Verification

open NUnit.Framework
open FsCheck.NUnit
open NovaLang

[<TestFixture>]
type GrammarProperties() =
    let eval = AstEvaluator.Evaluate

    [<Property>]
    let ``Addition is associative`` (a: int, b: int, c: int) =
        let leftAssociative = BinaryAstNode("+", BinaryAstNode("+", LiteralAstNode(a.ToString()) :> AstNode, LiteralAstNode(b.ToString()) :> AstNode), LiteralAstNode(c.ToString()) :> AstNode)
        let rightAssociative = BinaryAstNode("+", LiteralAstNode(a.ToString()) :> AstNode, BinaryAstNode("+", LiteralAstNode(b.ToString()) :> AstNode, LiteralAstNode(c.ToString()) :> AstNode))
        
        let left = eval leftAssociative
        let right = eval rightAssociative
        
        printfn $"Left: {left}"
        printfn $"Right: {right}"
        left = right

    [<Property>]
    let ``Multiplication is associative`` (a: int, b: int, c: int) =
        let leftAssociative =
            BinaryAstNode("*",
                          BinaryAstNode("*",
                                        LiteralAstNode(a.ToString()) :> AstNode,
                                        LiteralAstNode(b.ToString()) :> AstNode),
                          LiteralAstNode(c.ToString()) :> AstNode)
        let rightAssociative =
            BinaryAstNode("*",
                          LiteralAstNode(a.ToString()) :> AstNode,
                          BinaryAstNode("*",
                                        LiteralAstNode(b.ToString()) :> AstNode,
                                        LiteralAstNode(c.ToString()) :> AstNode))

        let leftResult = eval leftAssociative
        let rightResult = eval rightAssociative

        printfn $"Left Result: {leftResult}"
        printfn $"Right Result: {rightResult}"
        leftResult = rightResult



    [<Test>]
    member _.testToString () =
        let node = BinaryAstNode("*", LiteralAstNode("0") :> AstNode, LiteralAstNode("1") :> AstNode)
        let result = node.ToString()
        printfn $"Result: %s{result}"
        let result = result = "0 * 1"
        Assert.IsTrue(result)