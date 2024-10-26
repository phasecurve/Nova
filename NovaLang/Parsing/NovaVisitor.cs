//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Nova.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="NovaParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface INovaVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpr([NotNull] NovaParser.ExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.container"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitContainer([NotNull] NovaParser.ContainerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.package"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPackage([NotNull] NovaParser.PackageContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.module"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitModule([NotNull] NovaParser.ModuleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.let"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLet([NotNull] NovaParser.LetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.lambda"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambda([NotNull] NovaParser.LambdaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.paramList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParamList([NotNull] NovaParser.ParamListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] NovaParser.LiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.binary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinary([NotNull] NovaParser.BinaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.addition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddition([NotNull] NovaParser.AdditionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.multiplication"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplication([NotNull] NovaParser.MultiplicationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAtom([NotNull] NovaParser.AtomContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="NovaParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifier([NotNull] NovaParser.IdentifierContext context);
}
