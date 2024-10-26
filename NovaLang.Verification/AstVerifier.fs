namespace SimpleLang.Verification

open FsCheck
open NUnit.Framework
open FsCheck.NUnit
open NovaLang


[<TestFixture>]
type AstVerifier() =
    
    let validIdentifierGen: Gen<string> =
        Gen.choose(1, 10)
        |> Gen.map (fun length ->
            let chars =
                [|
                    let selection = System.Random().Next(0, 3)
                    yield
                      if selection = 0 then char (65 + System.Random().Next(0, 26))
                      elif selection = 1 then char (97 + System.Random().Next(0, 26))
                      else char 95
                    for i in 1 .. length - 1 do
                        yield
                            match System.Random().Next(0, 4) with
                            | 0 -> char (65 + System.Random().Next(0, 26)) // A-Z
                            | 1 -> char (97 + System.Random().Next(0, 26)) // a-z
                            | 2 -> char (48 + System.Random().Next(0, 10)) // 0-9
                            | _ -> '_'
                |]
            System.String.Concat(chars))
        
    [<Property>]
    let ``BinaryNode has two children`` (op: string, leftVal: string, rightVal: string) =
        let leftNode = LiteralAstNode(leftVal) :> AstNode
        let rightNode = LiteralAstNode(rightVal) :> AstNode
        let binaryNode = BinaryAstNode(op, leftNode, rightNode)

        binaryNode.Left = leftNode && binaryNode.Right = rightNode

    [<Property>]
    let ``LetNode attaches RHS as child`` (varName: string, expressionValue: string) =
        let letNode = LetAstNode(varName) :> ExprAstNode
        let exprNode = LiteralAstNode(expressionValue) :> AstNode

        letNode.AddChild(exprNode)

        letNode.ChildNodes |> List.ofSeq |> List.contains exprNode

    [<Property>]
    let ``Round-trip parsing maintains expression precedence`` () =
        let leftNode = LiteralAstNode("5") :> AstNode
        let rightNode = BinaryAstNode("*", LiteralAstNode("3") :> AstNode, LiteralAstNode("2") :> AstNode)
        let binaryNode = BinaryAstNode("+", leftNode, rightNode)

        let actual = binaryNode.ToString()
        actual = "5 + 3 * 2"
        
    [<Property>]
    let ``Round-trip parsing maintains expression precedence with explicit precedence`` () =
        let leftNode = LiteralAstNode("5") :> AstNode
        let additionNode = BinaryAstNode("+", LiteralAstNode("1") :> AstNode, LiteralAstNode("4") :> AstNode)
        let firstMultiplication = BinaryAstNode("*", additionNode, LiteralAstNode("3") :> AstNode)
        let rightNode = BinaryAstNode("*", firstMultiplication, LiteralAstNode("2") :> AstNode)

        let binaryNode = BinaryAstNode("-", leftNode, rightNode)

        let actual = binaryNode.ToString()
        let expected = "5 - ((1 + 4) * 3) * 2"

        printfn $"Actual: %s{actual}"
        printfn $"Expected: %s{expected}"

        actual = expected


    [<Property>]
    let ``Multiplication has higher precedence than addition`` () =
        let exprNode = BinaryAstNode("+", LiteralAstNode("5") :> AstNode, BinaryAstNode("*", LiteralAstNode("3") :> AstNode, LiteralAstNode("2") :> AstNode))
        exprNode.ToString() = "5 + 3 * 2"

    [<Property>]
    let ``LiteralAstNode sets value correctly`` () =
        let node = LiteralAstNode("42")
        Assert.AreEqual("42", node.Value)
        
    [<Property>]
    let ``Let binding parsing produces correct structure`` (name: string, value: int) =
        let letNode = LetAstNode(name)
        letNode.AddChild(BinaryAstNode("+", LiteralAstNode("5") :> AstNode, LiteralAstNode("3") :> AstNode))

        let expectedString = $"let {name} = 5 + 3"
        let letNodeString = $"let {name} = {letNode.ChildNodes.[0].ToString()}"
        letNodeString = expectedString

    
    
    [<Property>]
    let ``Nested let binding structure is correct`` () =
        let nameX = Gen.sample 0 1 validIdentifierGen |> List.head
        let nameY = Gen.sample 0 1 validIdentifierGen |> List.head

        let innerLetNode = LetAstNode(nameY)
        let innerExpr = BinaryAstNode("+", LiteralAstNode("5") :> AstNode, LiteralAstNode("3") :> AstNode)
        innerLetNode.AddChild(innerExpr)

        let outerLetNode = LetAstNode(nameX)
        
        let outerExpr = BinaryAstNode("*", 
                                       LiteralAstNode(nameY) :> AstNode, 
                                       LiteralAstNode("2") :> AstNode)
        outerLetNode.AddChild(outerExpr)

        let moduleNode = ModuleAstNode("MyModule")
        moduleNode.AddChild(innerLetNode)
        moduleNode.AddChild(outerLetNode)

        let expectedStructure = 
            $"module MyModule {{\n" +
            $"let {nameY} = 5 + 3\n" +
            $"let {nameX} = {nameY} * 2\n" +
            $"}}"

        let moduleString = moduleNode.ToString()

        Assert.AreEqual(expectedStructure, moduleString)

        
    [<Property>]
    let ``Parentheses group correctly`` () =
        let exprNode = BinaryAstNode("*", BinaryAstNode("+", LiteralAstNode("5") :> AstNode, LiteralAstNode("3") :> AstNode), LiteralAstNode("2") :> AstNode)
        exprNode.ToString() = "(5 + 3) * 2"

    [<Property>]
    let ``Nested let binding with blocks is correct`` () =
        let nameX = Gen.sample 0 1 validIdentifierGen |> List.head
        let nameY = Gen.sample 0 1 validIdentifierGen |> List.head
        let moduleName = Gen.sample 0 1 validIdentifierGen |> List.head

        let innerLetNode = LetAstNode(nameY)
        let innerExpr = BinaryAstNode("+", LiteralAstNode("5") :> AstNode, LiteralAstNode("3") :> AstNode)
        innerLetNode.AddChild(innerExpr)

        let outerLetNode = LetAstNode(nameX)
        outerLetNode.AddChild(innerLetNode)
        outerLetNode.AddChild(LiteralAstNode("2") :> AstNode)

        let moduleNode = ModuleAstNode(moduleName)
        moduleNode.AddChild(innerLetNode)
        moduleNode.AddChild(outerLetNode)

        let expectedStructure = 
            $"module {moduleName} {{\nlet {nameY} = 5 + 3\nlet {nameX} = {{\n{innerLetNode.ToString()}\n2\n}}\n}}"

        let moduleString = moduleNode.ToString()

        Assert.AreEqual(expectedStructure, moduleString)
