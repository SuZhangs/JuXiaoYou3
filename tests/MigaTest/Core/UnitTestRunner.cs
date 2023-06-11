using System.Reflection;

namespace Acorisoft.FutureGL.MigaTest.Core
{
    public static class UnitTestRunner
    {
        public static void Run(IUnitTestContext context)
        {
            if (context.Case == UnitTestCase.ViewModel)
            {
                testViewModel(context);
            }
            else if (context.Case == UnitTestCase.DialogViewModel)
            {
                
            }
            else if (context.Case == UnitTestCase.Model)
            {
                
            }
            else
            {
                
            }
        }


        private static void testViewModel(IUnitTestContext context)
        {
            
        }

        private static void PropertyStateCheck(IUnitTestContext context)
        {
            //
            // 检查应该为NULL
            //
            // AssertPropertyWasNull
            // AssertPropertyWasNotNull
            // AssertPropertyInRange
            
            foreach (var property in context.Properties)
            {
                if (!property.IsDefined(typeof(Property)))
                {
                    continue;
                }
            }
        }
        
        
        private static void FieldStateCheck(IUnitTestContext context)
        {
            
        }
        
        private static void ConstructorProcedureCheck(IUnitTestContext context)
        {
            // 生成对象
            context.Instance = Activator.CreateInstance(context.InstanceType);
            
            
            // 检查属性
            PropertyStateCheck(context);

            // 检查字段
            FieldStateCheck(context);
        }
    }
}