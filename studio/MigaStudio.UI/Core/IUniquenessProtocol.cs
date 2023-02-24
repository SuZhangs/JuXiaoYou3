using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface IUniquenessProtocol
    {
        string Id { get; }
    }
    
    public class UniquenessProtocol<TDocument, TIndex> : IUniquenessProtocol
        where TDocument : IUniqueness 
        where TIndex : IUniqueness
    {
        public UniquenessProtocol()
        {
            Parameter = new Parameter();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Id => Parameter.Args[0].ToString();

        /// <summary>
        /// 
        /// </summary>
        public TIndex Cache{
            get
            {
                return (TIndex)Parameter.Args[1];
            }
            init
            {
                Parameter.Args[0] = value.Id;
                Parameter.Args[1] = value;
            } 
        }

        /// <summary>
        /// 设置文档
        /// </summary>
        public TDocument Document {
            get
            {
                return (TDocument)Parameter.Args[1];
            }
            init
            {
                Parameter.Args[0] = value.Id;
                Parameter.Args[2] = value;
            } 
        }
        
        public object Args3 {
            get
            {
                return Parameter.Args[3];
            }
            init
            {
                Parameter.Args[3] = value;
            } 
        }
        
        
        public object Args4 {
            get
            {
                return Parameter.Args[4];
            }
            init
            {
                Parameter.Args[4] = value;
            } 
        }
        
        
        public object Args5 {
            get
            {
                return Parameter.Args[5];
            }
            init
            {
                Parameter.Args[5] = value;
            } 
        }
        
        
        public object Args6 {
            get
            {
                return Parameter.Args[6];
            }
            init
            {
                Parameter.Args[6] = value;
            } 
        }
        
        
        public object Args7 {
            get
            {
                return Parameter.Args[6];
            }
            init
            {
                Parameter.Args[6] = value;
            } 
        }
        
        
        public object Args8 {
            get
            {
                return Parameter.Args[6];
            }
            init
            {
                Parameter.Args[6] = value;
            } 
        }

        internal Parameter Parameter { get; }
    }
}