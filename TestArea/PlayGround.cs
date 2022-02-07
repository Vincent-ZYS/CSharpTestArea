using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestArea
{
    /*PlayGround for more test*/
    class PlayGround
    {
        public int Run(int ii, int pp)
        {
            return ii + pp;
        }
    }

    class ExpressionTool
    {
        public ExpressionTool(object obj, string methodName)
        {
            MethodInfo methodInfo = obj.GetType().GetMethod(methodName);
            if (methodInfo != null)
            {
                GetParameters(methodInfo);
            }
        }

        public ExpressionTool(MethodInfo methodInfo)
        {
            if (methodInfo != null)
            {
                GetParameters(methodInfo);
            }
        }

        public void GetParameters(MethodInfo methodInfo)
        {
            ParameterExpression instanceP = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersP = Expression.Parameter(typeof(object[]), "parameters");

            List<Expression> parametersExps = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();

            for (int i = 0; i < paramInfos.Length; i++)
            {
                BinaryExpression valueObj = Expression.ArrayIndex(parametersP, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);
                parametersExps.Add(valueCast);
            }
        }
    }
}
