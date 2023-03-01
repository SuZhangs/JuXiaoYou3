using System.Threading.Tasks;
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
        }

        public async Task AddRepository(RepositoryCache cache)
        {
            if (cache is null)
            {
                return;
            }

            RepositorySetting.LastRepository = cache.Path;
            RepositorySetting.Repositories.Add(cache);
            await SaveAsync();
        }
        
        public AdvancedSettingModel AdvancedSetting { get; init; }
        public RepositorySetting RepositorySetting { get; init; }
        
        public string AdvancedSettingFileName { get; init; }
        public string RepositorySettingFileName { get; init; }
    }
}