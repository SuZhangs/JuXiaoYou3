
namespace Acorisoft.Miga.Doc.Core
{
    
    /// <summary>
    /// <see cref="StorageObject"/> 类型表示一个存储对象。
    /// </summary>
    public abstract class StorageObject : PropertyChanger, ICloneable
    {
        
        public object Clone()
        {
            //
            // 创建一个新的实例
            var instance = CreateInstanceOverride();

            ShadowCopy(instance);
            
            return instance;
        }

        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <returns>返回一个新的实例</returns>
        protected abstract StorageObject CreateInstanceOverride();

        /// <summary>
        /// 复制当前的数据
        /// </summary>
        /// <param name="target">要复制到的目标实例。</param>
        protected virtual void ShadowCopy(StorageObject target)
        {
            target.Name = Name;
            target.Id = Id;
        }

        /// <summary>
        /// 唯一标识符。
        /// </summary>
        [BsonId]
        public string Id { get; set; }
        
        /// <summary>
        /// 当前对象名。
        /// </summary>
        public string Name { get; set; }
    }
    
    /// <summary>
    /// <see cref="OrderedStorageObject"/> 类型表示一个顺序存储对象。
    /// </summary>
    public abstract class OrderedStorageObject: StorageObject
    {
        protected override void ShadowCopy(StorageObject target)
        {
            //
            //
            if (target is OrderedStorageObject oso)
            {
                oso.Order = Order;
            }
            
            //
            // 拷贝数据
            base.ShadowCopy(target);
        }

        /// <summary>
        /// 当前对象的顺序。
        /// </summary>
        public int Order { get; set; }
    }
}