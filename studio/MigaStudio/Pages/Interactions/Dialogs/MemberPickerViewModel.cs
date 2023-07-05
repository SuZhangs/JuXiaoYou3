using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    public class MemberPickerViewModel : ImplicitDialogVM
    {
        private MemberCache _selected;

        public MemberPickerViewModel()
        {
            Documents = new ObservableCollection<MemberCache>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;

            if (a[0] is IEnumerable<MemberCache> enumerable)
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
                                  .Cast<MemberCache>()
                                  .ToArray();
        }

        protected override string Failed() => SubSystemString.Unknown;


        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public MemberCache Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }

        public ObservableCollection<MemberCache> Documents { get; }
        public ListBox TargetElement { get; set; }

        public static Task<Op<IEnumerable<MemberCache>>> Pick(IEnumerable<MemberCache> documents)
        {
            return DialogService()
                .Dialog<IEnumerable<MemberCache>, NewMemberViewModel>(new Parameter
                {
                    Args = new object[]
                    {
                        documents
                    }
                });
        }
    }
}