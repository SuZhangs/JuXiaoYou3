namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public class ProjectEngine : KnowledgeEngine
    {
        public override Knowledge GetKnowledge(string id)
        {
            return new Knowledge
            {
            };
        }

        protected override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            Preshapes     = database.GetCollection<Preshape>(Constants.Name_Preshape);
            Appraises     = database.GetCollection<Appraise>(Constants.Name_Appraise);
            Timelines     = database.GetCollection<TimelineConcept>(Constants.Name_Timeline);
            Terminologies = database.GetCollection<Terminology>(Constants.Name_Terminology);
            Prototypes    = database.GetCollection<Prototype>(Constants.Name_Prototype);
        }

        protected override void OnDatabaseClosingOverride()
        {
            Prototypes    = null;
            Preshapes     = null;
            Appraises     = null;
            Timelines     = null;
            Terminologies = null;
        }
        
        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Prototype> Prototypes { get; private set; }
        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Preshape> Preshapes { get; private set; }
        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Appraise> Appraises { get; private set; }

        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<TimelineConcept> Timelines { get; private set; }

        /// <summary>
        /// 术语
        /// </summary>
        public ILiteCollection<Terminology> Terminologies { get; private set; }
    }
}