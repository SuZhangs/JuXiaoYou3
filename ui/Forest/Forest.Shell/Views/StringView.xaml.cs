using System.Windows.Controls;

namespace Acorisoft.FutureGL.Forest.Views
{
    public partial class StringView : UserControl
    {
        public StringView()
        {
            InitializeComponent();
        }
    }

    public sealed class StringViewModel : InputViewModel
    {
        private string     _text;

        protected override bool IsCompleted() => !string.IsNullOrEmpty(_text);

        protected override void Finish()
        {
            Result = _text;
        }

        protected override string Failed()
        {
            return "值为空";
        }


        /// <summary>
        /// 获取或设置 <see cref="Text"/> 属性。
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                SetValue(ref _text, value);
            }
        }
    }
}