using System.Net.WebSockets;

namespace Acorisoft.Miga.Doc.Networks
{
    public class RemoteDevice
    {
        private readonly ClientWebSocket _client;
        
        public RemoteDevice(IRepositoryEngine engine)
        {
            Engine  = engine;
            _client = new ClientWebSocket();
        }
        

        public Task<Manifest> BuildManifest()
        {
            return Task.Run(() =>
            {
                var image = Engine.GetService<ImageService>();
                var module = Engine.GetService<ModuleService>();
                var diff = Engine.GetService<ComposeService>();

                var directlyServices = new List<IDirectlyDifferenceProvider>(32);
                foreach (var service in Engine.GetServices().Where(x => x is IDirectlyDifferenceProvider))
                {
                    service.ManualInitialized();
                    directlyServices.Add(service as IDirectlyDifferenceProvider);
                }
                
                var directly = new List<DirectlyDescription>(512);

                foreach (var directlyService in directlyServices)
                {
                    directlyService.GetDescriptions(directly);
                }

                return new Manifest
                {
                    Modules    = module.GetDescriptions(),
                    Image      = image.GetDescriptions(),
                    Difference = diff.GetDescriptions(),
                    Directly   = directly
                };
            });
        }

        public Task<TransactionManifest> Process(TransactionManifest manifest)
        {
            return Task.Run(() =>
            {
                if (manifest is null)
                {
                    return null;
                }
                
                var image = Engine.GetService<ImageService>();
                var module = Engine.GetService<ModuleService>();
                var diff = Engine.GetService<ComposeService>();
            
                // ReSharper disable once SuspiciousTypeConversion.Global
                var directlyServices = Engine.GetServices().OfType<IDirectlyDifferenceProvider>();
                var directlyServiceDictionary = new Dictionary<EntityID, IDirectlyDifferenceProvider>();
                
                foreach (var directlyService in directlyServices)
                {
                    directlyService.BuildService(directlyServiceDictionary);
                }

                foreach (var transaction in manifest.Module)
                {
                    module.Process(transaction);
                }
                
                foreach (var transaction in manifest.Image)
                {
                    image.Process(transaction);
                }
                
                foreach (var transaction in manifest.Difference)
                {
                    diff.Process(transaction);
                }
                
                foreach (var transaction in manifest.Directly)
                {
                    if (directlyServiceDictionary.TryGetValue(transaction.EntityId, out var provider))
                    {
                        provider.Process(transaction);
                    }
                }
                
                
                return manifest;
            });
        }
        
        public IRepositoryEngine Engine { get; }
    }
}