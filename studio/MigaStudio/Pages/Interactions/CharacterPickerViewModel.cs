using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public class CharacterPickerViewModel : ExplicitDialogVM
    {
        private DocumentCache _selected;
        public static Task<Op<IEnumerable<CharacterUI>>> MultiSelectExclude(IEnumerable<CharacterUI> characters, ISet<string> pool)
        {
            var documents = characters.Where(x => !pool.Contains(x.Id));
            return DialogService()
                       .Dialog<IEnumerable<CharacterUI>, CharacterPickerViewModel>(new Parameter
                       {
                           Args = new object[]
                           {
                               documents
                           }
                       });
        }

        public CharacterPickerViewModel()
        {
            Documents = new ObservableCollection<CharacterUI>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            
            if (a[0] is IEnumerable<CharacterUI> enumerable)
            {
                Documents.AddMany(enumerable, true);
            }
        }

        protected override bool IsCompleted() => Selected is not null;

        protected override void Finish()
        {
            if (TargetElement is null)
            {
                return;
            }

            Result = TargetElement.SelectedItems
                                  .Cast<CharacterUI>()
                                  .ToArray();
        }

        protected override string Failed() => SubSystemString.Unknown;
        

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public DocumentCache Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        
        public ObservableCollection<CharacterUI> Documents { get; }
        public ListBox TargetElement { get; set; }
    }
}