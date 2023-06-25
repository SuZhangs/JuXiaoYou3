using System.IO;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Utils;
using LiteDB;
using OldRepositoryProperty = Acorisoft.FutureGL.MigaStudio.Resources.Updaters.RepositoryProperty;
using NewRepositoryProperty = Acorisoft.FutureGL.MigaDB.Models.DatabaseProperty;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Updaters
{
    public static partial class V209DatabaseUpdater
    {
        public static async Task<EngineResult> Update(IDatabaseManager databaseManager, string databaseFilePath, string targetFilePath)
        {
            using (var session = Xaml.Get<IBusyService>()
                                     .CreateSession())
            {
                //
                //
                await session.Await(SR.Updating);


                var exists = await IsFileExistence(session, databaseFilePath, targetFilePath);
                if (!exists.IsFinished)
                {
                    return exists;
                }
                
                var mainDatabaseFileName = Path.Combine(databaseFilePath, "main.mgdb");
                var indexFileName        = Path.Combine(databaseFilePath, "index.migaidx");

                await session.Await(SR.Updating);
                var oldProperty = FromFile<OldRepositoryProperty>(indexFileName) ??
                                  new OldRepositoryProperty
                                  {
                                      Author      = UntitledField,
                                      EnglishName = UntitledField,
                                      Email       = UntitledField,
                                      Summary     = UntitledField,
                                      Name        = UntitledField,
                                      Backgrounds = new ObservableCollection<string>(),
                                  };

                var newProperty = Mapping(oldProperty);
                var r           = await databaseManager.CreateAsync(targetFilePath, newProperty);

                if (r.IsFinished)
                {
                    var src = new LiteDatabase(mainDatabaseFileName);
                    return await Updating(session, databaseManager, src);
                }

                return EngineResult.Failed(EngineFailedReason.InputDataError);
            }
        }
        
        private static async Task<EngineResult> IsFileExistence(IBusySession session, string databaseFilePath, string targetFilePath)
        {
            if (string.IsNullOrEmpty(databaseFilePath) ||
                string.IsNullOrEmpty(targetFilePath))
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            await session.Await("text.Updating.FileExistence");
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

            return EngineResult.Successful;
        }
        
        private static async Task<EngineResult> Updating(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB)
        {
            try
            {
                // 迁移文档
                await MigratingDocument();
                
                // 迁移文章
                await MigratingCompose();
                
                // 迁移标签
                await MigratingKeyword();
                
                // 迁移人物关系
                await MigratingRelatijve();
                
                // 迁移
                await MigratingKeyword();
                   
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

        private static async Task MigratingKeyword()
        {
            throw new NotImplementedException();
        }

        private static async Task MigratingRelatijve()
        {
            throw new NotImplementedException();
        }
        private static async Task MigratingDocument()
        {
            
        }
        
        private static async Task MigratingCompose()
        {
            
        }

        #region Value Help

        internal static MigaDB.Interfaces.DocumentType DocumentType(this BsonDocument document)
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
                1 => MigaDB.Interfaces.DocumentType.Skill,
                2 => MigaDB.Interfaces.DocumentType.Material,
                3 => MigaDB.Interfaces.DocumentType.Geography,
                4 => MigaDB.Interfaces.DocumentType.Item,
                5 => MigaDB.Interfaces.DocumentType.Other,
                _ => MigaDB.Interfaces.DocumentType.Character
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