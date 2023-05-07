﻿using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Foundation;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public abstract class DetailViewModel<TDetail> : ViewModelBase where TDetail : PartOfDetail
    {
        public TDetail Detail { get; init; }
        public DocumentEditorBase Owner { get; init; }
    }

    public abstract class DetailViewModel<TPart, TDetail> : DetailViewModel<TPart>
        where TPart : PartOfDetail
        where TDetail : class
    {
        private readonly Subject<TDetail> _threadSafeAdding;
        private          TDetail          _selected;
        private          bool             _hasElement;


        protected DetailViewModel()
        {
            Collection        = new ObservableCollection<TDetail>();
            _threadSafeAdding = new Subject<TDetail>().DisposeWith(Collector);
            _threadSafeAdding.ObserveOn(Scheduler)
                             .Subscribe(OnCollectionChanged)
                             .DisposeWith(Collector);
            AddCommand       = AsyncCommand(AddItem);
            RemoveCommand    = AsyncCommand<TDetail>(RemoveItem, HasItem);
            ShiftUpCommand   = Command<TDetail>(ShiftUpItem, HasItem);
            ShiftDownCommand = Command<TDetail>(ShiftDownItem, HasItem);
        }

        protected void Sync(TDetail item) => _threadSafeAdding.OnNext(item);
        
        public override void Start()
        {
            base.Start();
            OnInitialize(Collection);
        }

        protected override void ReleaseManagedResourcesOverride()
        {
            _threadSafeAdding.Dispose();
        }



        protected void UpdateCollectionState()
        {
            HasElement = Collection.Count > 0;
            Save();
        }

        protected abstract Task AddItem();
        protected abstract Task RemoveItem(TDetail part);
        protected abstract void ShiftDownItem(TDetail album);
        protected abstract void ShiftUpItem(TDetail album);
        protected abstract void OnCollectionChanged(TDetail x);
        protected abstract void Save();
        protected abstract void OnInitialize(ICollection<TDetail> collection);
        protected abstract void UpdateCommandState();

        /// <summary>
        /// 获取或设置 <see cref="HasElement"/> 属性。
        /// </summary>
        public bool HasElement
        {
            get => _hasElement;
            private set => SetValue(ref _hasElement, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TDetail> Collection { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public TDetail Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);
                RemoveCommand.NotifyCanExecuteChanged();
                ShiftDownCommand.NotifyCanExecuteChanged();
                ShiftUpCommand.NotifyCanExecuteChanged();
                UpdateCommandState();
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<TDetail> ShiftUpCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<TDetail> ShiftDownCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<TDetail> RemoveCommand { get; }
    }
}