using DryIoc;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    /// <summary>
    /// 取消安装
    /// </summary>
    public interface IDataEngine
    {
        /// <summary>
        /// 安装
        /// </summary>
        /// <param name="container">服务容器</param>
        void Install(IContainer container);

        /// <summary>
        /// 取消安装
        /// </summary>
        /// <param name="container">服务容器</param>
        void Uninstall(IContainer container);
        
        /// <summary>
        /// 激活
        /// </summary>
        /// <returns></returns>
        bool Activate();
        
        /// <summary>
        /// 是否激活
        /// </summary>
        bool Activated { get; }
        
        /// <summary>
        /// 是否为懒加载模式
        /// </summary>
        bool IsLazyMode { get; set; }
    }
}