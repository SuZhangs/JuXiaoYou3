namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public abstract class Browsable : ObservableObject
    {
        private string _name;
        private string _uid;

        /// <summary>
        /// 获取或设置 <see cref="Uid"/> 属性。
        /// </summary>
        public string Uid
        {
            get => _uid;
            set => SetValue(ref _uid, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }

    public abstract class BrowsableRoot : Browsable, IBrowsableRoot
    {
        protected BrowsableRoot()
        {
            InternalCollection = new ObservableCollection<IBrowsable>();
            Children           = new ReadOnlyObservableCollection<IBrowsable>(InternalCollection);
        }
        
        protected ObservableCollection<IBrowsable> InternalCollection { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyObservableCollection<IBrowsable> Children { get; init; }
    }

    public abstract class BrowsableElement : Browsable, IBrowsableElement
    {
        
    }

    public interface IBrowsable
    {
        string Uid { get; }
        string Name { get; set; }
    }


    public interface IBrowsableRoot : IBrowsableElement
    {
        /// <summary>
        /// 可阅览的子节点 
        /// </summary>
        ReadOnlyObservableCollection<IBrowsable> Children { get; init; }
    }


    public interface IBrowsableElement : IBrowsable
    {
    }
}