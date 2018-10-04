using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NullEqualityBinaryOperatorCodeFixProvider)), Shared]
    public class NullEqualityBinaryOperatorCodeFixProvider : CodeFixProvider
    {
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.NullReferenceCheckCodeFixTitle), Resources.ResourceManager, typeof(Resources));

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(NullEqualityBinaryOperatorAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.FirstOrDefault();
            if (diagnostic is null)
                return;
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            if (diagnosticSpan.IsEmpty)
                return;

            var diagnosticSpanStartToken = root.FindToken(diagnosticSpan.Start);
            var binaryExpression = diagnosticSpanStartToken.Parent.FirstAncestorOrSelf<BinaryExpressionSyntax>();

            if (binaryExpression.IsKind(SyntaxKind.EqualsExpression))
            {
                context.RegisterCodeFix(CodeAction.Create(Title.ToString(CultureInfo.CurrentUICulture),
                    createChangedDocument: cancelToken => MakeIsNullCheck(context.Document, root, binaryExpression, cancelToken)),
                    diagnostic);
            }
            else if (binaryExpression.IsKind(SyntaxKind.NotEqualsExpression))
            {
                context.RegisterCodeFix(CodeAction.Create(Title.ToString(CultureInfo.CurrentUICulture),
                    createChangedDocument: cancelToken => MakeIsNotNullCheck(context.Document, root, binaryExpression, cancelToken)),
                    diagnostic);
            }
        }

        private static IsPatternExpressionSyntax MakeIsPattern(BinaryExpressionSyntax binaryExpression)
        {
            var (left, right) = (binaryExpression.Left, binaryExpression.Right);
            if (left.IsKind(SyntaxKind.NullLiteralExpression))
            {
                var t = left;
                left = right;
                right = t;
            }
            return SyntaxFactory.IsPatternExpression(left, SyntaxFactory.ConstantPattern(right));
        }

        private static async Task<Document> MakeIsNotNullCheck(Document document, SyntaxNode root, BinaryExpressionSyntax binaryExpression, CancellationToken cancelToken)
        {
            var isPattern = MakeIsPattern(binaryExpression);
            var parens = SyntaxFactory.ParenthesizedExpression(isPattern);
            var notExpr = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, parens);

            var oldRoot = root ?? await document.GetSyntaxRootAsync(cancelToken).ConfigureAwait(false);
            var newRoot = oldRoot.ReplaceNode(binaryExpression, notExpr);
            return document.WithSyntaxRoot(newRoot);
        }

        private static async Task<Document> MakeIsNullCheck(Document document, SyntaxNode root, BinaryExpressionSyntax binaryExpression, CancellationToken cancelToken)
        {
            var isPattern = MakeIsPattern(binaryExpression);

            var oldRoot = root ?? await document.GetSyntaxRootAsync(cancelToken).ConfigureAwait(false);
            var newRoot = oldRoot.ReplaceNode(binaryExpression, isPattern);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
