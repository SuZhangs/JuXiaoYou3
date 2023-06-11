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
    }
}