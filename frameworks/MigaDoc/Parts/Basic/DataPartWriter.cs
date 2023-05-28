using System.Runtime.InteropServices;
using Koyashiro.PngChunkUtil;

namespace Acorisoft.Miga.Doc.Parts
{
    public class DataPartWriter
    {
        public void Write(Module module, string fileName)
        {
            if (module is null)
            {
                throw new InvalidOperationException(nameof(module));
            }

            var document = GetModule(module);
            using var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite); 
            document.Save(fs, SaveOptions.None);
        }
        
        public void WriteTo(Module module, byte[] imageBuffer, string fileName)
        {
            if (module is null)
            {
                throw new InvalidOperationException(nameof(module));
            }

            var document = GetModule(module);
            using var ms = new MemoryStream(65536);
            var sw = new StringWriter();
            document.Save(sw, SaveOptions.None);
            
            var payloadBuffer = Encoding.UTF8.GetBytes(sw.GetStringBuilder().ToString());
            var chunks = PngReader.ReadBytes(imageBuffer);
            var chunks1 = new Chunk[chunks.Length];
            var IEND = Chunk.Create("IEND", payloadBuffer);
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                chunks1[i] = chunks[i];
            }
            chunks1[^1] = IEND;
            var buffer = PngWriter.WriteBytes(CollectionsMarshal.AsSpan<Chunk>(chunks1.ToList()));
            File.WriteAllBytes(fileName, buffer);
        }
        
        public async Task WriteToAsync(Module module, byte[] imageBuffer, string fileName)
        {
            if (module is null)
            {
                throw new InvalidOperationException(nameof(module));
            }

            var document = GetModule(module);
            using var ms = new MemoryStream(65536);
            var sw = new StringWriter();
            document.Save(sw, SaveOptions.None);
            
            var payloadBuffer = Encoding.UTF8.GetBytes(sw.GetStringBuilder().ToString());
            var chunks = PngReader.ReadBytes(imageBuffer);
            var chunks1 = new Chunk[chunks.Length];
            var IEND = Chunk.Create("IEND", payloadBuffer);
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                chunks1[i] = chunks[i];
            }
            chunks1[^1] = IEND;
            var buffer = PngWriter.WriteBytes(CollectionsMarshal.AsSpan<Chunk>(chunks1.ToList()));
            await File.WriteAllBytesAsync(fileName, buffer);
        }

        private static XElement GetModule(Module module)
        {
            var element = new XElement("module");
            
            element.Add(new XAttribute("id", module.Id));
            element.Add(new XAttribute("author", module.Author ?? string.Empty));
            element.Add(new XAttribute("org", module.Organization ?? string.Empty));
            element.Add(new XAttribute("contract", module.Contract ?? string.Empty));
            element.Add(new XAttribute("name", module.Name ?? string.Empty));
            element.Add(new XAttribute("ver", module.Version));
            element.Add(new XAttribute("type", module.Type));

            foreach (var node in module.Items.Select(InputProperty.GetElement))
            {
                element.Add(node);
            }
            
            return element;
        }
    }
}