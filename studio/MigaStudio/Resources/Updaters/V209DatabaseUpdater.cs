using System.IO;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.Miga.Doc.Documents;
using Acorisoft.Miga.Doc.Parts;
using LiteDB;
using OldDocument = Acorisoft.Miga.Doc.Documents.Document;
using NewDocument = Acorisoft.FutureGL.MigaDB.Documents.Document;
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
                    return await Updating(session, databaseManager, src, databaseFilePath);
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
        
        private static async Task<EngineResult> Updating(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB, string databaseFilePath)
        {
            try
            {
                // 
                await MigratingModules(session, databaseManager, oldDB);
                    
                // 迁移文档
                await MigratingDocuments(session, databaseManager, oldDB, databaseFilePath);
                
                // 迁移文章
                await MigratingComposes();
                
                // 迁移标签
                await MigratingKeywords();
                
                // 迁移人物关系
                await MigratingRelatives();
                
                // 迁移
                await MigratingKeywords();
                   
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

        private static async Task MigratingKeywords()
        {
            throw new NotImplementedException();
        }

        private static async Task MigratingRelatives()
        {
            throw new NotImplementedException();
        }
        
        private static async Task MigratingDocuments(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB, string databaseFilePath)
        {
            await session.Await(Language.GetText("text.Updating.DocumentUpdating"));
            
            //
            // 
            var imageEngine          = databaseManager.GetEngine<ImageEngine>();
            var documentEngine       = databaseManager.GetEngine<DocumentEngine>();
            var documentService      = oldDB.GetCollection<OldDocument>(Miga.Doc.Constants.cn_index);
            var documentIndexService = oldDB.GetCollection<DocumentIndex>(Miga.Doc.Constants.cn_document);
            var srcImageDir          = Path.Combine(databaseFilePath, "Images");
            var dstImageDir          = imageEngine.FullDirectory;

            foreach (var cache in documentIndexService.FindAll()
                                                         .Select(x => Transform(MigratingImage(x.Avatar),x )))
            {
                //
                // porting avatar to new 
                var avatarFileName = Path.Combine(cache.Avatar, srcImageDir);
                var buffer         = await File.ReadAllBytesAsync(avatarFileName);
                var raw            = Pool.MD5.ComputeHash(buffer);
                var md5            = Convert.ToBase64String(raw);
                var ms             = new MemoryStream(buffer);
                imageEngine.AddFile(new FileRecord
                {
                    Id     = md5,
                    Uri    = cache.Avatar,
                    Width  = 256,
                    Height = 256,
                    Type   = ResourceType.Image
                });
                imageEngine.WriteAvatar(ms, cache.Avatar);
                await ms.DisposeAsync();
                documentEngine.AddDocumentCache(cache);
            }

            foreach (var document in documentService.FindAll()
                                                    .Select(Transform))
            {
                documentEngine.AddDocument(document);
            }
        }
        
        public static string GetAvatarName() => string.Format(ImageUtilities.AvatarPattern, ID.Get());

        private static string MigratingImage(string oldPattern)
        {
            // miga://v2.img/QQ图片20230118233550.jpg
            var param = oldPattern.Substring(14);
            return param;
        }

        public static string GetNewImageSource(string param, string srcDir, string dstDir)
        {
            
            var src = Path.Combine(srcDir, param);
            var res = GetAvatarName();
            var dst = Path.Combine(dstDir, res);
            File.Copy(src, dst, true);
            return res;
        }
        
        
        private static async Task MigratingModules(IBusySession session, IDatabaseManager databaseManager, LiteDatabase oldDB)
        {
            await session.Await(Language.GetText("text.Updating.ModuleUpdating"));

            //
            // 
            var oldModuleEngine = oldDB.GetCollection<ModuleIndex>(Miga.Doc.Constants.cn_modules);
            var reader          = new DataPartReader();
            var newModuleEngine = databaseManager.GetEngine<TemplateEngine>();



            foreach (var index in oldModuleEngine.FindAll())
            {
                var result = await reader.ReadFromAsync(index.FileName);
                if (!result.IsFinished)
                {
                    continue;
                }

                var oldModule = result.Result;
                var newModule = ModuleBlockFactory.Upgrade(oldModule);

                newModuleEngine.AddModule(newModule);
            }

        }
        
        private static async Task MigratingComposes()
        {
            
        }
        
        public const string NameField             = "Name";
        public const string UntitledField         = "Untitiled";
    }
}