using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaStudio.SubSystem.Models
{
    public class Playlist : ForestObject
    {
        private string _name;

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public ObservableCollection<Music> Items { get; init; }
    }
}