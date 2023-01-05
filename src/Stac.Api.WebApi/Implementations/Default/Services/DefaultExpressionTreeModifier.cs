using System;
using System.Linq.Expressions;
using Stac.Api.Interfaces;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Queryable;

namespace Stac.Api.WebApi.Implementations.Default.Services
{
    internal class DefaultExpressionTreeModifier : ExpressionVisitor
    {
        private readonly DefaultStacQueryProvider _stacQueryProvider;

        public DefaultExpressionTreeModifier(DefaultStacQueryProvider stacQueryProvider)
        {
            _stacQueryProvider = stacQueryProvider;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(IStacQueryProvider) )
            {
                return Expression.Constant(_stacQueryProvider);
            }

            return base.VisitParameter(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
        }

    }
}
