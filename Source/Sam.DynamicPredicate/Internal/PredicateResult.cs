using System.Collections.Generic;

namespace Sam.DynamicPredicate.Internal
{
    internal class PredicateResult
    {
        public PredicateResult(string logicalExpression, Dictionary<string, string> expressions)
        {
            LogicalExpression = logicalExpression;
            Expressions = expressions;
        }

        public string LogicalExpression { get; }
        public Dictionary<string, string> Expressions { get; }
    }
}