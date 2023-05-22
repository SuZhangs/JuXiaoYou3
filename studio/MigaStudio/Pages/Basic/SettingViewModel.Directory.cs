using System.Diagnostics;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public partial class SettingViewModel
    {
        private static FolderCounter CreateApplicationCounter()
        {
            return new FolderCounter
            {
                Directory = AppDomain.CurrentDomain.BaseDirectory,
                Name      = Language.GetText("global.appSelf")
            };
        }
        
        private static FolderCounter CreateLogCounter()
        {
            return new FolderCounter
            {
                Directory = Xaml.Get<ApplicationModel>()
                                .Logs,
                Name = Language.GetText("global.logs")
            };
        }
        
        private static DatabaseCounter CreateSelfCounter(FolderCounter app, FolderCounter logs)
        {
            return new DatabaseCounter
            {
                Name = Language.GetText("global.appSelf"),
                Counters = new ObservableCollection<FolderCounter>
                {
                    app,
                    logs
                },
                Directory = app.Directory
            };
        }
        
        private static DatabaseCounter CreateDatabaseCounter()
        {
            return new DatabaseCounter
            {
                Counters = new ObservableCollection<FolderCounter>(),
                Directory = Studio.DatabaseManager()
                                  .Database
                                  .CurrentValue
                                  .DatabaseDirectory,
                Name = Language.GetText("__Universe"),
            };
        }
        
        
        private void OnDataSourceChanged(ValueTuple<long, long, long, EngineCounter[]> x)
        {
            var (app, log, db, engines) = x;
            Application.Size            = app;
            Logs.Size                   = log;
            Self.Size                   = Logs.Size + Application.Size;
            Application.Percent         = Application.Size / (double)Self.Size * 100;
            Logs.Percent                = Logs.Size / (double)Self.Size * 100;

            //
            //
            DatabaseCounter.Counters
                           .AddMany(engines, true);
            DatabaseCounter.Size = engines.Sum(e => e.Size) + db;
            foreach (var engine in engines)
            {
                engine.Percent = engine.Size / (double)DatabaseCounter.Size * 100;
            }
        }

        private void OpenCounter(FolderCounter x)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName        = "explorer.exe",
                Arguments       = x.Directory
            });
        }
        
        public void ComputeDirectorySize()
        {
            Task.Run(async () =>
            {
                var appSize = await IOSystemUtilities.GetFolderSize(Application.Directory, true);
                var logSize = await IOSystemUtilities.GetFolderSize(Logs.Directory, false);
                var engines = Studio.DatabaseManager()
                                    .GetEngines()
                                    .OfType<FileEngine>()
                                    .Select(x => new EngineCounter
                                    {
                                        Directory = x.FullDirectory,
                                        Name      = Language.GetText(x.GetTextSource())
                                    })
                                    .ToArray();
                var databaseDirSize = await IOSystemUtilities.GetFolderSize(DatabaseCounter.Directory, false);
                foreach (var engine in engines)
                {
                    engine.Size = await IOSystemUtilities.GetFolderSize(engine.Directory, false);
                }

                _subject.OnNext(new ValueTuple<long, long,long, EngineCounter[]>(appSize, logSize,databaseDirSize, engines));
            });
            
        }
        
        
        
        [NullCheck(UniTestLifetime.Constructor)]
        public DatabaseCounter DatabaseCounter { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public DatabaseCounter Self { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public FolderCounter Application { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public FolderCounter Logs { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand RefreshCommand { get; }


        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<FolderCounter> OpenCommand { get; }
    }
}