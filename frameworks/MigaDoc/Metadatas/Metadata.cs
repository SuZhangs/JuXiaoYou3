
namespace Acorisoft.Miga.Doc.Metadatas
{
    /// <summary>
    /// <see cref="Metadata"/> 类型表示一个元数据。
    /// </summary>
    public class Metadata : ICloneable, IEquatable<Metadata>, IDocumentNameService
    {
        #region ICloneable Interface

        public object Clone()
        {
            //
            // 创建一个新的实例
            var instance = new Metadata
            {
                Name  = Name,
                Value = Value
            };

            return instance;
        }

        #endregion
        
        
        
        /// <summary>
        /// 当前对象名。
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// 获取或设置当前元数据的值。
        /// </summary>
        public string Value { get; set; }
        

        #region ToString / Equals / GetHashCode

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString()
        {
            return $"{Name}-{Value}";
        }

        public bool Equals(Metadata target)
        {
            if (target is null)
            {
                return false;
            }

            if (ReferenceEquals(this, target))
            {
                return true;
            }

            return Name.EqualsWithIgnoreCase(target.Name) &&
                   Value.EqualsWithIgnoreCase(target.Value);
        }

        public override bool Equals(object target)
        {
            if (target is Metadata other)
            {
                return Equals(other);
            }

            return false;
        }

        public sealed override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
        
        #endregion
    }
}