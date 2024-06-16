using Sam.DynamicPredicate.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sam.DynamicPredicate
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Compile<T>(string predicateString)
        {
            Dictionary<string, Expression<Func<T, bool>>> expressions = new Dictionary<string, Expression<Func<T, bool>>>();

            var result = Simplification(predicateString);

            foreach (var exprDict in result.Expressions)
            {
                expressions.Add(exprDict.Key, GenerateExpression<T>(exprDict.Value));
            }

            string logicalExpression = result.LogicalExpression;

            var parameter = Expression.Parameter(typeof(T), "x");
            var body = ParseExpression(logicalExpression, expressions, parameter);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression<Func<T, bool>> GenerateExpression<T>(string predicateString)
        {
            var parts = predicateString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var propertyName = parts[0];
            var @operator = parts[1];
            var valueString = parts[2];

            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.", nameof(predicateString));
            }
            var changedType = Convert.ChangeType(valueString.Trim('"'), propertyInfo.PropertyType);

            var constant = Expression.Convert(Expression.Constant(changedType, propertyInfo.PropertyType), propertyInfo.PropertyType);

            var parameter = Expression.Parameter(typeof(T), "x");

            var member = Expression.Property(parameter, propertyInfo);

            Expression body = @operator switch
            {
                "==" => Expression.Equal(member, constant),
                "!=" => Expression.NotEqual(member, constant),
                ">" => Expression.GreaterThan(member, constant),
                ">=" => Expression.GreaterThanOrEqual(member, constant),
                "<" => Expression.LessThan(member, constant),
                "<=" => Expression.LessThanOrEqual(member, constant),
                "Contains" => Expression.Call(member, "Contains", null, constant),
                "StartsWith" => Expression.Call(member, "StartsWith", null, constant),
                "EndsWith" => Expression.Call(member, "EndsWith", null, constant),
                _ => throw new ArgumentException($"Unsupported operator '{@operator}'", nameof(predicateString)),
            };

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static PredicateResult Simplification(string predicateString)
        {
            var pattern = @"[^&|()]+";
            var matches = Regex.Matches(predicateString, pattern);

            var expressions = matches.Select(m => m.Value.Trim()).Where(e => !string.IsNullOrWhiteSpace(e)).ToList();

            var placeholderDict = new Dictionary<string, string>();
            string simplifiedPredicate = predicateString;

            foreach (var item in expressions)
            {
                var key = Guid.NewGuid().ToString();
                placeholderDict.Add(key, item);
                simplifiedPredicate = simplifiedPredicate.Replace(item, key);
            }

            return new PredicateResult(simplifiedPredicate, placeholderDict);
        }

        private static Expression ParseExpression<T>(string expression, Dictionary<string, Expression<Func<T, bool>>> expressions, ParameterExpression parameter)
        {
            var stack = new Stack<Expression>();
            var operators = new Stack<string>();
            var tokens = Regex.Split(expression, @"(\(|\)|\&\&|\|\|)").Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToList();

            foreach (var token in tokens)
            {
                if (token == "&&" || token == "||")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        var op = operators.Pop();
                        var right = stack.Pop();
                        var left = stack.Pop();
                        stack.Push(op == "&&" ? Expression.AndAlso(left, right) : Expression.OrElse(left, right));
                    }
                    operators.Push(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Peek() != "(")
                    {
                        var op = operators.Pop();
                        var right = stack.Pop();
                        var left = stack.Pop();
                        stack.Push(op == "&&" ? Expression.AndAlso(left, right) : Expression.OrElse(left, right));
                    }
                    operators.Pop(); // Remove the "("
                }
                else
                {
                    stack.Push(Expression.Invoke(expressions[token], parameter));
                }
            }

            while (operators.Count > 0)
            {
                var op = operators.Pop();
                var right = stack.Pop();
                var left = stack.Pop();
                stack.Push(op == "&&" ? Expression.AndAlso(left, right) : Expression.OrElse(left, right));
            }

            return stack.Single();
        }
    }
}
