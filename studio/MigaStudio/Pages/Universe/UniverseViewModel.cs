﻿using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms.VisualStyles;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Models;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public partial class UniverseViewModel : TabViewModel
    {
        private readonly Subject<Album>   _threadSafeAdding;
        private readonly DatabaseProperty _databaseProperty;

        private Album  _selectedAlbum;
        
        public UniverseViewModel()
        {
            PictureCollection = new ObservableCollection<Album>();
            DatabaseManager   = Xaml.Get<IDatabaseManager>();
            ImageEngine       = DatabaseManager.GetEngine<ImageEngine>();
            Database          = DatabaseManager.Database.CurrentValue;
            _databaseProperty = Database.Get<DatabaseProperty>();
            _threadSafeAdding = new Subject<Album>().DisposeWith(Collector);
            _threadSafeAdding.ObserveOn(Scheduler)
                             .Subscribe(x =>
                             {
                                 if (PictureCollection.Any(y => y.Source == x.Source))
                                 {
                                     Warning(Language.ItemDuplicatedText);
                                     return;
                                 }

                                 PictureCollection.Add(x);
                                 _databaseProperty.Album.Add(x);
                                 SelectedAlbum ??= PictureCollection.FirstOrDefault();
                                 Save();
                             })
                             .DisposeWith(Collector);
            AddAlbumCommand       = AsyncCommand(AddAlbumImpl);
            RemoveAlbumCommand    = AsyncCommand<Album>(RemoveAlbumImpl, HasItem);
            ShiftUpAlbumCommand   = Command<Album>(ShiftUpAlbumImpl, HasItem);
            ShiftDownAlbumCommand = Command<Album>(ShiftDownAlbumImpl, HasItem);
            OpenAlbumCommand      = Command<Album>(OpenAlbumImpl, HasItem);
        }

        public override void OnStart()
        {
            if (_databaseProperty.Album is null)
            {
                _databaseProperty.Album = new ObservableCollection<Album>();
                Save();
            }

            PictureCollection.AddMany(_databaseProperty.Album, true);
        }

        public override void Stop()
        {
            Save();
        }

        private void Save()
        {
            Database.Set(_databaseProperty);
        }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _databaseProperty.Intro;
            set
            {
                _databaseProperty.Intro = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="ForeignName"/> 属性。
        /// </summary>
        public string ForeignName
        {
            get => _databaseProperty.ForeignName;
            set
            {
                _databaseProperty.ForeignName = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }
        
        public string Author
        {
            get => _databaseProperty.Author;
            set
            {
                _databaseProperty.Author = value;
                SetDirtyState();
                RaiseUpdated();
            }
        }
        
        public string Name
        {
            get => _databaseProperty.Name;
            set
            {
                _databaseProperty.Name = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Icon
        {
            get => _databaseProperty.Icon;
            set
            {
                _databaseProperty.Icon = value;
                RaiseUpdated();
                SetDirtyState();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Album> PictureCollection { get; init; }

        public IDatabase Database { get; }
        public IDatabaseManager DatabaseManager { get; }
        public ImageEngine ImageEngine { get; }

        public override bool Uniqueness => true;
    }
}