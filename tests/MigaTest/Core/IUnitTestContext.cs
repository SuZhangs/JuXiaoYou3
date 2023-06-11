using System.Reflection;

namespace Acorisoft.FutureGL.MigaTest.Core
{
    public interface IUnitTestContext
    {
        UnitTestCase Case { get; }
        
        /// <summary>
        /// 
        /// </summary>
        Type SpecifiedCase { get; }
        
        /// <summary>
        /// 测试实例，实例一旦生成就无法更改。
        /// </summary>
        object Instance { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        Type InstanceType { get; set; }
        
        IEnumerable<FieldInfo> Fields { get; }
        IEnumerable<MethodInfo> Methods { get; }
        IEnumerable<PropertyInfo> Properties { get; }
    }
}