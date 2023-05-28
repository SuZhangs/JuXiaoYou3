
using System.Diagnostics.CodeAnalysis;
using LabelDictionary = System.Collections.Generic.Dictionary<System.String, Acorisoft.Miga.Doc.Labels.Label>;
using LabelCollection = LiteDB.ILiteCollection<Acorisoft.Miga.Doc.Labels.Label>;
using DirectoryDatabase = LiteDB.ILiteCollection<Acorisoft.Miga.Doc.Labels.VirtualDirectory>;
using DirectoryCollection = DynamicData.SourceList<Acorisoft.Miga.Doc.Labels.VirtualDirectory>;

namespace Acorisoft.Miga.Doc.Engines
{
    [Obsolete]
    public class LabelService : StorageService, IVirtualDirectoryService<Label, LabelMapping, VirtualDirectory>
    {

        public LabelService()
        {
            ContextualDirectoryCollection = new SourceCache<Label, string>(x => x.Id);
        }

        #region Labels

        
        
        private class ObjectLabelCollection : IObjectLabelCollection<Label>
        {
            private readonly LabelDictionary            _duplicated;
            private readonly LabelCollection            _database;
            private readonly SourceCache<Label, string> _collection;
            private const    string                     CollectionName = "labels";

            public ObjectLabelCollection(RepositoryContext context)
            {
                _database = context.Database.GetCollection<Label>(CollectionName);
                _collection     = new SourceCache<Label, string>(x => x.Id);
                _duplicated = new LabelDictionary(13);
            }

            public void ResolveConflict()
            {
                foreach (var label in _database.FindAll())
                {
                    if (_duplicated.TryAdd(label.Name, label))
                    {
                        _collection.AddOrUpdate(label);
                    }
                }
            }

            public bool HasLabel(string labelName) => _duplicated.ContainsKey(labelName);

            public bool GetLabel(string labelName, out Label label)
            {
                return _duplicated.TryGetValue(labelName, out label);
            }

            public IEnumerable<Label> GetLabels()
            {
                return _database.FindAll();
            }

            public void AddLabel(Label label,[AllowNull] Label parent = null)
            {
                if (label is null)
                {
                    return;
                }
                if (parent is not null)
                {
                    label.Owner = parent.Id;
                }

                _duplicated.TryAdd(label.Name, label);
                _collection.AddOrUpdate(label);
                _database.Upsert(label);
            }

            public void RemoveLabel(Label label)
            {
                if (!_duplicated.Remove(label.Name))
                {
                    return;
                }
                _collection.Remove(label);
                _database.Delete(label.Id);
            }

            public SourceCache<Label, string> Collection => _collection;
        }
        
        public bool Add(Label label,[AllowNull] Label parent = null)
        {
            if (Labels.HasLabel(label.Name))
            {
                return false;
            }
            
            Labels.AddLabel(label, parent);
            return true;
        }

        public void Remove(Label label)
        {
            if (label is null)
            {
                return;
            }
            
            Labels.RemoveLabel(label);
        }

        #endregion
        
        #region Directory
        
        private class VirtualDirectoryCollection : IVirtualDirectoryCollection<VirtualDirectory>
        {
            private readonly DirectoryDatabase   _database;
            private readonly DirectoryCollection _collection;
            private readonly HashSet<string>     _duplicated;
            private const    string              CollectionName = "vDir";
            
            public VirtualDirectoryCollection(RepositoryContext context)
            {
                _duplicated = new HashSet<string>(13);
                _collection = new DirectoryCollection();
                _database   = context.Database.GetCollection<VirtualDirectory>(CollectionName);
            }
            
            public void ResolveConflict()
            {
                foreach (var dir in _database.FindAll())
                {
                    if (_duplicated.Add(dir.Name) && _duplicated.Add(dir.BaseOn))
                    {
                        _collection.Add(dir);
                    }
                }
            }
            
            public bool AddDirectory(string directoryName, IObjectLabel target)
            {
                if (target is null)
                {
                    return false;
                }

                if (!_duplicated.Add(directoryName) ||
                    !_duplicated.Add(target.Name))
                {
                    return false;
                }

                var dir = new VirtualDirectory
                {
                    BaseOn = target.Id,
                    Id     = ShortGuidString.GetId(),
                    Name   = directoryName
                };

                _database.Upsert(dir);
                _collection.Add(dir);
                return true;
            }

            public void RemoveDirectory(VirtualDirectory directory)
            {
                if (directory is null)
                {
                    return;
                }

                if (!_duplicated.Remove(directory.Name) || !_duplicated.Remove(directory.BaseOn))
                {
                    return;
                }
                _database.Delete(directory.Id);
                _collection.Remove(directory);
            }

            public DirectoryCollection Collection => _collection;
        }

        public bool Add(string directoryName, Label target)
        {
            return InnerDirectories.AddDirectory(directoryName, target);
        }
        public void Remove(VirtualDirectory directory) => InnerDirectories.RemoveDirectory(directory);
        
        
        #endregion

        #region Mappings

        
        private class ObjectMappingCollection : IObjectMappingCollection<LabelMapping>
        {
            private readonly ILiteCollection<LabelMapping> _collection;
            private const    string                        CollectionName = "labelMap";
            
            public ObjectMappingCollection(RepositoryContext context)
            {
                _collection = context.Database.GetCollection<LabelMapping>(CollectionName);
            }

            public void AddMapping(LabelMapping mapping)
            {
                _collection.Upsert(mapping);
            }

            public void RemoveMapping(IObjectLabel label, string targetId)
            {
                var target = _collection.FindOne(
                    Query.And(
                        Query.EQ(nameof(LabelMapping.LabelId), label.Id),
                        Query.EQ(nameof(LabelMapping.TargetId), targetId)));

                if (target is null)
                {
                    return;
                }

                _collection.Delete(target.Id);
            }

            
            public IEnumerable<IObjectMapping> GetMappings(IObjectLabel label)
            {
                return _collection.Find(Query.EQ(nameof(LabelMapping.LabelId), label!.Id)).ToArray();
            }
        }


        public void Add(string labelName, string id, bool isDoc)
        {
            if (!Labels.GetLabel(labelName, out var label))
            {
                return;
            }

            Mappings.AddMapping(new LabelMapping
            {
                Id         = ShortGuidString.GetId(),
                IsDocument = isDoc,
                LabelId    = label.Id,
                TargetId   = id
            });
        }

        public void Remove(string labelName, string id)
        {
            if (!Labels.GetLabel(labelName, out var label))
            {
                return;
            }
            
            Mappings.RemoveMapping(label, id);
        }

        #endregion
        
        public IEnumerable<IObjectMapping> GetFiles(string labelName)
        {
            return !Labels.GetLabel(labelName, out var output) ? null : Mappings.GetMappings(output);
        }

        public IEnumerable<IObjectMapping> GetFiles(IObjectLabel label)
        {
            return !Labels.GetLabel(label.Name, out var output) ? null : Mappings.GetMappings(output);
        }

        public IEnumerable<Label> GetLabels() => InnerLabels.GetLabels();

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            InnerLabels      = new ObjectLabelCollection(context);
            InnerDirectories = new VirtualDirectoryCollection(context);
            InnerMappings    = new ObjectMappingCollection(context);


            Labels.ResolveConflict();
            Directories.ResolveConflict();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            InnerLabels      = null;
            InnerMappings    = null;
            InnerDirectories = null;
        }
        
        // refactor:
        private ObjectLabelCollection InnerLabels { get; set; }
        private ObjectMappingCollection InnerMappings { get; set; }
        private VirtualDirectoryCollection InnerDirectories { get; set; }
        
        public IObjectLabelCollection<Label> Labels => InnerLabels;
        public IObjectMappingCollection<LabelMapping> Mappings => InnerMappings;
        public IVirtualDirectoryCollection<VirtualDirectory> Directories => InnerDirectories;
        
        public SourceCache<Label, string> LabelCollection => InnerLabels.Collection;
        public SourceCache<Label, string> ContextualDirectoryCollection { get; }
        public SourceList<VirtualDirectory> DirectoryCollection => InnerDirectories.Collection;

    }
}