namespace Acorisoft.Miga.Doc.Core
{
    public interface IRepositoryEngine
    {
        Task SaveAsync();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IsCompleted<string>> CloseAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="name"></param>
        /// <param name="ownerName"></param>
        /// <param name="avatar"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IsCompleted<string>> CreateAsync(
            string folderName,
            string name,
            string ownerName,
            string avatar,
            string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<IsCompleted<string>> CreateAsync(string folderName, RepositoryProperty property);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        Task<IsCompleted<string>> LoadAsync(string folderName);

        /// <summary>
        /// 获取指定类型的数据服务。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>() where T : StorageService;

        /// <summary>
        /// 获取所有的数据服务
        /// </summary>
        /// <returns></returns>
        IEnumerable<StorageService> GetServices();

        /// <summary>
        /// 获取当前仓储引擎的加载状态
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// 
        /// </summary>
        string RepositoryFolder { get; }

        /// <summary>
        /// 获取当前打开的仓库的仓储属性
        /// </summary>
        RepositoryProperty Property { get; }
        
        /// <summary>
        /// 获取当前仓储的信息
        /// </summary>
        public RepositoryInformation Information  { get; }

        /// <summary>
        /// 获取当前仓储的配置
        /// </summary>
        public RepositoryConfiguration Configuration  { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public RepositoryAuthor Author { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public ObjectManager ObjectManager { get; }
    }
}