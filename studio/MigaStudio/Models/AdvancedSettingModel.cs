using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public enum DebugMode
    {
        Debug,
        Attach,
        Release,
    }
    
    public class AdvancedSettingModel : ObservableObject
    {
        private DebugMode _debugMode;

        /// <summary>
        /// 获取或设置 <see cref="DebugMode"/> 属性。
        /// </summary>
        public DebugMode DebugMode
        {
            get => _debugMode;
            set => SetValue(ref _debugMode, value);
        }
    }
}