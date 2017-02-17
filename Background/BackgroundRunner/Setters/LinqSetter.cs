using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Performance.Setters
{
    /// <summary>
    /// Providing an way to set the original\newly created instance back to its container using
    /// Linq Expression of the form
    /// () => prop
    /// or
    /// () => field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class LinqSetter<T> : ISetter<T>
    {
        private MemberExpression expressionBody;

        public LinqSetter(Expression<Func<T>> setBackExpression)
        {           
            expressionBody = (MemberExpression)setBackExpression.Body;
        }

        public void SetBack(T instance)
        {
            var constExpressioVal = ((ConstantExpression)(expressionBody.Expression)).Value;

            if (expressionBody.Member is PropertyInfo)
            {
                ((PropertyInfo)expressionBody.Member).SetValue(constExpressioVal, instance);
            }
            else if (expressionBody.Member is FieldInfo)
            {
                ((FieldInfo)expressionBody.Member).SetValue(constExpressioVal, instance);
            }
            else
            {                
                throw new InvalidOperationException("Linq expression for setting back value is invalid");
            }            
        }
    }
}
