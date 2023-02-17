using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Models;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface INotifyService
    {
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="message">消息</param>
        void Notify(IconMessage message);
        
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="message">消息</param>
        void Notify(ImageMessage message);
    }
    
    
    public interface INotifyServiceAmbient : IServiceAmbient<DialogHost>
    {
        
    }
}