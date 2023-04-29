using System.Reactive.Linq;
using System.Windows.Input;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    public abstract partial class ComposeEditorBase : TabViewModel
    {
        private readonly Dictionary<string, DataPart> _DataPartTrackerOfId;
        private readonly Dictionary<Type, DataPart>   _DataPartTrackerOfType;

        protected ComposeEditorBase()
        {
            _DataPartTrackerOfType = new Dictionary<Type, DataPart>();
            _DataPartTrackerOfId   = new Dictionary<string, DataPart>(StringComparer.OrdinalIgnoreCase);

            var dbMgr = Xaml.Get<IDatabaseManager>();
            Xaml.Get<IAutoSaveService>()
                .Observable
                .ObserveOn(Scheduler)
                .Subscribe(_ => { Save(); })
                .DisposeWith(Collector);
            ComposeContainer = new ComposeContainer();
            DataParts        = new ObservableCollection<DataPart>();
            Services         = new ObservableCollection<ComposeService>();
            DatabaseManager  = dbMgr;
            ComposeEngine    = dbMgr.GetEngine<ComposeEngine>();
            ImageEngine      = dbMgr.GetEngine<ImageEngine>();
            KeywordEngine    = dbMgr.GetEngine<KeywordEngine>();

            
            SaveComposeCommand = Command(Save);
            NewComposeCommand  = AsyncCommand(async () => await ComposeUtilities.AddComposeAsync(ComposeEngine));

            AddKeywordCommand    = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand = AsyncCommand<string>(RemoveKeywordImpl, x => !string.IsNullOrEmpty(x));

            Initialize();
        }

        private void Save()
        {
            ComposeEngine.UpdateCompose(Compose, Cache);
            SetDirtyState(false);
            Successful(SubSystemString.OperationOfAutoSaveIsSuccessful);
        }

        public sealed override void Stop()
        { 
            Save();
            base.Stop();
        }

        private void Initialize()
        {
            // CreateSubViews(InternalSubViews);
            // SelectedSubView = InternalSubViews.FirstOrDefault();

            //
            // 添加绑定
            AddKeyBinding(ModifierKeys.Control, Key.S, Save);
        }

        private void ActivateAllEngines()
        {
            var engines = new DataEngine[]
            {
                ImageEngine,
                ComposeEngine,
                KeywordEngine,
            };

            foreach (var engine in engines)
            {
                if (!engine.Activated)
                {
                    engine.Activate();
                }
            }
        }
        
        protected override void OnDirtyStateChanged(bool state)
        {
            if (state)
            {
                SetTitle(Compose.Name, true);
            }
        }

        protected override void OnStart(Parameter parameter)
        {
            ActivateAllEngines();
            Cache   = (ComposeCache)parameter.Args[0];
            Compose = ComposeEngine.GetCompose(Cache.Id);

            Open();

            base.OnStart(parameter);
        }

        //
        // ComposeManager Part
        protected void Open()
        {
            if (Compose is null)
            {
                //
                // 创建文档
                CreateComposeImpl();
            }

            // 加载文档
            LoadComposeImpl();
        }

        #region OnLoad

        private void LoadComposeImpl()
        {
            LoadDataPart();
            IsDataPartExistence();
        }

        private void LoadDataPart()
        {
            foreach (var part in Compose.Parts)
            {
                if (!string.IsNullOrEmpty(part.Id) &&
                    !_DataPartTrackerOfId.TryAdd(part.Id, part))
                {
                    Xaml.Get<ILogger>()
                        .Warn($"部件没有ID或者部件重复不予添加，部件ID：{part.Id}");
                    continue;
                }

                if (_DataPartTrackerOfType.TryAdd(part.GetType(), part))
                {
                    if (part is PartOfMarkdown pom)
                    {
                        Markdown = pom;
                    }
                    else if (part is PartOfAlbum poa)
                    {
                        Album = poa;
                        Services.Add(ComposeContainer.GetService(poa));
                    }
                    else if (part is PartOfManifest pom2)
                    {
                        DataParts.Add(part);
                        Services.Add(ComposeContainer.GetService(pom2));
                    }
                }
            }
        }

        private void IsDataPartExistence()
        {
            //
            // 检查当前打开的文档是否缺失指定的DataPart
            if (Markdown is null)
            {
                Markdown = new PartOfMarkdown();
                Compose.Parts.Add(Markdown);
                _DataPartTrackerOfId.TryAdd(Markdown.Id, Markdown);
                _DataPartTrackerOfType.TryAdd(Markdown.GetType(), Markdown);
            }

            if (Album is null)
            {
                Album = new PartOfAlbum();
                Compose.Parts.Add(Album);
                _DataPartTrackerOfId.TryAdd(Album.Id, Album);
                _DataPartTrackerOfType.TryAdd(Album.GetType(), Album);
            }

            //
            // 检查部件的缺失
            IsDataPartExistence(Compose);
        }

        protected abstract void IsDataPartExistence(Compose document);

        #endregion

        #region OnCreate

        private void CreateComposeImpl()
        {
            var document = new Compose
            {
                Id    = Cache.Id,
                Name  = Cache.Name,
                Parts = new DataPartCollection(),
                Metas = new MetadataCollection(),
            };

            //
            //
            Compose = document;
            CreateComposeFromManifest(document);
            OnCreateCompose(document);

            ComposeEngine.AddCompose(document);
        }

        private static void CreateComposeFromManifest(Compose document)
        {
            document.Parts.Add(new PartOfMarkdown());
            document.Parts.Add(new PartOfAlbum());
        }

        protected abstract void OnCreateCompose(Compose document);

        #endregion

        private async Task AddKeywordImpl()
        {
            await DocumentUtilities.AddKeyword(Keywords,
                KeywordEngine,
                SetDirtyState,
                Warning);
        }

        private async Task RemoveKeywordImpl(string item)
        {
            await DocumentUtilities.RemoveKeyword(
                item,
                Keywords,
                KeywordEngine,
                SetDirtyState,
                DangerousOperation);
        }

        public string Content
        {
            get => Markdown.Content;
            set
            {
                Markdown.Content = value;
                RaiseUpdated();
            }
        }
        
        public string Name
        {
            get => Cache.Name;
            set
            {
                Cache.Name   = value;
                Compose.Name = value;
                RaiseUpdated();
            }
        }


        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<string> RemoveKeywordCommand { get; }

        [NullCheck(UniTestLifetime.Startup)]
        public ObservableCollection<string> Keywords => Cache.Keywords;

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<DataPart> DataParts { get; }


        [NullCheck(UniTestLifetime.Startup)]
        public ObservableCollection<ComposeService> Services { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ComposeEngine ComposeEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public KeywordEngine KeywordEngine { get; }

        public PartOfMarkdown Markdown { get; private set; }
        public PartOfAlbum Album { get; private set; }
        public ComposeContainer ComposeContainer { get; }
        public Compose Compose { get; protected set; }
        public ComposeCache Cache { get; protected set; }
    }
}