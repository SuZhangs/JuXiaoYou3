using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class EntityTabViewModel<TEntity> : TabViewModel
    {
        private TEntity _selected;

        protected EntityTabViewModel()
        {
            ApprovalRequired = false;
            Collection       = new ObservableCollection<TEntity>();
            AddCommand       = AsyncCommand(AddEntityImpl);
            EditCommand      = AsyncCommand<TEntity>(EditEntityImpl);
            RemoveCommand    = AsyncCommand<TEntity>(RemoveEntityImpl);
            ClearCommand     = AsyncCommand(ClearEntityImpl);
            SaveCommand      = Command(Save);
            ShiftUpCommand   = Command<TEntity>(ShiftUpEntityImpl);
            ShiftDownCommand = Command<TEntity>(ShiftDownEntityImpl);
        }

        private async Task AddEntityImpl()
        {
            var r = await Add();
            if (!r.IsFinished)
            {
                return;
            }
            
            Collection.Add(r.Value);
        }

        
        private async Task EditEntityImpl(TEntity entity)
        {
            await Edit(entity);
            SetDirtyState();
        }
        
        private async Task RemoveEntityImpl(TEntity entity)
        {
            if (!await DangerousOperation(Language.GetText("text.AreYouSureRemoveIt")))
            {
                return;
            }

            Remove(entity);
            SetDirtyState();

            //
            //
            if (ReferenceEquals(Selected, entity))
            {
                Selected = default(TEntity);
            }
        }
        
        private async Task ClearEntityImpl()
        {
            if (!await DangerousOperation(Language.GetText("text.AreYouSureClearIt")))
            {
                return;
            }

            ClearEntity(Collection.ToArray());
            SetDirtyState();
        }
        
        private void ShiftUpEntityImpl(TEntity entity)
        {
            Collection.ShiftUp(entity, ShiftUp);
            SetDirtyState();
        }
        
        
        private void ShiftDownEntityImpl(TEntity entity)
        {
            Collection.ShiftDown(entity, ShiftDown);
            SetDirtyState();
        }
        
        #region OnStart / OnResume
        
        
        /// <summary>
        /// 需要同步数据源
        /// </summary>
        /// <returns>true为需要，false为不需要</returns>
        protected abstract bool NeedDataSourceSynchronize();

        /// <summary>
        /// 请求同步数据源
        /// </summary>
        /// <param name="dataSource">需要同步的数据源</param>
        protected abstract void OnRequestDataSourceSynchronize(ICollection<TEntity> dataSource);

        public sealed override void OnStart()
        {
            OnRequestDataSourceSynchronize(Collection);
            base.OnStart();
        }

        public override void Resume()
        {
            if (NeedDataSourceSynchronize())
            {
                OnRequestDataSourceSynchronize(Collection);
            }

            base.Resume();
        }

        #endregion

        protected abstract void Save();
        protected abstract Task<Op<TEntity>> Add();
        protected abstract Task Edit(TEntity entity);
        protected abstract void Remove(TEntity entity);
        protected abstract void ShiftUp(TEntity entity, int oldIndex, int newIndex);
        protected abstract void ShiftDown(TEntity entity, int oldIndex, int newIndex);
        protected abstract void ClearEntity(TEntity[] entities);

        protected void UpdateCommands()
        {
            AddCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
            ClearCommand.NotifyCanExecuteChanged();
            EditCommand.NotifyCanExecuteChanged();
            ShiftUpCommand.NotifyCanExecuteChanged();
            ShiftDownCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public TEntity Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);
                UpdateCommands();
            }
        }

        public ObservableCollection<TEntity> Collection { get; }
        public AsyncRelayCommand AddCommand { get; }
        public AsyncRelayCommand<TEntity> EditCommand { get; }
        public AsyncRelayCommand<TEntity> RemoveCommand { get; }
        public AsyncRelayCommand ClearCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand<TEntity> ShiftUpCommand { get; }
        public RelayCommand<TEntity> ShiftDownCommand { get; }
    }
}