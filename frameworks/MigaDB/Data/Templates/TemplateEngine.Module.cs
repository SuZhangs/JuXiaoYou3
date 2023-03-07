

using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Utils;
using Newtonsoft.Json;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates
{
    public partial class TemplateEngine
    {
        private void AddMetadata(ModuleTemplate template)
        {
            foreach (var meta in template.MetadataList)
            {
                var outsideMetadata = MetadataCacheDB.FindById(meta.Id);
                if (outsideMetadata is null)
                {
                    outsideMetadata = new MetadataCache
                    {
                        Id       = ID.Get(),
                        RefCount = 1,
                        Name     = meta.Name
                    };

                    MetadataCacheDB.Insert(outsideMetadata);
                }
                else
                {
                    outsideMetadata.RefCount++;
                    MetadataCacheDB.Update(meta);
                }
            }
        }
        
        private void RemoveMetadata(ModuleTemplate template)
        {
            foreach (var meta in template.MetadataList)
            {
                var insideMetadata = MetadataCacheDB.FindById(meta.Id);
                if (insideMetadata is null)
                {
                    continue;
                }

                insideMetadata.RefCount--;
                
                //
                //
                if (insideMetadata.RefCount == 0)
                {
                    MetadataCacheDB.Delete(meta.Id);
                }
                else
                {
                    MetadataCacheDB.Update(meta);
                }
            }
        }
        
        private void UpdateMetadata(ModuleTemplate insideTemplate, ModuleTemplate outSideTemplate)
        {
            //
            // removeAll
            RemoveMetadata(insideTemplate);
            
            //
            // addAll
            AddMetadata(outSideTemplate);
        }
        
        protected void ModuleOpening(IDatabase database)
        {
            TemplateDB      = database.GetCollection<ModuleTemplate>(Constants.Name_ModuleTemplate);
            TemplateCacheDB = database.GetCollection<ModuleTemplateCache>(Constants.Name_Cache_ModuleTemplate);
            MetadataCacheDB = database.GetCollection<MetadataCache>(Constants.Name_Cache_Metadata);
        }

        protected void ModuleClosing()
        {
            TemplateDB      = null;
            TemplateCacheDB = null;
            MetadataCacheDB = null;
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="fileName">指定要添加的模板路径。</param>
        /// <returns>返回操作结果</returns>
        public async Task<EngineResult> AddTemplate(string fileName)
        {
            try
            {
                var payload = await PNG.ReadDataAsync(fileName);
                var template = JSON.FromJson<ModuleTemplate>(payload);
                Modified();
                return await AddTemplate(template);
            }
            catch (IOException)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterSourceOccupied);
            }
            catch (UnauthorizedAccessException)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterSourceUnauthorizedAccess);
            }
            catch (JsonException)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterDataError);
            }
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="template">指定要添加的参数。</param>
        /// <returns>返回操作结果</returns>
        public Task<EngineResult> AddTemplate(ModuleTemplate template)
        {
            if (template is null)
            {
                return Task.FromResult(EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull));
            }
            
            return Task.Run(() =>
            {
                if (!ModuleTemplate.IsValid(template))
                {
                    return EngineResult.Failed(EngineFailedReason.ParameterDataError);
                }

                //
                // 判断数据库中是否有同名模组
                var templateInside = TemplateDB.FindById(template.Id);

                if (templateInside is null)
                {
                    
                    //
                    // Update
                    TemplateDB.Insert(template);
                    TemplateCacheDB.Insert(template.ExtractIndex());
                    AddMetadata(template);
                }
                else
                {
                    if (templateInside.Version >= template.Version)
                    {
                        return EngineResult.Failed(EngineFailedReason.NoChanged);
                    }
                    
                    //
                    // Update
                    TemplateDB.Update(template);
                    TemplateCacheDB.Update(template.ExtractIndex());
                    UpdateMetadata(templateInside, template);
                }

                Modified();
                return EngineResult.Successful;
            });
        }

        /// <summary>
        /// 移除模板
        /// </summary>
        /// <param name="template">指定要添加的参数。</param>
        /// <returns>返回操作结果</returns>
        public Task<EngineResult> RemoveTemplate(ModuleTemplate template)
        {
            if (template is null)
            {
                return Task.FromResult(EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull));
            }
            
            return Task.Run(() =>
            {
                //
                // 判断数据库中是否有同名模组
                var templateInside = TemplateDB.FindById(template.Id);

                if (templateInside is not null)
                {
                    TemplateDB.Delete(template.Id);
                    TemplateCacheDB.Delete(template.Id);
                    RemoveMetadata(template);
                    Modified();
                    return EngineResult.Successful;
                }
                
                return EngineResult.Failed(EngineFailedReason.MissingParameter);
            });
            
        }
        
        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="id">指定要添加的参数。</param>
        /// <param name="fileName">指定要添加的模板路径。</param>
        /// <returns>返回操作结果</returns>
        public async Task<EngineResult> ExportTemplate(string id, string fileName)
        {
            if (string.IsNullOrEmpty(id) ||
                string.IsNullOrEmpty(fileName))
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            //
            var template = TemplateDB.FindById(id);
            
            if (template is null)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }
            
            if (string.IsNullOrEmpty(fileName))
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }


            await JSON.ToFileAsync(template, fileName);
            return EngineResult.Successful;
        }

        /// <summary>
        /// 模板
        /// </summary>
        public ILiteCollection<ModuleTemplate> TemplateDB { get; private set; }
        
        /// <summary>
        /// 模板缓存
        /// </summary>
        public ILiteCollection<ModuleTemplateCache> TemplateCacheDB { get; private set; }
        
        /// <summary>
        /// 元数据缓存
        /// </summary>
        public ILiteCollection<MetadataCache> MetadataCacheDB { get; private set; }
    }
}