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
                Documents.AddRange(enumerable, true);
            }
        }

        protected override bool IsCompleted() => Selected is not null;

        protected override void Finish()
        {
            Result = Selected;
        }

        protected override string Failed() => SubSystemString.Unknown;

        public static Task<Op<DocumentCache>> Select(IEnumerable<DocumentCache> excludeDocuments)
        {
            return DialogService().Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    excludeDocuments
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