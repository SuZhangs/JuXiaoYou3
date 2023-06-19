﻿using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Threading;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class StartupViewModel : TabViewModel
    {
        private readonly DatabaseProperty _databaseProperty;
        private          int              _index;
        private          Album            _backgroundCover;

        public StartupViewModel()
        {
            _databaseProperty = Studio.Database()
                                      .Get<DatabaseProperty>();
            Xaml.Get<IAutoSaveService>()
                .MinutesCounter
                .Subscribe(SwitchFantasyProjectBackgroundCover)
                .DisposeWith(Collector);
            SwitchControllerCommand = Command<ControllerManifest>(SwitchControllerImpl);
        }

        protected override void OnStart()
        {
            BackgroundCover = _databaseProperty?.Album?.FirstOrDefault();
            RaiseUpdated(nameof(ProjectName));
            RaiseUpdated(nameof(ProjectIntro));
            RaiseUpdated(nameof(ProjectAuthor));
            RaiseUpdated(nameof(HasBackgroundCover));
            base.OnStart();
        }

        protected override void OnResume()
        {
            RaiseUpdated(nameof(ProjectName));
            RaiseUpdated(nameof(ProjectIntro));
            RaiseUpdated(nameof(ProjectAuthor));
            RaiseUpdated(nameof(HasBackgroundCover));
            RaiseUpdated(nameof(Controllers));
            base.OnResume();
        }

        private void SwitchFantasyProjectBackgroundCover(Unit _)
        {
            if (_databaseProperty.Album is not null &&
                _databaseProperty.Album.Count > 0)
            {
                var count = _databaseProperty.Album.Count;
                _index          = Math.Clamp(_index + 1, 0, count - 1);
                BackgroundCover = _databaseProperty.Album[_index];
            }
        }

        private void SwitchControllerImpl(ControllerManifest manifest)
        {
            if (manifest is null)
            {
                return;
            }

            ModeSwitchViewModel.SwitchTo(Context, manifest);
        }

        public IEnumerable<ControllerManifest> Controllers
        {
            get
            {
                return Context.ControllerList
                              .Where(x => x.Value != AppViewModel.IdOfTabShellController);
            }
        }

        public GlobalStudioContext Context => Xaml.Get<AppViewModel>()
                                                  .Context;


        /// <summary>
        /// 获取或设置 <see cref="BackgroundCover"/> 属性。
        /// </summary>
        public Album BackgroundCover
        {
            get => _backgroundCover;
            set => SetValue(ref _backgroundCover, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HasBackgroundCover"/> 属性。
        /// </summary>
        public bool HasBackgroundCover => _databaseProperty.Album is not null &&
                                          _databaseProperty.Album.Count > 0;

        public string ProjectName => _databaseProperty.Name;
        public string ProjectIntro => _databaseProperty.Intro;
        public string ProjectAuthor => _databaseProperty.Author;

        public RelayCommand<ControllerManifest> SwitchControllerCommand { get; }
    }
}