using System.Windows;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IViewModelLanguageService
    {
        /// <summary>
        /// 根元素名
        /// </summary>
        string RootName { get; set; }
        
        /// <summary>
        /// 获取语言服务信息
        /// </summary>
        Dictionary<string, ILanguageNode> ElementBag { get; set; }
    }
}