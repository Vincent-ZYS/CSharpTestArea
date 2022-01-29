using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestArea
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Test01*/
            /*通过反射来实现调用*/
            /*Has its own main entrance inside the script.*/

            /*Test02*/
            /*通过Expression调用实体的方法*/
            //var _serviceProvider = new ServiceProvider();
            //var service = _serviceProvider.GetService(_serviceProvider);
            //DynamicMethodExecutor dynamicMethodRun = new DynamicMethodExecutor(service, "Run");
            //var redata = dynamicMethodRun.Execute(service, null);
            //Console.WriteLine(redata);

            /*Test03*/
            /*通过表达式获取成员属性*/
            //PersonModel pm = new PersonModel();
            //pm.Test();

            //Simple_Test
            //SimpleTest.TestMethod01();
            SimpleTest.TestMethod02();
        }
    }

    class SimpleTest
    {
        /// <summary>
        /// Reflection Test by using Type & GetMethod
        /// </summary>
        public static void TestMethod01()
        {
            var strType = typeof(String);
            MethodInfo subStr = strType.GetMethod("Substring", new Type[] { typeof(int), typeof(int) });
            object result = subStr.Invoke("Hello, World!", new object[] { 7, 5 });
            Console.WriteLine("{0} returned /{1}/", subStr, result);
        }

        /// <summary>
        /// 下面的代码示例演示如何创建表示类型转换运算的表达式
        /// </summary>
        public static void TestMethod02()
        {
            Expression convertExpr = Expression.Convert(Expression.Constant(5.5), typeof(Int16));
            Console.WriteLine(convertExpr.ToString());

            Console.WriteLine(Expression.Lambda<Func<Int16>>(convertExpr).Compile()());
        }
    }
}