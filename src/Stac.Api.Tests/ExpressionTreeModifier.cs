using System;
using System.Linq.Expressions;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Queryable;

namespace Stac.Api.Tests
{
    internal class ExpressionTreeModifier : ExpressionVisitor
    {
        private readonly StacQueryProvider _stacQueryProvider;

        public ExpressionTreeModifier(StacQueryProvider stacQueryProvider)
        {
            _stacQueryProvider = stacQueryProvider;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            // Compare Method is proper to STAC filters
            if (node.Method.Name == "Compare")
            {
                var leftPredicate = (Expression<Func<IStacObject, IComparable>>)node.Arguments[1];
                var rightPredicate = (Expression<Func<IStacObject, IComparable>>)node.Arguments[2];
                return Expression.Call(
                        Expression.Constant(_stacQueryProvider),
                            typeof(StacQueryProvider).GetMethod("Compare",
                            new Type[] { typeof(IComparable), typeof(IComparable) }),
                        Expression.Lambda(leftPredicate.Body, leftPredicate.Parameters[0]),
                        Expression.Lambda(rightPredicate.Body, rightPredicate.Parameters[0]));
            }
            return node;
        }

    }
}
