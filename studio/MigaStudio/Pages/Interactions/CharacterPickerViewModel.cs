﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public class CharacterPickerViewModel : ExplicitDialogVM
    {
        private SocialCharacter _selected;
        
        public static Task<Op<IEnumerable<SocialCharacter>>> MultiSelectExclude(IEnumerable<SocialCharacter> characters, ISet<string> pool)
        {
            var documents = characters.Where(x => !pool.Contains(x.Id));
            return DialogService()
                       .Dialog<IEnumerable<SocialCharacter>, CharacterPickerViewModel>(new Parameter
                       {
                           Args = new object[]
                           {
                               documents
                           }
                       });
        }

        public CharacterPickerViewModel()
        {
            Documents = new ObservableCollection<SocialCharacter>();
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            
            if (a[0] is IEnumerable<SocialCharacter> enumerable)
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
                                  .Cast<SocialCharacter>()
                                  .ToArray();
        }

        protected override string Failed() => SubSystemString.Unknown;
        

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public SocialCharacter Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        
        public ObservableCollection<SocialCharacter> Documents { get; }
        public ListBox TargetElement { get; set; }
    }
}