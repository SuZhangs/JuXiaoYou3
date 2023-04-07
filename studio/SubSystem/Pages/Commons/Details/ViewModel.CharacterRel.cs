namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class CharacterRelPartViewModel
    {
        /// <summary>
        /// 编辑器
        /// </summary>
        public DocumentEditorVMBase EditorViewModel { get; init; }

        /// <summary>
        /// 文档
        /// </summary>
        public Document Document { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DocumentCache DocumentCache { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public PartOfRel Detail { get; init; }
    }
}