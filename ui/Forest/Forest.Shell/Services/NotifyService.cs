using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Models;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class NotifyService : INotifyService, INotifyServiceAmbient
    {
        private const string Color = "#6F0240";
        private DialogHost _host;

        /// <summary>
        /// 设置服务响应者
        /// </summary>
        /// <param name="host"></param>
        public void SetServiceProvider(DialogHost host) => _host = host;

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="message">消息</param>
        public void Notify(IconMessage message)
        {
            if (message is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(message.Color))
            {
                message.Color = Color;
            }

            if (message.Delay.TotalMilliseconds < 1000)
            {
                //
                //
                message.Initialize();
            }
            
            _host.Messaging(message);
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="message">消息</param>
        public void Notify(ImageMessage message)
        {
            if (message is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(message.Color))
            {
                message.Color = Color;
            }

            if (message.Delay.TotalMilliseconds < 1000)
            {
                //
                //
                message.Initialize();
            }
            
            _host.Messaging(message);
        }
    }
}