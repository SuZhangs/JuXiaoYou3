using Acorisoft.FutureGL.MigaTest.Core;

namespace Acorisoft.FutureGL.MigaTest
{
    public class Tester
    {
        public static void Test<T>()
        {
            var targetType = typeof(T);
            var context    = Prepare(targetType);

            Run(context);
        }

        public static IUnitTestContext Prepare(Type type)
        {
            return null;
        }

        public static void Run(IUnitTestContext context)
        {
            UnitTestRunner.Run(context);
        }
        
        public static void Initialize(string configFileName)
        {
            // test project path
            // 
        }
    }
}