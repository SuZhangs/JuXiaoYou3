using System.Collections.ObjectModel;


namespace Acorisoft.Miga.Doc.Parts
{
    public class DocumentData : FixedDataPart
    {
        public ObservableCollection<TimelineSet> TimelineSets { get; set; }
        public ObservableCollection<string> Album { get; set; }
        public ObservableCollection<Glimpse> Inspiration { get; set; }
        public ObservableCollection<SurveySet> SurveySets { get; set; }
        public ObservableCollection<object> StickyNote { get; set; }
        public ObservableCollection<object> Others { get; set; }
        public ObservableCollection<SimpleTextNode> SimpleTexts { get; set; }

    }

    public class SimpleTextNode : ObservableObject
    {
        private string _name;
        private string _content;

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}