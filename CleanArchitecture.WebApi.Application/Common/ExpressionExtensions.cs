using System.Linq.Expressions;

namespace CleanArchitecture.WebApi.Application.Common;

internal static class ExpressionExtensions
{
    internal static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = left.Parameters[0];
        var visitor = new ParameterReplacer(parameter);
        var rightBody = visitor.Visit(right.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, rightBody), parameter);
    }
}

internal sealed class ParameterReplacer(ParameterExpression parameter) : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node) => parameter;
}
