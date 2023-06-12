using System.Diagnostics;
using Acorisoft.FutureGL.MigaTest.Attributes;
using Acorisoft.FutureGL.MigaTest.Utils;

namespace Acorisoft.FutureGL.MigaTest.Cases.Primitives
{
    public class UnitTestSolution : IUnitTestSolution
    {
        public UnitTestSolution(List<IUnitTestTarget> targets) => Targets = targets.AsReadOnly();
        
        public void Run()
        {
            Trace.WriteLine($"构建完闭，开始测试...");
            foreach (var unitTestTarget in Targets)
            {
                var target = unitTestTarget as UnitTestTarget;
                if (target is null)
                {
                    continue;
                }
                
                //
                // 测试
                AssertViewModel(target);
            }
        }

        private void AssertViewModel(UnitTestTarget target)
        {
            Trace.WriteLine($"正在执行ViewModel单元测试，当前模块:{target.AssemblyName}");
            
            foreach (var vm in target.Case_ViewModel)
            {
                
            }
        }
        
        public IReadOnlyList<IUnitTestTarget> Targets { get; }
    }
}