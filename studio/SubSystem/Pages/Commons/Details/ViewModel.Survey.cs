using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class SurveyPartViewModelProxy : BindingProxy<SurveyPartViewModel>
    {
    }
    
    public class SurveyPartViewModel : DetailViewModel<PartOfSurvey>
    {
        private SurveySet _selectedSurveySet;
        
        public SurveyPartViewModel()
        {
            Sets                      = new ObservableCollection<SurveySet>();
            Surveys                   = new ObservableCollection<Survey>();
            AddSurveySetCommand       = AsyncCommand(AddSurveySetImpl);
            AddSurveyCommand          = AsyncCommand<SurveySet>(AddSurveyImpl);
            ShiftUpSurveySetCommand   = Command<SurveySet>(ShiftUpSurveySetImpl);
            ShiftUpSurveyCommand      = Command<Survey>(ShiftUpSurveyImpl);
            ShiftDownSurveySetCommand = Command<SurveySet>(ShiftDownSurveySetImpl);
            ShiftDownSurveyCommand    = Command<Survey>(ShiftDownSurveyImpl);
            EditSurveySetCommand      = AsyncCommand<SurveySet>(EditSurveySetImpl);
            EditSurveyCommand         = AsyncCommand<Survey>(EditSurveyImpl);
            RemoveSurveySetCommand    = AsyncCommand<SurveySet>(RemoveSurveySetImpl);
            RemoveSurveyCommand       = AsyncCommand<Survey>(RemoveSurveyImpl);
        }
        
        private async Task EditSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            var r = await NewSurveyViewModel.Edit(item);

            if (!r.IsFinished)
            {
                return;
            }
            
            Save();
        }

        private async Task EditSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            
            var r = await NewSurveyViewModel.Edit(item);

            if (!r.IsFinished)
            {
                return;
            }
            
            Save();
        }

        private async Task RemoveSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            SelectedSurveySet.Items.Remove(item);
            Surveys.Remove(item);
            
            Save();
        }

        private async Task RemoveSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Sets.Remove(item);
            Detail.Items.Remove(item);

            if (ReferenceEquals(item, SelectedSurveySet))
            {
                SelectedSurveySet = null;
            }
            
            Save();
        }

        private void ShiftDownSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            Surveys.ShiftDown(item);
            SelectedSurveySet.Items.ShiftDown(item);
            Save();
        }

        private void ShiftDownSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            Sets.ShiftDown(item);
            Detail.Items.ShiftDown(item);
            Save();
        }

        private void ShiftUpSurveyImpl(Survey item)
        {
            if (item is null)
            {
                return;
            }

            Surveys.ShiftUp(item);
            SelectedSurveySet.Items.ShiftUp(item);
            Save();
        }

        private void ShiftUpSurveySetImpl(SurveySet item)
        {
            if (item is null)
            {
                return;
            }

            Sets.ShiftUp(item);
            Detail.Items.ShiftUp(item);
            Save();
        }

        private async Task AddSurveyImpl(SurveySet item)
        {
            var r = await NewSurveyViewModel.New();
            
            if (!r.IsFinished)
            {
                return;
            }
            
            item.Items.Add(r.Value);
            if(ReferenceEquals(SelectedSurveySet, item))
            {
                SelectedSurveySet.Items.Add(r.Value);
            }
            Save();
        }

        private async Task AddSurveySetImpl()
        {
            var r = await NewSurveyViewModel.NewSet();
            
            if (!r.IsFinished)
            {
                return;
            }
            
            Sets.Add(r.Value);
            Detail.Items.Add(r.Value);
            Save();
        }

        public override void Start()
        {
            base.Start();

            if (Detail.Items is null)
            {
                Xaml.Get<ILogger>()
                    .Warn("PartOf为空");
                return;
            }

            Sets.AddRange(Detail.Items);
            SelectedSurveySet = Sets.FirstOrDefault();
        }

        private void Save()
        {
            Owner.SetDirtyState();
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SurveySet> Sets { get; }
        public ObservableCollection<Survey> Surveys { get; }

        /// <summary>
        /// 获取或设置 <see cref="SelectedSurveySet"/> 属性。
        /// </summary>
        public SurveySet SelectedSurveySet
        {
            get => _selectedSurveySet;
            set
            {
                SetValue(ref _selectedSurveySet, value);

                if (_selectedSurveySet is null)
                {
                    return;
                }
                
                Surveys.AddRange(_selectedSurveySet.Items, true);
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddSurveySetCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SurveySet> AddSurveyCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<SurveySet> ShiftUpSurveySetCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Survey> ShiftUpSurveyCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<SurveySet> ShiftDownSurveySetCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Survey> ShiftDownSurveyCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SurveySet> RemoveSurveySetCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Survey> RemoveSurveyCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SurveySet> EditSurveySetCommand { get; }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Survey> EditSurveyCommand { get; }
    }
}