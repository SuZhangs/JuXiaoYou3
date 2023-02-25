using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    /// <summary>
    /// <see cref="IViewHostServiceAdapter"/> 类型表示一个视图服务模型的适配器，用于重写
    /// </summary>
    public interface IViewHostServiceAdapter
    {
        /// <summary>
        /// 跳转到指定的页面
        /// </summary>
        /// <param name="parameter">跳转参数</param>
        /// <typeparam name="TViewModel">指定的视图模型</typeparam>
        /// <returns>返回操作结果</returns>
        void Route<TViewModel>(NavigationParameter parameter)  where TViewModel : IViewModel;

        /// <summary>
        /// 跳转到指定的页面
        /// </summary>
        /// <param name="parameter">跳转参数</param>
        /// <param name="viewModel">指定的视图模型</param>
        /// <returns>返回操作结果</returns>
        void Route(IViewModel viewModel, NavigationParameter parameter);
        
        /// <summary>
        /// 
        /// </summary>
        IViewHostService Host { get; }
        
        /// <summary>
        /// 控制器
        /// </summary>
        ITabViewController Controller { get; }
    }
}