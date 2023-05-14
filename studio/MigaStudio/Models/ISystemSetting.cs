﻿using System.Linq;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public interface ISystemSetting
    {
        Task AddRepository(RepositoryCache cache);
        
        Task SaveAsync();
    }
    
    public class SystemSetting : ISystemSetting
    {
        /// <summary>
        /// 保存
        /// </summary>
        public async Task SaveAsync()
        {
            await JSON.ToFileAsync(RepositorySetting, RepositorySettingFileName);
            await JSON.ToFileAsync(AdvancedSetting, AdvancedSettingFileName);
        }

        public void Save()
        {
            JSON.ToFile(RepositorySetting, RepositorySettingFileName);
            JSON.ToFile(AdvancedSetting, AdvancedSettingFileName);
        }

        public async Task AddRepository(RepositoryCache cache)
        {
            if (cache is null)
            {
                return;
            }

            var r = RepositorySetting.Repositories;
            RepositorySetting.LastRepository = cache.Path;

            if (r.Any(x => x.Path == cache.Path))
            {
                await SaveAsync();
                return;
            }
            
            if (r.Count >= 32)
            {
                r.RemoveAt(r.Count - 1);
            }
            
            r.Add(cache);
            await SaveAsync();
        }
        
        public AdvancedSettingModel AdvancedSetting { get; init; }
        public RepositorySetting RepositorySetting { get; init; }
        
        public string AdvancedSettingFileName { get; init; }
        public string RepositorySettingFileName { get; init; }
    }
}