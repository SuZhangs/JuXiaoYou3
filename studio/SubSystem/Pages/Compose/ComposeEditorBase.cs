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
    public abstract partial class ComposeEditorBase : DataPartEditable<ComposeCache, Compose>
    {
        protected ComposeEditorBase()
        {
            var dbMgr = Studio.DatabaseManager();
            Xaml.Get<IAutoSaveService>()
                .Observable
                .ObserveOn(Scheduler)
                .Subscribe(_ => { Save(); })
                .DisposeWith(Collector);
            DataParts       = new ObservableCollection<DataPart>();
            Services        = new ObservableCollection<ComposeService>();
            DatabaseManager = dbMgr;
            ComposeEngine   = dbMgr.GetEngine<ComposeEngine>();
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();

            
            SaveComposeCommand = Command(Save);
            NewComposeCommand  = AsyncCommand(async () => await ComposeUtilities.AddComposeAsync(ComposeEngine));

            Initialize();
        }

        private void Save()
        {
            ComposeEngine.UpdateCompose(Document, Cache);
            SetDirtyState(false);
            this.SuccessfulNotification(SubSystemString.OperationOfSaveIsSuccessful);
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
                SetTitle(Document.Name, true);
            }
        }
        
        protected override void OpeningDocument(ComposeCache cache, Compose document)
        {
            SynchronizeKeywords();
        }

        #region OnLoad

        protected override bool OnDataPartAddingBefore(DataPart part)
        {
            return false;
        }

        protected override void OnDataPartAddingAfter(DataPart part)
        {
            if (DataPartTrackerOfType.TryAdd(part.GetType(), part))
            {
                if (part is PartOfMarkdown pom)
                {
                    Markdown = pom;
                }
                else if (part is PartOfAlbum poa)
                {
                    Album = poa;
                    Services.Add(ServiceViewContainer.GetService(poa));
                }
                else if (part is PartOfManifest pom2)
                {
                    DataParts.Add(part);
                    Services.Add(ServiceViewContainer.GetService(pom2));
                }
            }
        }

        #endregion


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
                Document.Name = value;
                SetDirtyState();
                RaiseUpdated();
            }
        }

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

        public PartOfMarkdown Markdown { get; private set; }
        public PartOfAlbum Album { get; private set; }
    }
}