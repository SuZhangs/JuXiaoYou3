
namespace Acorisoft.Miga.Doc.Groups
{
    public class CarryGroupingInformation : TeamGroupingInformation
    {
        private DocumentIndexCopy _carry;
        private string            _carrySummary;

        /// <summary>
        /// 获取或设置 <see cref="CarrySummary"/> 属性。
        /// </summary>
        public string CarrySummary
        {
            get => _carrySummary;
            set => SetValue(ref _carrySummary, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="Carry"/> 属性。
        /// </summary>
        public DocumentIndexCopy Carry
        {
            get => _carry;
            set => SetValue(ref _carry, value);
        }
        
    }
}