using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        private string beforeValueAppend = string.Empty;
        private string afterValueAppend = string.Empty;
        private bool isTranslateIntoE3S = false;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        public string TranslateIntoE3S(Expression exp)
        {
            isTranslateIntoE3S = true;
            _resultStringBuilder.Append(@"""statements"": [");
            Visit(exp);
            _resultStringBuilder.Append("]");

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }
            else if (node.Method.DeclaringType == typeof(string))
            {
                switch (node.Method.Name)
                {
                    case "Contains":
                        beforeValueAppend = "*";
                        afterValueAppend = "*";
                        break;
                    case "EndsWith":
                        beforeValueAppend = "*";
                        break;
                    case "StartsWith":
                        afterValueAppend = "*";
                        break;
                }
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    var nodes = new[] { node.Left, node.Right };
                    var memberNode = nodes.FirstOrDefault(n => n.NodeType == ExpressionType.MemberAccess);
                    var constantNode = nodes.FirstOrDefault(n => n.NodeType == ExpressionType.Constant);
                    if (memberNode is null || constantNode is null)
                        throw new NotSupportedException($"Unable to parse the expression");

                    Visit(memberNode);
                    Visit(constantNode);
                    break;
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    Visit(node.Right);
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (isTranslateIntoE3S)
            {
                _resultStringBuilder.Append(@"{""query"":""");
            }
            _resultStringBuilder
                .Append(node.Member.Name);

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder
                .Append($":({beforeValueAppend}")
                .Append(node.Value)
                .Append($"{afterValueAppend})");

            if (isTranslateIntoE3S)
            {
                _resultStringBuilder.Append(@"""},");
            }

            return node;
        }

        #endregion
    }
}
