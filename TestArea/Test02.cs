using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*Test02*/
/*通过Expression调用实体的方法*/
namespace TestArea
{
    public class ServiceProvider
    {
        private object m_type;
        public object GetService<T>(T type)
        {
           return m_type = type;
        }

        public void Run()
        {
            Console.WriteLine("Is Running the Type:" + m_type.GetType());
        }
    }

    public class DynamicMethodExecutor
    {
        private readonly Func<object, object[], object> m_execute;

        public DynamicMethodExecutor(object obj, string method)
        {
            MethodInfo methodInfo = obj.GetType().GetMethod(method);
            this.m_execute = GetExecuteDelegate(methodInfo);
        }

        public DynamicMethodExecutor(MethodInfo methodInfo)
        {
            this.m_execute = GetExecuteDelegate(methodInfo);
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="instance">实体对象</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public object Execute(object instance, object[] parameters)
        {
            return this.m_execute(instance, parameters);
        }
        private static Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo)
        {
            // parameters to execute
            ParameterExpression instanceParameter =
                Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter =
                Expression.Parameter(typeof(object[]), "parameters");

            // build parameter list
            List<Expression> parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                // (Ti)parameters[i]
                BinaryExpression valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));

                UnaryExpression valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }

            // non-instance for static method, or ((TInstance)instance)
            Expression instanceCast = methodInfo.IsStatic ? null :
                Expression.Convert(instanceParameter, methodInfo.ReflectedType);

            // static invoke or ((TInstance)instance).Method
            MethodCallExpression methodCall = Expression.Call(
                instanceCast, methodInfo, parameterExpressions);

            // ((TInstance)instance).Method((T0)parameters[0], (T1)parameters[1], ...)
            if (methodCall.Type == typeof(void))
            {
                Expression<Action<object, object[]>> lambda =
                    Expression.Lambda<Action<object, object[]>>(
                        methodCall, instanceParameter, parametersParameter);

                Action<object, object[]> execute = lambda.Compile();
                return (instance, parameters) =>
                {
                    execute(instance, parameters);
                    return null;
                };
            }
            else
            {
                UnaryExpression castMethodCall = Expression.Convert(
                    methodCall, typeof(object));
                Expression<Func<object, object[], object>> lambda =
                    Expression.Lambda<Func<object, object[], object>>(
                        castMethodCall, instanceParameter, parametersParameter);

                return lambda.Compile();
            }
        }
    }
}