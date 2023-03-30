using Acorisoft.FutureGL.MigaDB.Data.Metadatas;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        private const string MetaNameOfName     = "@name";
        private const string MetaNameOfAvatar   = "@avatar";
        private const string MetaNameOfAge      = "@age";
        private const string MetaNameOfBirth    = "@birth";
        private const string MetaNameOfWeight   = "@weight";
        private const string MetaNameOfHeight   = "@height";
        private const string MetaNameOfGender   = "@gender";
        private const string MetaNameOfCountry  = "@country";
        private const string MetaNameOfRace     = "@race";
        private const string MetaNameOfNickName = "@nickname";

        private string GetOrAddMetadata(string name)
        {
            if (_basicPart.Buckets.TryGetValue(name, out var value))
            {
                return value;
            }

            value = string.Empty;
            _basicPart.Buckets.TryAdd(name, value);
            return value;
        }

        private string GetOrAddMetadata(string name, string defaultValue)
        {
            if (_basicPart.Buckets.TryGetValue(name, out var value))
            {
                return value;
            }

            value = defaultValue;
            _basicPart.Buckets.TryAdd(name, value);
            return value;
        }

        private void UpsertMetadata(string name, string value)
        {
            if (_basicPart.Buckets.ContainsKey(name))
            {
                _basicPart.Buckets[name] = value;
            }
            else
            {
                _basicPart.Buckets.Add(name, value);
            }

            AddMetadata(new Metadata
            {
                Name  = name,
                Value = value,
                Type  = MetadataKind.Text,
            });
        }


        public string NickName
        {
            get => GetOrAddMetadata(MetaNameOfNickName);
            set
            {
                UpsertMetadata(MetaNameOfNickName, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }


        public string Age
        {
            get => GetOrAddMetadata(MetaNameOfAge);
            set
            {
                UpsertMetadata(MetaNameOfAge, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }


        public string Race
        {
            get => GetOrAddMetadata(MetaNameOfRace);
            set
            {
                UpsertMetadata(MetaNameOfRace, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }

        public string Country
        {
            get => GetOrAddMetadata(MetaNameOfCountry);
            set
            {
                UpsertMetadata(MetaNameOfCountry, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }

        public string Weight
        {
            get => GetOrAddMetadata(MetaNameOfWeight);
            set
            {
                UpsertMetadata(MetaNameOfWeight, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }

        public string Height
        {
            get => GetOrAddMetadata(MetaNameOfHeight);
            set
            {
                UpsertMetadata(MetaNameOfHeight, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }


        public string Birth
        {
            get => GetOrAddMetadata(MetaNameOfBirth);
            set
            {
                UpsertMetadata(MetaNameOfBirth, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }


        public string Gender
        {
            get => GetOrAddMetadata(MetaNameOfGender, Language.GetText("global.DefaultGender"));
            set
            {
                UpsertMetadata(MetaNameOfGender, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }


        public string Avatar
        {
            get => GetOrAddMetadata(MetaNameOfAvatar);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                _document.Avatar = value;
                _cache.Avatar    = value;
                UpsertMetadata(MetaNameOfAvatar, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }

        public string Name
        {
            get => GetOrAddMetadata(MetaNameOfName, Language.GetText("global.DefaultName"));
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                _document.Name = value;
                _cache.Name    = value;
                UpsertMetadata(MetaNameOfName, value);
                SetDirtyState(true);
                RaiseUpdating();
            }
        }
    }
}