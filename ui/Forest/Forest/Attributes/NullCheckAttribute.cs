namespace Acorisoft.FutureGL.Forest.Attributes
{
    public enum UniTestLifetime
    {
        Constructor,
        StartParameter,
        Start,
        Running
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class NullCheckAttribute : Attribute
    {
        public NullCheckAttribute(UniTestLifetime lifetime)
        {
            Lifetime = lifetime;
        }
        
        public UniTestLifetime Lifetime { get;}
    }
}