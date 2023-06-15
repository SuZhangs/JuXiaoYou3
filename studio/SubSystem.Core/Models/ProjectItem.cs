using System.Collections;
using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject
{
    public class ProjectItem : ObservableObject, ICollection<ProjectItem>
    {
        private string _name;
        private string _toolTips;
        

        #region ICollection<ProjectItem>
        
        
        public IEnumerator<ProjectItem> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Children).GetEnumerator();
        }

        public void Add(ProjectItem item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(ProjectItem item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(ProjectItem[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(ProjectItem item)
        {
            return Children.Remove(item);
        }

        public int Count => Children.Count;

        public bool IsReadOnly => ((ICollection<ProjectItem>)Children).IsReadOnly;
        
        #endregion

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
        /// 视图模型
        /// </summary>
        public Type ViewModelType { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public TabViewModel ViewModel { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement View { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Func<ProjectItem, FrameworkElement> Expression { get; init; }

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