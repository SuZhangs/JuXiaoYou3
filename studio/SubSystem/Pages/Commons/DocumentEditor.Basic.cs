using Acorisoft.FutureGL.MigaDB.Data.Metadatas;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private const string MetaNameOfName     = "@name";
        private const string MetaNameOfAvatar   = "@avatar";
        private const string MetaNameOfAge      = "@age";
        private const string MetaNameOfBirth    = "@birth";
        private const string MetaNameOfWeight   = "@weight";
        private const string MetaNameOfHeight   = "@height";
        private const string MetaNameOfGender   = "@gender";
        private const string MetaNameOfIntro    = "@intro";
        private const string MetaNameOfCountry  = "@country";
        private const string MetaNameOfRace     = "@race";
        private const string MetaNameOfNickName = "@nickname";

        private string GetOrAddMetadata(string name)
        {
            if (BasicPart.Buckets.TryGetValue(name, out var value))
            {
                return value;
            }

            value = string.Empty;
            BasicPart.Buckets.TryAdd(name, value);
            return value;
        }

        private string GetOrAddMetadata(string name, string defaultValue)
        {
            if (BasicPart.Buckets.TryGetValue(name, out var value))
            {
                return value;
            }

            value = defaultValue;
            BasicPart.Buckets.TryAdd(name, value);
            return value;
        }

        private void UpsertMetadata(string name, string value)
        {
            if (BasicPart.Buckets.ContainsKey(name))
            {
                BasicPart.Buckets[name] = value;
            }
            else
            {
                BasicPart.Buckets.Add(name, value);
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
                SetDirtyState();
                RaiseUpdated();
            }
        }


        public string Age
        {
            get => GetOrAddMetadata(MetaNameOfAge);
            set
            {
                UpsertMetadata(MetaNameOfAge, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }


        public string Race
        {
            get => GetOrAddMetadata(MetaNameOfRace);
            set
            {
                UpsertMetadata(MetaNameOfRace, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }

        public string Country
        {
            get => GetOrAddMetadata(MetaNameOfCountry);
            set
            {
                UpsertMetadata(MetaNameOfCountry, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }

        public string Weight
        {
            get => GetOrAddMetadata(MetaNameOfWeight);
            set
            {
                UpsertMetadata(MetaNameOfWeight, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }

        public string Height
        {
            get => GetOrAddMetadata(MetaNameOfHeight);
            set
            {
                UpsertMetadata(MetaNameOfHeight, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }


        public string Birth
        {
            get => GetOrAddMetadata(MetaNameOfBirth);
            set
            {
                UpsertMetadata(MetaNameOfBirth, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }


        public string Gender
        {
            get => GetOrAddMetadata(MetaNameOfGender);
            set
            {
                UpsertMetadata(MetaNameOfGender, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }

        public string Intro
        {
            get => GetOrAddMetadata(MetaNameOfIntro);
            set
            {
                UpsertMetadata(MetaNameOfIntro, value);
                Document.Intro = value;
                Cache.Intro    = value;
                SetDirtyState();
                RaiseUpdated();
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

                Document.Avatar = value;
                Cache.Avatar    = value;
                UpsertMetadata(MetaNameOfAvatar, value);
                SetDirtyState();
                RaiseUpdated();
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

                Document.Name = value;
                Cache.Name    = value;
                UpsertMetadata(MetaNameOfName, value);
                SetDirtyState();
                RaiseUpdated();
            }
        }

        public PartOfBasic BasicPart { get; private set; }
    }
}