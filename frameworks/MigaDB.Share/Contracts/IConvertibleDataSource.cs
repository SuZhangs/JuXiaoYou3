namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IConvertibleDataSource
    {
        /// <summary>
        /// 转化为纯文本
        /// </summary>
        /// <returns>返回带有固定格式的纯文本</returns>
        string ToPlainText();
        
        /// <summary>
        /// 转化为Markdown文本
        /// </summary>
        /// <returns>返回Markdown文本。</returns>
        string ToMarkdown();
    }
}