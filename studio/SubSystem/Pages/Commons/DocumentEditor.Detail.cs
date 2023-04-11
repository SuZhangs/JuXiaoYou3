using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private PartOfDetail _selectedDetailPartInDocument;
        
        protected virtual IEnumerable<object> CreateDetailPartList()
        {
            return new object[]
            {
                new PartOfAlbum { Items       = new List<Album>()},
                new PartOfPlaylist { Items = new List<Music>() },
                new PartOfRel()
            };
        }

        protected void AddDetailPart(PartOfDetail part)
        {
            part.Index = DetailParts.Count;
            DetailParts.Add(part);
            Document.Parts.Add(part);
        }

        private async Task AddDetailPartImpl()
        {
            var hash = DetailParts.Select(x => x.GetType())
                                  .ToHashSet();

            var iterator = CreateDetailPartList().Where(x => !hash.Contains(x.GetType()))
                                                 .ToArray();

            if (iterator.Length == 0)
            {
                await Warning(SubSystemString.NoMoreData);
                return;
            }

            var r = await SubSystem.OptionSelection<PartOfDetail>(
                SubSystemString.SelectTitle,
                null,
                iterator);

            if (!r.IsFinished)
            {
                return;
            }

            var part = r.Value;
            AddDetailPart(part);

            //
            //
            Successful(SubSystemString.OperationOfAddIsSuccessful);
            ResortDetailPart();
            SetDirtyState();
        }

        private void ResortDetailPart()
        {
            for (var i = 0; i < DetailParts.Count; i++)
            {
                DetailParts[i].Index = i;
            }

            SetDirtyState();

            RemoveDetailPartCommand.NotifyCanExecuteChanged();
            ShiftUpDetailPartCommand.NotifyCanExecuteChanged();
            ShiftDownDetailPartCommand.NotifyCanExecuteChanged();
        }

        private async Task RemoveDetailPartImpl(PartOfDetail part)
        {
            if (part is null ||
                !part.Removable)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (ReferenceEquals(SelectedDetailPart, part))
            {
                SelectedDetailPart           = null;
                SelectedDetailPartInDocument = null;
            }

            DetailParts.Remove(part);
            ResortDetailPart();
            SetDirtyState();
        }

        private void ShiftDownDetailPartImpl(PartOfDetail module)
        {
            DetailParts.ShiftDown(module, (_, _, _) => ResortDetailPart());
        }

        private void ShiftUpDetailPartImpl(PartOfDetail module)
        {
            DetailParts.ShiftUp(module, (_, _, _) => ResortDetailPart());
        }


        /// <summary>
        /// 获取或设置 <see cref="DetailPart"/> 属性。
        /// </summary>
        public FrameworkElement DetailPart
        {
            get => _detailPartOfDetail;
            private set => SetValue(ref _detailPartOfDetail, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedDetailPart"/> 属性。
        /// </summary>
        public object SelectedDetailPart
        {
            get => _selectedDetailPart;
            set
            {
                SetValue(ref _selectedDetailPart, value);

                if (_selectedDetailPart is IPartOfDetail detail)
                {
                    DetailPart = ViewMapper.GetView(this, detail);
                    (DetailPart.DataContext as ViewModelBase)?.Start();
                }
                else
                {
                    DetailPart = value as FrameworkElement;
                }
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="SelectedDetailPartInDocument"/> 属性。
        /// </summary>
        public PartOfDetail SelectedDetailPartInDocument
        {
            get => _selectedDetailPartInDocument;
            set
            {
                SetValue(ref _selectedDetailPartInDocument, value);
                RemoveDetailPartCommand.NotifyCanExecuteChanged();
                ShiftUpDetailPartCommand.NotifyCanExecuteChanged();
                ShiftDownDetailPartCommand.NotifyCanExecuteChanged();
            }
        }
        
        /// <summary>
        /// 自定义部件
        /// </summary>
        /// <remarks>自定义部件会出现在【设定】-【基础信息】当中，用户可以添加删除部件、调整部件顺序。</remarks>
        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<PartOfDetail> DetailParts { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddDetailPartCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PartOfDetail> ShiftUpDetailPartCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<PartOfDetail> ShiftDownDetailPartCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<PartOfDetail> RemoveDetailPartCommand { get; }
    }
}