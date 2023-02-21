using System.Windows;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface ILanguageNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void SetText(string text);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void SetToolTips(string text);
    }
}