﻿using System.Runtime.CompilerServices;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        private const string MetaNameOfName        = "@name";
        private const string MetaNameOfAvatar      = "@avatar";
        private const string MetaNameOfAge         = "@age";
        private const string MetaNameOfBirth       = "@birth";
        private const string MetaNameOfWeight      = "@weight";
        private const string MetaNameOfHeight      = "@height";
        private const string MetaNameOfGender      = "@gender";
        private const string MetaNameOfIntro       = "@intro";
        private const string MetaNameOfCountry     = "@country";
        private const string MetaNameOfRace        = "@race";
        private const string MetaNameOfNickName    = "@nickname";
        private const string MetaNameOfIsDeath     = "@death";
        private const string MetaNameOfRarity      = "@rarity";
        private const string MetaNameOfPrice       = "@price";
        private const string MetaNameOfProduceArea = "@produce-area";
        private const string MetaNameOfWeakness    = "@Weakness";

        private string GetOrAddMetadata(string name)
        {
            var dict = BasicPart.Buckets;
            if (dict.TryGetValue(name, out var value))
            {
                return value;
            }

            if (_MetadataTrackerByName.TryGetValue(name, out var metadata))
            {
                if (!string.IsNullOrEmpty(metadata.Value))
                {
                    BasicPart.Buckets.TryAdd(name, metadata.Value);
                    return metadata.Value;
                }
            }

            value = string.Empty;
            dict.TryAdd(name, value);
            return value;
        }

        private string GetOrAddMetadata(string name, string defaultValue)
        {
            var dict = BasicPart.Buckets;
            if (dict.TryGetValue(name, out var value))
            {
                return value;
            }

            value = defaultValue;
            dict.TryAdd(name, value);
            return value;
        }

        private void UpsertMetadata(string name, string value, [CallerMemberName] string propName = "")
        {
            var dict = BasicPart.Buckets;
            if (dict.ContainsKey(name))
            {
                dict[name] = value;
            }
            else
            {
                dict.Add(name, value);
            }

            AddMetadata(new Metadata
            {
                Name  = name,
                Value = value,
                Type  = MetadataKind.Text,
            });
            
            SetDirtyState();
            RaiseUpdated(propName);
        }
        
        public string Rarity
        {
            get => GetOrAddMetadata(MetaNameOfRarity);
            set
            {
                UpsertMetadata(MetaNameOfRarity, value);
            }
        }
        
        public string Price
        {
            get => GetOrAddMetadata(MetaNameOfPrice);
            set
            {
                UpsertMetadata(MetaNameOfPrice, value);
            }
        }
        
        public string ProduceArea
        {
            get => GetOrAddMetadata(MetaNameOfProduceArea);
            set
            {
                UpsertMetadata(MetaNameOfProduceArea, value);
            }
        }
        

        public string NickName
        {
            get => GetOrAddMetadata(MetaNameOfNickName);
            set
            {
                UpsertMetadata(MetaNameOfNickName, value);
            }
        }


        public string Age
        {
            get => GetOrAddMetadata(MetaNameOfAge);
            set
            {
                UpsertMetadata(MetaNameOfAge, value);
            }
        }


        public string Race
        {
            get => GetOrAddMetadata(MetaNameOfRace);
            set
            {
                UpsertMetadata(MetaNameOfRace, value);
            }
        }

        public string Country
        {
            get => GetOrAddMetadata(MetaNameOfCountry);
            set
            {
                UpsertMetadata(MetaNameOfCountry, value);
            }
        }

        public string Weight
        {
            get => GetOrAddMetadata(MetaNameOfWeight);
            set
            {
                UpsertMetadata(MetaNameOfWeight, value);
            }
        }

        public bool IsDeath
        {
            get
            {
                var v = GetOrAddMetadata(MetaNameOfIsDeath);
                return bool.TryParse(v, out var n) && n;
            }
            set
            {
                UpsertMetadata(MetaNameOfIsDeath, value.ToString());
            }
        }
        
        public string Height
        {
            get => GetOrAddMetadata(MetaNameOfHeight);
            set
            {
                UpsertMetadata(MetaNameOfHeight, value);
            }
        }


        public string Birth
        {
            get => GetOrAddMetadata(MetaNameOfBirth);
            set
            {
                UpsertMetadata(MetaNameOfBirth, value);
            }
        }


        public string Gender
        {
            get => GetOrAddMetadata(MetaNameOfGender);
            set => UpsertMetadata(MetaNameOfGender, value);
        }
        
        public string Intro
        {
            get => GetOrAddMetadata(MetaNameOfIntro);
            set
            {
                UpsertMetadata(MetaNameOfIntro, value);
                Document.Intro = value;
                Cache.Intro    = value;
            }
        }

        public string Weakness
        {
            get => GetOrAddMetadata(MetaNameOfWeakness);
            set
            {
                UpsertMetadata(MetaNameOfWeakness, value);
                Document.Intro = value;
                Cache.Intro    = value;
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
            }
        }

        public PartOfBasic BasicPart { get; private set; }
    }
}