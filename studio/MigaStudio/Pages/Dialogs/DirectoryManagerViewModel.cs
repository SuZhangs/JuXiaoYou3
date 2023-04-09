﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class DirectoryManagerViewModelProxy : BindingProxy<DirectoryManagerViewModel>{}
    
    public class DirectoryManagerViewModel : DialogViewModel
    {
        private readonly Subject<ValueTuple<long, long, EngineCounter[]>> _subject;

        public DirectoryManagerViewModel()
        {
            Database = new DatabaseCounter
            {
                Counters = new ObservableCollection<FolderCounter>(),
                Directory = Xaml.Get<IDatabaseManager>()
                                .Database
                                .CurrentValue
                                .DatabaseDirectory,
                Name = Language.GetText("__Universe"),
            };
            
            Application = new FolderCounter
            {
                Directory = AppDomain.CurrentDomain.BaseDirectory,
                Name      = Language.GetText("global.appSelf")
            };

            Logs = new FolderCounter
            {
                Directory = Xaml.Get<ApplicationModel>()
                                .Logs,
                Name = Language.GetText("global.logs")
            };
            
            Self = new DatabaseCounter
            {
                Name = Language.GetText("global.appSelf"),
                Counters = new ObservableCollection<FolderCounter>
                {
                    Application,
                    Logs
                }
            };

            _subject = new Subject<(long, long, EngineCounter[])>();
            _subject.ObserveOn(Scheduler)
                    .Subscribe(x =>
                    {
                        var (app, log, engines) = x;
                        Application.Size        = app;
                        Logs.Size               = log;
                        Self.Size               = Logs.Size + Application.Size;
                        Application.Percent     = Application.Size / (double)Self.Size * 100;
                        Logs.Percent            = Logs.Size / (double)Self.Size * 100;
                        
                        //
                        //
                        Database.Counters.AddRange(engines, true);
                        Database.Size = engines.Sum(e => e.Size);
                        foreach (var engine in engines)
                        {
                            engine.Percent = engine.Size / (double)Database.Size * 100;
                        }
                    });

            RefreshCommand = Command(ComputeDirectorySize);
            OpenCommand = Command<FolderCounter>(x =>
            {
                Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "explorer.exe",
                    Arguments = x.Directory
                });
            });
        }

        public override void Start()
        {
            ComputeDirectorySize();
            base.Start();
        }

        public void ComputeDirectorySize()
        {
            Task.Run(async () =>
            {
                var appSize = await IOSystemUtilities.GetFolderSize(Application.Directory, true);
                var logSize = await IOSystemUtilities.GetFolderSize(Logs.Directory, false);
                var engines = Xaml.Get<IDatabaseManager>()
                                  .GetEngines()
                                  .OfType<FileEngine>()
                                  .Select(x => new EngineCounter
                                  {
                                      Directory = x.FullDirectory,
                                      Name      = Language.GetText(x.GetTextSource())
                                  })
                                  .ToArray();
                foreach (var engine in engines)
                {
                    engine.Size = await IOSystemUtilities.GetFolderSize(engine.Directory, false);
                }

                _subject.OnNext(new ValueTuple<long, long, EngineCounter[]>(appSize, logSize, engines));
            });
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public DatabaseCounter Database { get; }
        
        
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