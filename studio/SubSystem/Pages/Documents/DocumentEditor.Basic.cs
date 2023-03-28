namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    partial class DocumentEditorVMBase
    {
        private string _name;
        private string _avatar;
        private string _gender;
        private string _birth;
        private string _height;
        private string _weight;

        /// <summary>
        /// 获取或设置 <see cref="Weight"/> 属性。
        /// </summary>
        public string Weight
        {
            get => _weight;
            set => SetValue(ref _weight, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Height"/> 属性。
        /// </summary>
        public string Height
        {
            get => _height;
            set => SetValue(ref _height, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Birth"/> 属性。
        /// </summary>
        public string Birth
        {
            get => _birth;
            set => SetValue(ref _birth, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Gender"/> 属性。
        /// </summary>
        public string Gender
        {
            get => _gender;
            set => SetValue(ref _gender, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set
            {
                SetValue(ref _avatar, value);
            }
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