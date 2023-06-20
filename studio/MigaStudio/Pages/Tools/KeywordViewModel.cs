﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Local")]
    public class KeywordViewModel : TabViewModel
    {
        public static void Initialize(KeywordEngine keywordEngine, ICollection<DirectorySupportUI> collection)
        {
            IEnumerable<DirectorySupportUI> dirs = keywordEngine.GetDirectories()
                                    .Select(Select)
                                    .ToArray();
            var dict = new Dictionary<string, DirectorySupportUI>();
            dirs.ForEach(x => dict.TryAdd(x.Id, x));

            foreach (var dir in dirs)
            {
                if (dir is DirectoryNodeUI dnu &&
                    !string.IsNullOrEmpty(dnu.Owner) &&
                    dict.TryGetValue(dnu.Owner, out var parent))
                {
                    dnu.Parent = parent;
                    parent.Add(dnu);
                }
            }

            collection.AddMany(dirs.OfType<DirectoryRootUI>(), true);
        }

        public static void CreateSubTree(KeywordEngine keywordEngine, ICollection<DirectorySupportUI> collection, string root)
        {
            if (string.IsNullOrEmpty(root))
            {
                return;
            }
            
            DirectorySupportUI[] dirs = keywordEngine.GetDirectories()
                                                   .Select(Select)
                                                   .ToArray();

            var node = keywordEngine.GetDirectory(root);
            collection.Clear();
            collection.Add(node);

            foreach (var dir in dirs.Where(x => x is DirectoryNodeUI dnu && dnu.Owner == root))
            {
                CreateSubTree(dirs, dir, dir.Children, dir.Id);
                node.Add(dir);
            }
            
        }

        private static void CreateSubTree(DirectorySupportUI[] dirs, DirectorySupportUI node, ICollection<DirectorySupportUI> collection, string root)
        {
            foreach (var dir in dirs.Where(x => x is DirectoryNodeUI dnu && dnu.Owner == root))
            {
                CreateSubTree(dirs, dir, dir.Children, dir.Id);
                node.Add(dir);
            }
        }

        public KeywordViewModel()
        {
            Root             = new ObservableCollection<DirectorySupportUI>();
            KeywordEngine    = Studio.Engine<KeywordEngine>();
            AddCommand       = AsyncCommand<DirectorySupportUI>(AddImpl);
            EditCommand      = AsyncCommand<DirectorySupportUI>(EditImpl);
            RemoveCommand    = AsyncCommand<DirectorySupportUI>(RemoveImpl);
            ShiftUpCommand   = Command<DirectorySupportUI>(ShiftUpImpl);
            ShiftDownCommand = Command<DirectorySupportUI>(ShiftDownImpl);
        }

        private static DirectorySupportUI Select(DirectorySupport x)
        {
            if (x is DirectoryRoot dr)
            {
                return new DirectoryRootUI
                {
                    Source   = dr,
                    Children = new ObservableCollection<DirectorySupportUI>()
                };
            }
                                        
            return new DirectoryNodeUI
            {

                Source   = (DirectoryNode)x,
                Children = new ObservableCollection<DirectorySupportUI>()
            };
        }

        protected override void OnStart()
        {
            Initialize(KeywordEngine, Root);
            base.OnStart();
        }

        protected override void OnInvalidateDataSource()
        {
            if (Version != KeywordEngine.Version)
            {
                Initialize(KeywordEngine, Root);
            }
        }


        private async Task AddImpl(DirectorySupportUI parent)
        {
            var r = await SingleLineViewModel.String(Language.GetText("text.AddKeyword"));

            if (!r.IsFinished)
            {
                return;
            }

            if (KeywordEngine.HasDirectory(r.Value))
            {
                await this.WarningNotification(Language.GetText("text.duplicated.keyword"));
                return;
            }

            if (parent is null)
            {
                var dir = new DirectoryRoot
                {
                    Id   = ID.Get(),
                    Name = r.Value,
                };

                var ui = new DirectoryRootUI
                {
                    Source   = dir,
                    Children = new ObservableCollection<DirectorySupportUI>()
                };
                KeywordEngine.AddDirectory(dir);
                Root.Add(ui);
            }
            else
            {
                var dir = new DirectoryNode
                {
                    Id    = ID.Get(),
                    Name  = r.Value,
                    Owner = parent.Id
                };

                var ui = new DirectoryNodeUI
                {
                    Source   = dir,
                    Parent = parent,
                    Children = new ObservableCollection<DirectorySupportUI>()
                };

                parent.Add(ui);
                KeywordEngine.AddDirectory(dir);
            }
        }

        private async Task EditImpl(DirectorySupportUI parent)
        {
            var r = await SingleLineViewModel.String(Language.GetText("text.AddKeyword"));

            if (!r.IsFinished)
            {
                return;
            }

            if (KeywordEngine.HasDirectory(r.Value))
            {
                await this.WarningNotification(Language.GetText("text.duplicated.keyword"));
                return;
            }

            if (parent is DirectoryRootUI dru)
            {
                dru.Name = r.Value;
                KeywordEngine.UpdateDirectory(dru.Source);
                return;
            }

            if (parent is DirectoryNodeUI dnu)
            {
                dnu.Name = r.Value;
                KeywordEngine.UpdateDirectory(dnu.Source);
            }
        }

        private async Task RemoveImpl(DirectorySupportUI parent)
        {
            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (parent is DirectoryRootUI dru)
            {
                Root.Remove(dru);
                KeywordEngine.RemoveDirectory(dru.Source);
                return;
            }
            
            
            if(parent is DirectoryNodeUI dnu)
            {
                dnu.Parent
                   ?.Remove(dnu);
                KeywordEngine.RemoveDirectory(dnu.Source);
            }
            
        }

        private void ShiftUpImpl(DirectorySupportUI parent)
        {
            if (parent is DirectoryRootUI dru)
            {
                Root.ShiftUp(dru, () =>
                {
                    KeywordEngine.UpdateDirectory(dru.Source);
                });
            }
            else if (parent is DirectoryNodeUI dnu)
            {
                
                dnu.Parent
                   .Children
                   .ShiftUp(dnu, () =>
                   {
                       KeywordEngine.UpdateDirectory(dnu.Source);
                   });
            }
        }

        private void ShiftDownImpl(DirectorySupportUI parent)
        {
            if (parent is DirectoryRootUI dru)
            {
                Root.ShiftDown(dru, () =>
                {
                    KeywordEngine.UpdateDirectory(dru.Source);
                });
            }
            else if (parent is DirectoryNodeUI dnu)
            {
                
                dnu.Parent
                   .Children
                   .ShiftDown(dnu, () =>
                {
                    KeywordEngine.UpdateDirectory(dnu.Source);
                });
            }
        }

        private DirectorySupportUI _selected;

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public DirectorySupportUI Selected
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        
        public KeywordEngine KeywordEngine { get; }
        public AsyncRelayCommand<DirectorySupportUI> AddCommand { get; }
        public AsyncRelayCommand<DirectorySupportUI> EditCommand { get; }
        public AsyncRelayCommand<DirectorySupportUI> RemoveCommand { get; }
        public RelayCommand<DirectorySupportUI> ShiftUpCommand { get; }
        public RelayCommand<DirectorySupportUI> ShiftDownCommand { get; }
        public ObservableCollection<DirectorySupportUI> Root { get; }

        public sealed override bool Uniqueness => true;
    }
}