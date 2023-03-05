using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{
    [Name("新建设定")]
    public class NewDocumentWizardViewModel : DialogViewModel
    {
        private string _name;
        private string _avatar;
        private DocumentType _type;

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set => SetValue(ref _type, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
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