namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public partial class UniverseEngine
    {
        protected void OtherOpening(IDatabase database)
        {
            MeasuringUnitDB = database.GetCollection<MeasuringUnit>(Constants.Name_MeasuringUnit);
        }

        protected void OtherClosing()
        {
            FaithDB = null;
        }

        public ILiteCollection<MeasuringUnit> MeasuringUnitDB { get; private set; }
        public ILiteCollection<Market> MarketDB { get; private set; }
    }
}