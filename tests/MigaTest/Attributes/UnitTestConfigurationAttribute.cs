namespace Acorisoft.FutureGL.MigaTest.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class UnitTestConfigurationAttribute : Attribute
    {
        public string ConfigureFileName { get; set; }
    }
}