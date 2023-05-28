using System.ComponentModel;
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Acorisoft.Miga.Doc.Documents
{
    public class DocumentManager : IMetadataManager
    {
        private readonly Dictionary<Type, DataPart>       _dataPartTypeTracker;
        private readonly Dictionary<Guid, CustomDataPart> _dataPartIDTracker;
        private readonly Dictionary<string, MetaItem>     _metaItemNameTracker;
        private readonly Dictionary<string, Metadata>     _metadatas;
        private readonly Dictionary<string, int>          _metadataIndexTracker;
        private readonly List<MetaItem>                   _items;
        private readonly SourceList<DataPart>             _parts;
        private readonly ModuleService                   _modules;
        private readonly DataPartReader                   _reader;
        private readonly PropertyEventPublisher           _publisher;

        private Document _document;

        public DocumentManager(ModuleService moduleCollection)
        {
            _modules        = moduleCollection;
            _dataPartTypeTracker  = new Dictionary<Type, DataPart>(7);
            _dataPartIDTracker    = new Dictionary<Guid, CustomDataPart>(7);
            _metaItemNameTracker  = new Dictionary<string, MetaItem>(7, StringComparer.OrdinalIgnoreCase);
            _metadataIndexTracker = new Dictionary<string, int>(13, StringComparer.OrdinalIgnoreCase);
            _metadatas      = new Dictionary<string, Metadata>(13, StringComparer.OrdinalIgnoreCase);
            _items          = new List<MetaItem>(32);
            _parts          = new SourceList<DataPart>();
            _reader         = moduleCollection.Reader;
            _publisher      = new PropertyEventPublisher();
        }

        public IDisposable Subscribe(IPropertyEventSubscriber subscriber)
        {
            var disposable = subscriber.Subscribe(_publisher);
            UpdateAll();
            return disposable;
        }
        

        #region Manage / Unmanaged

        public void Unmanaged()
        {
            ClearPart();

            _document = null;
            
            
            
            _dataPartTypeTracker.Clear();
            _dataPartIDTracker.Clear();
            _metadataIndexTracker.Clear();
            _metadatas.Clear();
            _parts.Clear();

            UpdateAll();
        }

        public DocumentIndex GetIndex()
        {
            // ReSharper disable once InvertIf
            if (!_dataPartTypeTracker.TryGetValue(typeof(Avatar), out var avatar))
            {
                avatar = new Avatar();
                _dataPartTypeTracker.TryAdd(avatar.GetType(), avatar);
                _document.Parts.Add(avatar);
            }

            return new DocumentIndex
            {
                Id     = _document.Id,
                Name   = _document.Name,
                DocumentType   = _document.Type,
                Avatar = ((Avatar)avatar).Value
            };
        }

        private void ApplyMetadata(Document document)
        {
            var index = 0;

            //
            // 重新应用数据
            foreach (var metadata in document.Metas)
            {
                if (metadata is null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(metadata.Name))
                {
                    continue;
                }

                if (_metadatas.TryAdd(metadata.Name, metadata))
                {
                    _metadataIndexTracker.TryAdd(metadata.Name, index);
                    index++;
                }

                if (!_metaItemNameTracker.TryGetValue(metadata.Name, out var item))
                {
                    continue;
                }

                item.Update(metadata);
                _publisher.Publish(new PropertyChangedEventArgs(item.PropertyName));
            }
        }

        private void ApplyDataParts(Document document)
        {
            //
            // 添加模组
            foreach (var part in document.Parts.Where(part => part is not null))
            {
                if (part is CustomDataPart cdp)
                {
                    if (_dataPartIDTracker.TryAdd(cdp.Id, cdp))
                    {
                        //
                        // Tracking Property Changed
                        foreach (var property in cdp.Properties)
                        {
                            property.PropertyChanged += HookingPropertyChanged;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    _dataPartTypeTracker.TryAdd(part.GetType(), part);
                }

                part.Initialized(this);
                _parts.Add(part);
            }
        }

        public void Manage(Document document)
        {
            if (document is null)
            {
                return;
            }

            _document = document;
            
            ApplyMetadata(document);
            ApplyDataParts(document);
        }

        #endregion

        #region AddPart / RemovePart / ClearPart

        private async Task<CustomDataPart> ReadFromAsync(ModuleIndex index)
        {
            var realFileName = Path.Combine(_modules.Directory, index.FileName);
            var compilation = await _reader.ReadFromAsync(realFileName);
            var result = compilation.Result;
            return result.Active();
        }

        private static void UpdateAndMigratingData(CustomDataPart oldOne, CustomDataPart newOne)
        {
            foreach (var oldProp in oldOne.Properties)
            {
                foreach (var newProp in newOne.Properties
                             .Where(newProp => string.Equals(oldProp.Metadata, newProp.Metadata,
                                 StringComparison.OrdinalIgnoreCase)))
                {
                    newProp.Value = oldProp.Value;
                }
            }
        }

        private void TrackingPropertyChanged(CustomDataPart part)
        {
            //
            // 添加部件
            _dataPartIDTracker.TryAdd(part.Id, part);
            _parts.Add(part);
            _document.Parts.Add(part);
            
            //
            // 追踪部件变化
            foreach (var property in part.Properties)
            {
                if (property is GroupProperty propertyGroup)
                {
                    foreach (var op in propertyGroup.Values)
                    {
                        op.PropertyChanged += HookingPropertyChanged;
                    }
                }
                else
                {
                    property.PropertyChanged += HookingPropertyChanged;
                }
            }
            
            part.Initialized(this);
        }

        private bool UpdateDataPart(DataPart inside, CustomDataPart part)
        {
            // if (inside.Name == part.Name &&
            //     inside.Version >= part.Version)
            // {
            //     return false;
            // }
                
            if (inside.Version >= part.Version)
            {
                return false;
            }

            // part.Id = Guid.NewGuid();
            var type = part.GetType();
            var oldOne = _dataPartTypeTracker[type];
            var insideIndex = _document.Parts.IndexOf(oldOne);
            _document.Parts.Remove(oldOne);
            _document.Parts.Insert(insideIndex, part);

            //
            // replace
            _dataPartTypeTracker[type]  = part;
            _dataPartIDTracker[part.Id] = part;
            _parts.Replace(oldOne, part);

            UpdateAndMigratingData((CustomDataPart)oldOne, part);
            return true;
        }

        public async Task<IsCompleted> AddPart(ModuleIndex index)
        {
            try
            {
                var part = await ReadFromAsync(index);
                part.Priority = 1;

                if (!AddPartCore(part))
                {
                    return new IsCompleted
                    {
                        IsFinished = false,
                        Message    = DocSR.ErrMsg_DuplicatedDataPart
                    };
                }

                return new IsCompleted
                {
                    IsFinished = true,
                };
            }
            catch (Exception ex)
            {
                return new IsCompleted
                {
                    IsFinished = false,
                    Message    = ex.Message
                };
            }
        }

        public IsCompleted AddPart(CustomDataPart part)
        {
            try
            {
                if (!AddPartCore(part))
                {
                    return new IsCompleted
                    {
                        IsFinished = false,
                        Message    = DocSR.ErrMsg_DuplicatedDataPart
                    };
                }

                return new IsCompleted
                {
                    IsFinished = true,
                };
            }
            catch (Exception ex)
            {
                return new IsCompleted
                {
                    IsFinished = false,
                    Message    = ex.Message
                };
            }
        }

        private bool AddPartCore(CustomDataPart part)
        {
            //
            // 检查是否部件重复
            // ReSharper disable once InvertIf
            if (_dataPartIDTracker.TryGetValue(part.Id, out var inside))
            {
                if (!UpdateDataPart(inside, part)) 
                    return false;
            }
            
            
            TrackingPropertyChanged(part);
            return true;
        }

        public void ClearPart()
        {
            if (_document is null)
            {
                return;
            }

            foreach (var part in _document.Parts)
            {
                RemovePart(part);
            }
        }

        public void RemovePart(DataPart part)
        {
            if (part is not CustomDataPart cdp)
            {
                return;
            }

            foreach (var property in cdp.Properties)
            {
                if (property is GroupProperty propertyGroup)
                {
                    foreach (var op in propertyGroup.Values)
                    {
                        op.PropertyChanged -= HookingPropertyChanged;
                    }
                }
                else
                {
                    property.PropertyChanged -= HookingPropertyChanged;
                }


                if (string.IsNullOrEmpty(property.Metadata) ||
                    !_metaItemNameTracker.TryGetValue(property.Metadata, out var mapping))
                {
                    continue;
                }
                
                mapping.Update(null);
                _publisher.Publish(new PropertyChangedEventArgs(mapping.PropertyName));
            }


            _parts.Remove(cdp);
            _dataPartIDTracker.Remove(cdp.Id);
        }

        public T GetPart<T>() where T : DataPart
        {
            // ReSharper disable once InvertIf
            if (!_dataPartTypeTracker.TryGetValue(typeof(T), out var dataPart))
            {
                dataPart = Classes.CreateInstance<T>();
                _dataPartTypeTracker.TryAdd(typeof(T), dataPart);
                _document.Parts.Add(dataPart);
            }

            return (T)dataPart;
        }

        #endregion

        #region GetItem

        public StringMetaItem GetItem(string title, string color, string metaName)
        {
            var item = new StringMetaItem
            {
                MainTitle = title,
                Color     = color,
                Metadata  = metaName
            };
            _metaItemNameTracker.TryAdd(metaName, item);
            _items.Add(item);
            UpdateAll();
            return item;
        }

        public MetaItem<T> GetItem<T>(string metaName, string propertyName)
        {
            if (!_metaItemNameTracker.TryGetValue(metaName, out var item))
            {
                item = new MetaItem<T>(propertyName);
                _metaItemNameTracker.Add(metaName, item);
                _items.Add(item);
            }

            return (MetaItem<T>)item;
        }

        public MetaItem<string> GetSpeciesItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Species, propertyName);
        }

        public MetaItem<string> GetNameItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Name, propertyName);
        }

        public MetaItem<string> GetRarityItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Rarity, propertyName);
        }

        public MetaItem<string> GetElementalItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Elemental, propertyName);
        }

        public MetaItem<string> GetSexItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Sex, propertyName);
        }

        public MetaItem<string> GetAgeItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Age, propertyName);
        }

        public MetaItem<string> GetWeightItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Weight, propertyName);
        }

        public MetaItem<string> GetHeightItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Height, propertyName);
        }

        public MetaItem<string> GetBirthItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Birth, propertyName);
        }

        public MetaItem<string> GetCountryItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Country, propertyName);
        }

        public MetaItem<string> GetBloodTypeItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.BloodType, propertyName);
        }


        public MetaItem<string> GetAvatarItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Avatar, propertyName);
        }

        public MetaItem<string> GetIdentityItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Identity, propertyName);
        }

        public MetaItem<string> GetSloganItem(string propertyName)
        {
            return GetItem<string>(KnownMetadataNames.Slogan, propertyName);
        }

        public IEnumerable<MetaItem> GetItems => _items;

        #endregion

        #region Hooking

        private void HookingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = (InputProperty)sender;

            if (string.IsNullOrEmpty(property.Value) && property is IFallbackSupport fallback)
            {
                property.Value = fallback.Fallback;
            }

            if (string.IsNullOrEmpty(property.Metadata))
            {
                return;
            }

            var metadata = new Metadata
            {
                Name  = property.Metadata,
                Value = property.Value
            };

            if (metadata.Name == KnownMetadataNames.Name)
            {
                if (EnableNameIntercept && !string.IsNullOrEmpty(InterceptName))
                {
                    property.SetValue(InterceptName);
                    metadata.Value = InterceptName;
                }

                _document.Name = metadata.Value;
            }

            Set(metadata);
        }


        private void OnMetadataAddOrUpdate(Metadata metadata)
        {
            if (!_metaItemNameTracker.TryGetValue(metadata.Name, out var metaItem))
            {
                return;
            }

            metaItem.Update(metadata);

            //
            // 更新
            _publisher.Publish(new PropertyChangedEventArgs(metaItem.PropertyName));
        }

        private void OnMetadataAddOrUpdate(string metaName)
        {
            if (!_metaItemNameTracker.TryGetValue(metaName, out var metaItem))
            {
                return;
            }

            metaItem.Update(null);

            //
            // 更新
            _publisher.Publish(new PropertyChangedEventArgs(metaItem.PropertyName));
        }

        public void Set(Metadata metadata)
        {
            if (metadata is null)
            {
                return;
            }

            if (_metadatas.ContainsKey(metadata.Name))
            {
                _metadatas[metadata.Name] = metadata;
            }
            else
            {
                _metadatas.Add(metadata.Name, metadata);
            }

            if (_metadataIndexTracker.TryGetValue(metadata.Name, out var index))
            {
                _document.Metas[index].Value = metadata.Value;
            }
            else
            {
                index = _document.Metas.Count;
                _document.Metas.Add(metadata);
                _metadataIndexTracker.TryAdd(metadata.Name, index);
            }

            OnMetadataAddOrUpdate(metadata);
        }

        public void Unset(string metaName)
        {
            if (_metadatas.Remove(metaName))
            {
                if (_metadataIndexTracker.TryGetValue(metaName, out var index))
                {
                    _document.Metas.RemoveAt(index);
                    _metadataIndexTracker.Remove(metaName);
                }
            }

            OnMetadataAddOrUpdate(metaName);
        }

        #endregion

        public void Update(Metadata metadata)
        {
            if (metadata is null)
            {
                return;
            }

            OnMetadataAddOrUpdate(metadata);
        }

        public void UpdateAll()
        {
            //
            // 更新所有属性
            foreach (var key in _metadatas.Keys)
            {
                if (_metaItemNameTracker.TryGetValue(key, out var item))
                {
                    item.Update(_metadatas[key]);
                    _publisher.Publish(new PropertyChangedEventArgs(item.PropertyName));
                }
            }
        }
        
        public void UpdateAvatar()
        {
            if (_metaItemNameTracker.TryGetValue(KnownMetadataNames.Avatar, out var item) &&
                _metadatas.TryGetValue(KnownMetadataNames.Avatar, out var data))
            {
                item.Update(data);
                _publisher.Publish(new PropertyChangedEventArgs(item.PropertyName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IPropertyEventPublisher Publisher => _publisher;

        /// <summary>
        /// 
        /// </summary>
        public bool EnableNameIntercept { get; set; }

        /// <summary>
        /// 获取或设置需要拦截的名字
        /// </summary>
        public string InterceptName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SourceList<DataPart> DataParts => _parts;
    }
}