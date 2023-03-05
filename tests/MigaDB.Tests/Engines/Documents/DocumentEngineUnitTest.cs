using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.UnitTests;
using Acorisoft.FutureGL.MigaDB.Utils;
// ReSharper disable JoinDeclarationAndInitializer

namespace MigaDB.Tests.Engines.Documents
{
    [TestClass, TestCategory("Engines")]
    public class DocumentEngineUnitTest
    {
        [TestMethod]
        public void AddDocumentWasCorrection()
        {
            var adapter = new DataEngineAdapter<DocumentEngine>(new DocumentEngine());
            var engine = adapter.Engine;
            adapter.Start();

            EngineResult er;

            er = engine.AddDocument(null);
            Assert.IsTrue(er.Reason == EngineFailedReason.ParameterEmptyOrNull);
            
            er = engine.AddDocument(new Document());
            Assert.IsTrue(er.Reason == EngineFailedReason.MissingId);
            
            
            er = engine.AddDocument(new Document{ Id = "1", Metas = new MetadataCollection()});
            Assert.IsTrue(er.Reason == EngineFailedReason.ParameterDataError);
            
            
            er = engine.AddDocument(new Document{ Id = "1", Parts = new DataPartCollection()});
            Assert.IsTrue(er.Reason == EngineFailedReason.ParameterDataError);

            var docA = new Document { Id = "A", Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            var docB = new Document { Id = "A", Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            var docC = new Document { Id = "C", Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            var docD = new Document { Id = "D", Metas = new MetadataCollection(), Parts = new DataPartCollection() };

            er = engine.AddDocument(docA);
            Assert.IsTrue(er.IsFinished);

            er = engine.AddDocument(docB);
            Assert.IsTrue(er.Reason == EngineFailedReason.Duplicated);

            er = engine.AddDocument(docC);
            Assert.IsTrue(er.IsFinished);
            
            er = engine.AddDocument(docD);
            Assert.IsTrue(er.IsFinished);
            
            adapter.Restart();
            Assert.IsTrue(engine.DocumentDB.HasID(docA.Id) && engine.DocumentCacheDB.HasID(docA.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docC.Id) && engine.DocumentCacheDB.HasID(docC.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docD.Id) && engine.DocumentCacheDB.HasID(docD.Id));
            
            adapter.Stop();
        }
        
        
        [TestMethod]
        public void UpdateDocumentWasCorrection()
        {
            var adapter = new DataEngineAdapter<DocumentEngine>(new DocumentEngine());
            var engine = adapter.Engine;
            adapter.Start();

           
            var docA = new Document { Id = "A", Version = 1, Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            var docB = new Document { Id = "A", Version = 2, Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            
            //
            engine.UpdateDocument(null);
            Assert.IsTrue(engine.DocumentDB.Count() == 0); 
            
            //
            //
            engine.UpdateDocument(new Document());
            Assert.IsTrue(engine.DocumentDB.Count() == 0); 
            
            
            engine.UpdateDocument(new Document{ Metas = new MetadataCollection()});
            Assert.IsTrue(engine.DocumentDB.Count() == 0); 
            
            
            engine.UpdateDocument(new Document{ Parts = new DataPartCollection()});
            Assert.IsTrue(engine.DocumentDB.Count() == 0); 
            
            
            engine.AddDocument(docA);
            Assert.IsTrue(engine.DocumentDB.Count() == 1); 
            engine.UpdateDocument(docB);

            var docC = engine.DocumentDB.FindById("A");
            Assert.IsTrue(docC.Version == 2);
            
            adapter.Restart();
            Assert.IsTrue(engine.DocumentDB.HasID(docA.Id) && engine.DocumentCacheDB.HasID(docA.Id));
            
            adapter.Stop();
        }
        
        [TestMethod]
        public void RemoveDocumentWasCorrection()
        {
            var adapter = new DataEngineAdapter<DocumentEngine>(new DocumentEngine());
            var engine = adapter.Engine;
            adapter.Start();

           
            
            var docA = new Document { Id = "A", Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            var docB = new Document { Id = "B", Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            var docC = new Document { Id = "C", Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            var docD = new Document { Id = "D", Metas = new MetadataCollection(), Parts = new DataPartCollection() };
            
            //
            engine.AddDocument(docA);
            engine.AddDocument(docB);
            engine.AddDocument(docC);
            engine.AddDocument(docD);
            
            engine.RemoveDocument(docA);
            engine.RemoveDocument(docB);
            engine.RemoveDocument(docC);
            engine.RemoveDocument(docD);
            
            Assert.IsTrue(engine.DocumentDB.HasID(docA.Id) && engine.DocumentCacheDB.HasID(docA.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docB.Id) && engine.DocumentCacheDB.HasID(docB.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docC.Id) && engine.DocumentCacheDB.HasID(docC.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docD.Id) && engine.DocumentCacheDB.HasID(docD.Id));
            Assert.IsTrue(engine.DocumentCacheDB.FindById("A").IsDeleted);
            Assert.IsTrue(engine.DocumentCacheDB.FindById("B").IsDeleted);
            Assert.IsTrue(engine.DocumentCacheDB.FindById("C").IsDeleted);
            Assert.IsTrue(engine.DocumentCacheDB.FindById("D").IsDeleted);
            
            adapter.Restart();
            Assert.IsTrue(engine.DocumentDB.HasID(docA.Id) && engine.DocumentCacheDB.HasID(docA.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docB.Id) && engine.DocumentCacheDB.HasID(docB.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docC.Id) && engine.DocumentCacheDB.HasID(docC.Id));
            Assert.IsTrue(engine.DocumentDB.HasID(docD.Id) && engine.DocumentCacheDB.HasID(docD.Id));
            Assert.IsTrue(engine.DocumentCacheDB.FindById("A").IsDeleted);
            Assert.IsTrue(engine.DocumentCacheDB.FindById("B").IsDeleted);
            Assert.IsTrue(engine.DocumentCacheDB.FindById("C").IsDeleted);
            Assert.IsTrue(engine.DocumentCacheDB.FindById("D").IsDeleted);
            
            engine.ReduceSize();
            Assert.IsTrue(!engine.DocumentDB.HasID(docA.Id) && !engine.DocumentCacheDB.HasID(docA.Id));
            Assert.IsTrue(!engine.DocumentDB.HasID(docB.Id) && !engine.DocumentCacheDB.HasID(docB.Id));
            Assert.IsTrue(!engine.DocumentDB.HasID(docC.Id) && !engine.DocumentCacheDB.HasID(docC.Id));
            Assert.IsTrue(!engine.DocumentDB.HasID(docD.Id) && !engine.DocumentCacheDB.HasID(docD.Id));
        }
    }
}