namespace Acorisoft.FutureGL.MigaDB.UnitTests
{
    public class DataEngineAdapter
    {
        public DataEngineAdapter()
        {
            
        }
    }

    public class DataEngineAdapter<TEngine> : DataEngineAdapter where TEngine : DataEngine
    {
        public DataEngineAdapter(TEngine engine) : base()
        {
            //
            // 创建引擎。
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        /// <summary>
        /// 开始托管
        /// </summary>
        public void Start()
        {
            
        }
        
        /// <summary>
        /// 获取当前测试的引擎。
        /// </summary>
        public TEngine Engine { get; }
    }
}