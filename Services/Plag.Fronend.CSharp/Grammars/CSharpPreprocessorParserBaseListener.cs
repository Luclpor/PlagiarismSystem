//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\namofun\plagiarism\src\Plag.Frontend.Csharp\CSharpPreprocessorParser.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Antlr4.Grammar.Csharp {
 using System.Linq; 

using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ICSharpPreprocessorParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
//[System.CLSCompliant(false)]
public partial class CSharpPreprocessorParserBaseListener : ICSharpPreprocessorParserListener {
	/// <summary>
	/// Enter a parse tree produced by the <c>PreprocessorDeclaration</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorDeclaration([NotNull] CSharpPreprocessorParser.PreprocessorDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PreprocessorDeclaration</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorDeclaration([NotNull] CSharpPreprocessorParser.PreprocessorDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>PreprocessorConditional</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorConditional([NotNull] CSharpPreprocessorParser.PreprocessorConditionalContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PreprocessorConditional</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorConditional([NotNull] CSharpPreprocessorParser.PreprocessorConditionalContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>PreprocessorLine</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorLine([NotNull] CSharpPreprocessorParser.PreprocessorLineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PreprocessorLine</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorLine([NotNull] CSharpPreprocessorParser.PreprocessorLineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>PreprocessorDiagnostic</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorDiagnostic([NotNull] CSharpPreprocessorParser.PreprocessorDiagnosticContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PreprocessorDiagnostic</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorDiagnostic([NotNull] CSharpPreprocessorParser.PreprocessorDiagnosticContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>PreprocessorRegion</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorRegion([NotNull] CSharpPreprocessorParser.PreprocessorRegionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PreprocessorRegion</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorRegion([NotNull] CSharpPreprocessorParser.PreprocessorRegionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>PreprocessorPragma</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorPragma([NotNull] CSharpPreprocessorParser.PreprocessorPragmaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PreprocessorPragma</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorPragma([NotNull] CSharpPreprocessorParser.PreprocessorPragmaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>PreprocessorNullable</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorNullable([NotNull] CSharpPreprocessorParser.PreprocessorNullableContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PreprocessorNullable</c>
	/// labeled alternative in <see cref="CSharpPreprocessorParser.preprocessorDirective"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorNullable([NotNull] CSharpPreprocessorParser.PreprocessorNullableContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CSharpPreprocessorParser.DirectiveNewLineOrSharp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirectiveNewLineOrSharp([NotNull] CSharpPreprocessorParser.DirectiveNewLineOrSharpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CSharpPreprocessorParser.DirectiveNewLineOrSharp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirectiveNewLineOrSharp([NotNull] CSharpPreprocessorParser.DirectiveNewLineOrSharpContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CSharpPreprocessorParser.PreprocessorExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorExpression([NotNull] CSharpPreprocessorParser.PreprocessorExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CSharpPreprocessorParser.PreprocessorExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorExpression([NotNull] CSharpPreprocessorParser.PreprocessorExpressionContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace Antlr4.Grammar.Csharp
