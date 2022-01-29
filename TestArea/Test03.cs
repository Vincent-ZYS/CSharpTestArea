using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/*Test03*/
/*通过表达式获取成员属性*/
namespace TestArea
{
    [Description("Only identify")]
    class PersonModel
    {
        [Description("唯一标识")]
        public string ID { get; set; }
        [Description("名称")]
        public string Name { get; set; }
        [Description("值")]
        public double Value { get; set; }
        [Description("年龄")]
        public double Age { get; set; }
        [Description("收入")]
        public double InCome { get; set; }
        [Description("支出")]
        public double Pay { get; set; }

        public Tuple<string, string> GetPropertyValue<T>(T instance, Expression<Func<T, string>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            string propertyName = memberExpression.Member.Name;
            var attrs = memberExpression.Member.GetCustomAttributes(false);
            string attributeName = (attrs[0] as DescriptionAttribute).Description;

            var property = typeof(T).GetProperties().Where(l => l.Name == propertyName).First();

            return new Tuple<string, string>(attributeName, property.GetValue(instance).ToString());
        }

        public void Test()
        {
            PersonModel model = new PersonModel();
            model.ID = "1";
            model.Name = "ZYS";
            model.Value = 90;
            model.InCome = 100;
            model.Pay = 10;
            model.Age = 24;

            var result = this.GetPropertyValue(model, l => l.Name);

            Console.WriteLine($"Description:{ result.Item1} -Name:{result.Item2 }");
        }
    }
}
