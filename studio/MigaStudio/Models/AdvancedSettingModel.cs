using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class AdvancedSettingModel : ObservableObject
    {
        private DatabaseMode _debugMode;

        /// <summary>
        /// 获取或设置 <see cref="DebugMode"/> 属性。
        /// </summary>
        public DatabaseMode DebugMode
        {
            get => _debugMode;
            set => SetValue(ref _debugMode, value);
        }
    }
}