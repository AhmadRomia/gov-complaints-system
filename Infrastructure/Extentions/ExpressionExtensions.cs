
using System.Linq.Expressions;
using System.Text;


namespace Infrastructure.Extentions
{
    public static class ExpressionExtensions
    {
        public static string AsPath<T>(this Expression<Func<T, object>> expression)
        {
            if (expression == null) return string.Empty;

            Expression body = expression.Body;

            if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
            {
                body = unary.Operand;
            }

            var stack = new StringBuilder();
            while (body is MemberExpression member)
            {
                if (stack.Length > 0)
                    stack.Insert(0, ".");

                stack.Insert(0, member.Member.Name);
                body = member.Expression!;
            }

            return stack.ToString();
        }
    }
}
