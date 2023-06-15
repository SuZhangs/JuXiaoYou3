using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject
{
    public class ProjectItem : ObservableObject
    {
        private string _name;
        private string _toolTips;

        /// <summary>
        /// 获取或设置 <see cref="ToolTips"/> 属性。
        /// </summary>
        public string ToolTips
        {
            get => _toolTips;
            set => SetValue(ref _toolTips, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public TabViewModel ViewModel { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Func<ITabViewModel> Expression { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DocumentType Type { get; init; }
        
        /// <summary>
        /// 参数1
        /// </summary>
        public object Parameter1 { get; init; }
        
        /// <summary>
        /// 参数2
        /// </summary>
        public object Parameter2 { get; init; }
        
        /// <summary>
        /// 参数3
        /// </summary>
        public object Parameter3 { get; init; }
        
        /// <summary>
        /// 子级菜单
        /// </summary>
        public ObservableCollection<ProjectItem> Property { get; init; }
        
        /// <summary>
        /// 子级菜单
        /// </summary>
        public ObservableCollection<ProjectItem> Children { get; init; }
    }
}