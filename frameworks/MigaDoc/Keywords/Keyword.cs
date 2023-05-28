using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Acorisoft.Miga.Doc.Keywords
{
    public class Keyword : ObservableObject
    {
        private string _name;
        private string _summary;
        
        [BsonId]
        public string Id { get; init; }
        public string Owner { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary
        {
            get => _summary;
            set => SetValue(ref _summary, value);
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

    public class KeywordObject : ObservableObject
    {
        private readonly ReadOnlyObservableCollection<KeywordObject> _collection;
        private readonly IDisposable                                 _disposable;

        public KeywordObject(Node<Keyword, string> node, IScheduler scheduler)
        {
            _disposable = node.Children
                .Connect()
                .Transform(x => new KeywordObject(x, scheduler))
                .ObserveOn(scheduler)
                .Bind(out _collection)
                .Subscribe();

            Source = node.Item;
        }
        
        protected override void ReleaseManagedResources()
        {
            _disposable.Dispose();
        }

        public string Id => Source.Id;
        public Keyword Source { get; }
        public string Name
        {
            get => Source.Name;
            set
            {
                Source.Name = value;
                RaiseUpdated();
            }
        }

        public string Summary => Source.Summary;
        
        public ReadOnlyObservableCollection<KeywordObject> Children => _collection;
    }
}