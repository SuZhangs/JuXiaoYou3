using System;

namespace Acorisoft.FutureGL.MigaTest.Attributes
{
    [Flags]
    public enum GenerateTarget : int
    {
        ViewModel       = 0x1000,
        DialogViewModel = ViewModel + 0x1,
        TabViewModel    = ViewModel + 0x10,
        PageViewModel   = ViewModel + 0x100,
        Model           = 0x2000,
        Control         = 0x4000,
        UI              = 0x8000,
        Page            = 0xF000,
    }

    /// <summary>
    /// 用于代码生成的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UnitTestGeneratedAttribute : Attribute
    {
        public UnitTestGeneratedAttribute(GenerateTarget target)
        {
            GenerateTarget = target;
        }

        public GenerateTarget GenerateTarget { get; }
    }
}