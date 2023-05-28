using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaDB.Core.Migrations
{
    public static class V209DatabaseUpdater
    {
        //
        // 升级任务
        public class RepositoryProperty
        {
            public string IndexId { get; set; }

            /// <summary>
            /// 获取或设置 <see cref="Avatar"/> 属性。
            /// </summary>
            public string Avatar { get; set; }

            /// <summary>
            /// 获取或设置 <see cref="Summary"/> 属性。
            /// </summary>
            public string Summary { get; set; }

            /// <summary>
            /// 获取或设置 <see cref="EnglishName"/> 属性。
            /// </summary>
            public string EnglishName { get; set; }

            public string Author { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public ObservableCollection<string> Backgrounds { get; set; }
        }


        public const string AvatarPattern            = "avatar_{0}.png";
        public const string ThumbnailWithSizePattern = "{0}_{1}_{2}";
        public const string ThumbnailPattern         = "thumb_{0}.png";

        public static string GetAvatarName() => string.Format(AvatarPattern, ID.Get());

        private static string TransformAvatar(string avatar, string srcDir, string dstDir)
        {
            // miga://v2.img/QQ图片20230118233550.jpg
            var param = avatar.Substring(14);
            var src   = Path.Combine(srcDir, param);
            var res   = GetAvatarName();
            var dst   = Path.Combine(dstDir, res);
            File.Copy(src, dst, true);
            return res;
        }

        public static async Task<EngineResult> Update(IDatabaseManager databaseManager, string databaseFilePath, string targetFilePath)
        {
            if (string.IsNullOrEmpty(databaseFilePath) ||
                string.IsNullOrEmpty(targetFilePath))
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            var mainDatabaseFileName = Path.Combine(databaseFilePath, "main.mgdb");
            var indexFileName        = Path.Combine(databaseFilePath, "index.migaidx");


            //
            // 完成检查
            if (!File.Exists(mainDatabaseFileName) ||
                !File.Exists(indexFileName))
            {
                return EngineResult.Failed(EngineFailedReason.InputSourceNotExists);
            }

            if (!Directory.Exists(targetFilePath))
            {
                return EngineResult.Failed(EngineFailedReason.ParameterDependencyEmptyOrNull);
            }

            //
            // 执行模组复制
            try
            {
                var property = JSON.FromFile<RepositoryProperty>(indexFileName) ?? new RepositoryProperty
                {
                    Author      = UntitledField,
                    EnglishName = UntitledField,
                    Email       = UntitledField,
                    Summary     = UntitledField,
                    Name        = UntitledField,
                    Backgrounds = new ObservableCollection<string>(),
                };

                var property2 = new DatabaseProperty
                {
                    Name        = property.Name,
                    Album       = new ObservableCollection<Album>(),
                    Author      = property.Author,
                    ForeignName = property.EnglishName,
                };

                var r = await databaseManager.CreateAsync(targetFilePath, property2);

                if (r.IsFinished)
                {
                    var src = new LiteDatabase(mainDatabaseFileName);
                    return await UpdateImpl(databaseManager, src, mainDatabaseFileName, targetFilePath);
                }
                else
                {
                }
            }
            catch (IOException)
            {
                return EngineResult.Failed(EngineFailedReason.InputSourceOccupied);
            }
            catch (Exception)
            {
                return EngineResult.Failed(EngineFailedReason.Unknown);
            }

            return EngineResult.Successful;
        }

        private static async Task<EngineResult> UpdateImpl(IDatabaseManager databaseManager, LiteDatabase source, string databaseFilePath, string targetFilePath)
        {
            //
            // Update All Image
            var imagePool = new Dictionary<string, string>();

            //
            // 迁移Document 和 DocumentIndex
            return EngineResult.Successful;
        }

        private static void MigrateDocument(
            DocumentEngine dstEngine, 
            ImageEngine imgEngine,
            LiteDatabase database)
        {
            var rawCaches = database.GetCollection("idx")
                                         .FindAll()
                                         .Select(GetCache);

            var rawDocuments = database.GetCollection("doc")
                                       .FindAll()
                                       .Select(GetDocument);

            rawCaches.ForEach(x =>
            {
                x.Avatar = null;
                dstEngine.AddDocumentCache(x);
            });
            
            rawDocuments.ForEach(x =>
            {
                dstEngine.AddDocument(x);
            });
        }

        private static DocumentCache GetCache(BsonDocument document)
        {
            var cache = new DocumentCache
            {
                Type           = document.DocumentType(),
                Avatar         = document.String(AvatarField),
                Name           = document[NameField],
                Removable      = false,
                IsLocked       = document.Bool(IsLockingField),
                IsDeleted      = document.Bool(IsDeleteField),
                Intro          = document.String(SummaryField),
                TimeOfCreated  = document.Time(CreatedDateTimeField),
                TimeOfModified = document.Time(ModifiedDateTimeField),
                Id             = document.String(IDField),
            };

            return cache;
        }
        
        private static Document GetDocument(BsonDocument document)
        {
            var cache = new Document
            {
                Type      = document.DocumentType(),
                Avatar    = document.String(AvatarField),
                Name      = document[NameField],
                Removable = false,
                Version   = 0,
                Parts     = new DataPartCollection(),
                Metas     = new MetadataCollection(),
                Intro     = document.String(SummaryField),
                Id        = document.String(IDField),
            };

            cache.Parts.AddMany(GetDataParts(document));
            cache.Metas.AddMany(GetMetadatas(document));

            return cache;
        }

        private static IEnumerable<Metadata> GetMetadatas(BsonDocument document)
        {
            var sub = document.TryGetValue(MetasField, out var part) ? part.AsArray : new BsonArray();
            var metas = sub.Select(x => x.AsDocument).Select(x => new Metadata
            {
                Name  = x.String(NameField),
                Value = x.String(ValueField)
            });
            return metas;
        }
        
        private static IEnumerable<DataPart> GetDataParts(BsonDocument document)
        {
            var sub = document.TryGetValue(PartsField, out var part) ? part.AsArray : new BsonArray();
            var metas = sub.Select(x => x.AsDocument).Select(x => new Metadata
            {
                Name  = x.String(NameField),
                Value = x.String(ValueField)
            });
            return null;
        }

        #region Value Help

        

        internal static Interfaces.DocumentType DocumentType(this BsonDocument document)
        {
            var value = document.TryGetValue(DocumentTypeField, out var v) ? v.AsInt32 : 0;

            /*
             *  Character,
        Skills,
        Materials,
        Map,
        Assets,
        Custom,
             */
            return value switch
            {
                1 => Interfaces.DocumentType.Skill,
                2 => Interfaces.DocumentType.Material,
                3 => Interfaces.DocumentType.Geography,
                4 => Interfaces.DocumentType.Item,
                5 => Interfaces.DocumentType.Other,
                _ => Interfaces.DocumentType.Character
            };
        }


        internal static string String(this BsonDocument document, string key)
        {
            return document.TryGetValue(key, out var v) ? v : UntitledField;
        }

        internal static DateTime Time(this BsonDocument document, string key)
        {
            return document.TryGetValue(key, out var v) ? v.AsDateTime : DateTime.Now;
        }

        internal static bool Bool(this BsonDocument document, string key)
        {
            return document.TryGetValue(key, out var v) && v.AsBoolean;
        }

        #endregion
        
        public const string NameField             = "Name";
        public const string UntitledField         = "Untitiled";
        public const string AuthorField           = "Author";
        public const string IsDeleteField         = "IsDelete";
        public const string IsLockingField        = "IsLocking";
        public const string CreatedDateTimeField  = "CreatedDateTime";
        public const string ModifiedDateTimeField = "ModifiedDateTime";
        public const string AvatarField           = "Avatar";
        public const string DocumentTypeField     = "DocumentType";
        public const string IDField               = "_id";
        public const string SummaryField          = "Summary";
        public const string MetasField            = "Metas";
        public const string PartsField            = "Parts";
        public const string ValueField            = "Value";
    }
}