using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace THNETII.Common.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NullEqualityBinaryOperatorAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "TH0001";
        private const string Category = "Usage";

        private static readonly LocalizableString EqualityTitle = new LocalizableResourceString(nameof(Resources.NullEqualityBinaryOperationTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString InequalityTitle = new LocalizableResourceString(nameof(Resources.NullInequalityBinaryOperationTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.NullEqualityBinaryOperationMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.NullEqualityBinaryOperationDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor EqualityRule =
            new DiagnosticDescriptor(DiagnosticId, EqualityTitle, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private static readonly DiagnosticDescriptor InequalityRule =
            new DiagnosticDescriptor(DiagnosticId, InequalityTitle, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        { get; } = ImmutableArray.Create(EqualityRule, InequalityRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterOperationAction(AnalyzeBinaryOperator, OperationKind.BinaryOperator);
        }

        private static void AnalyzeBinaryOperator(OperationAnalysisContext context)
        {
            if (context.Operation is IBinaryOperation binaryOperation)
            {
                switch (binaryOperation.OperatorKind)
                {
                    case BinaryOperatorKind.Equals:
                    case BinaryOperatorKind.NotEquals:
                        var (left, right) = (binaryOperation.LeftOperand, binaryOperation.RightOperand);
                        if (IsLiteralNullOperand(left) || IsLiteralNullOperand(right))
                        {
                            Diagnostic diag = null;
                            switch (binaryOperation.OperatorKind)
                            {
                                case BinaryOperatorKind.Equals:
                                    diag = Diagnostic.Create(EqualityRule, binaryOperation.Syntax.GetLocation(), "==");
                                    break;
                                case BinaryOperatorKind.NotEquals:
                                    diag = Diagnostic.Create(InequalityRule, binaryOperation.Syntax.GetLocation(), "!=");
                                    break;
                            }
                            if (!(diag is null))
                                context.ReportDiagnostic(diag);
                        }
                        break;
                }
            }
        }

        private static bool IsLiteralNullOperand(IOperation operand)
        {
            if (!operand.ConstantValue.HasValue)
                return false;
            return operand.ConstantValue.Value is null;
        }
    }
}
