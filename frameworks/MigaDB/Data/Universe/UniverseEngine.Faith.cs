namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public partial class UniverseEngine
    {
        protected bool GetFaithKnowledge(string id, out Knowledge knowledge)
        {
            throw new NotImplementedException();
        }

        protected void FaithOpening(IDatabase database)
        {
            FaithDB = database.GetCollection<Faith>(Constants.Name_Faith);
        }

        protected void FaithClosing()
        {
            FaithDB = null;
        }

        public ILiteCollection<Faith> FaithDB { get; private set; }
    }
}