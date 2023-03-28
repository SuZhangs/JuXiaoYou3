using System.ComponentModel;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public interface ICustomDataPart
    {
        void Inspect(Document document);
        
        /// <summary>
        /// 获取或设置当前部件的顺序。
        /// </summary>
        int Index { get; set; }
        
        bool Removable { get; }
    }
    
    public abstract class CustomDataPart : DataPart, ICustomDataPart
    {
        /// <summary>
        /// 检查当前文档中是否存在指定类型的部件，如果没有则新建一个该类型的部件。
        /// </summary>
        /// <param name="document"></param>
        public void Inspect(Document document)
        {
            var thisType = GetType();
            
            //
            // 检查文档中是否存在当前属性的部件，如果没有则添加
            if(document.Parts.All(x => x.GetType() != thisType))
            {
                document.Parts.Add(CreateInstance());
            }
        }
        
        protected abstract CustomDataPart CreateInstance();
        
        /// <summary>
        /// 获取或设置当前部件的顺序。
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 获取当前的部件是否可以被删除。
        /// </summary>
        public bool Removable => true;
    }

    public abstract class FixedDataPart : DataPart, ICustomDataPart
    {
        /// <summary>
        /// 检查当前文档中是否存在指定类型的部件，如果没有则新建一个该类型的部件。
        /// </summary>
        /// <param name="document"></param>
        public void Inspect(Document document)
        {
            var thisType = GetType();
            
            //
            // 检查文档中是否存在当前属性的部件，如果没有则添加
            if(document.Parts.All(x => x.GetType() != thisType))
            {
                document.Parts.Add(CreateInstance());
            }
        }
        
        protected abstract FixedDataPart CreateInstance();
        
        /// <summary>
        /// 获取或设置当前部件的顺序。
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 获取当前的部件是否可以被删除。
        /// </summary>
        public bool Removable => false;
    }
}