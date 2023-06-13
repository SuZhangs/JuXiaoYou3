using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public abstract class DocumentEditable<TCache, TDocument> : TabViewModel
        where TDocument : class, IData
        where TCache : class, IDataCache
    {

        #region OnStart

        private void PrepareOpeningDocument(Parameter parameter)
        {
            PrepareOpeningDocument(Cache, Document);
            Cache    = (TCache)parameter.Args[0];
            Document = GetDocumentById(Cache.Id);
        }

        /// <summary>
        /// 在打开文档之前的准备
        /// </summary>
        /// <param name="cache">文档索引</param>
        /// <param name="document">文档本体</param>
        protected abstract void PrepareOpeningDocument(TCache cache, TDocument document);
        
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="cache">文档索引</param>
        /// <param name="document">文档本体</param>
        protected abstract void OpeningDocument(TCache cache, TDocument document);
        
        /// <summary>
        /// 完成文档打开
        /// </summary>
        /// <param name="cache">文档索引</param>
        /// <param name="document">文档本体</param>
        protected abstract void FinishOpeningDocument(TCache cache, TDocument document);
        
        protected sealed override void OnStart(Parameter parameter)
        {
            PrepareOpeningDocument(parameter);
            OpeningDocument(Cache, Document);
            FinishOpeningDocument();
        }

        private void FinishOpeningDocument()
        {
            if (Document is null)
            {
                //
                // 创建文档
                Document = CreateDocument();
            
                //
                // 传递到派生类
                OnCreateDocument(Document);
            }

            // 加载文档
            LoadDocument(Cache, Document);
            
            //
            // 完成打开
            FinishOpeningDocument(Cache, Document);
        }

        protected abstract TDocument CreateDocument();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document">要检查的文档</param>
        /// <remarks>
        /// <para>创建部件时，仅添加到文档！</para>
        /// </remarks>
        protected abstract void OnCreateDocument(TDocument document);

        protected abstract void LoadDocument(TCache cache, TDocument document);
        #endregion

        protected abstract TDocument GetDocumentById(string id);
        
        [NullCheck(UniTestLifetime.Startup)]
        public DocumentType Type { get; protected set; }
        
        [NullCheck(UniTestLifetime.Startup)]
        public TDocument Document { get; protected set; }
        
        [NullCheck(UniTestLifetime.Startup)]
        public TCache Cache { get; protected set; }
    }
}