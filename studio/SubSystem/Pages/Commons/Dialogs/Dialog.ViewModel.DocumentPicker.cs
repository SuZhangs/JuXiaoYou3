namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class DocumentPickerViewModel : ExplicitDialogVM
    {
        private DocumentCache _selected;

        public DocumentPickerViewModel()
        {
            Documents = new ObservableCollection<DocumentCache>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            
            if (a[0] is IEnumerable<DocumentCache> enumerable)
            {
                Documents.AddMany(enumerable, true);
            }
        }

        protected override bool IsCompleted() => Selected is not null;

        protected override void Finish()
        {
            Result = Selected;
        }

        protected override string Failed() => SubSystemString.Unknown;

        public static Task<Op<DocumentCache>> Select(IEnumerable<DocumentCache> documents)
        {
            return DialogService().Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }

        public static Task<Op<DocumentCache>> Select(DocumentType type)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches(type);
            return DialogService().Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }
        
        public static Task<Op<DocumentCache>> Select(DocumentType type, ISet<string> idPool)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches(type, idPool);
            return DialogService().Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }
        
        public static Task<Op<DocumentCache>> Select()
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches();
            return DialogService().Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public DocumentCache Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        
        public ObservableCollection<DocumentCache> Documents { get; }
    }
}