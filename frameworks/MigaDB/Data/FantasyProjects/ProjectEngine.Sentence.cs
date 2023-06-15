
namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    partial class ProjectEngine
    {

        #region Sentence

        public void AddSentence(Sentence item)
        {
            if (item is null)
            {
                return;
            }

            Sentences.Upsert(item);
        }

        public void RemoveSentence(Sentence item)
        {
            if (item is null)
            {
                return;
            }

            Sentences.Delete(item.Id);
        }

        public IEnumerable<Sentence> GetSentences(DocumentCache source)
        {
            if (source is null)
            {
                return GetSentences();
            }

            return Sentences
                   .Include(y => y.Source)
                   .Find(x => x.Source.Id == source.Id);
        }

        public IEnumerable<Sentence> GetSentences() => Sentences.FindAll();

        #endregion


        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Sentence> Sentences { get; private set; }
    }
}