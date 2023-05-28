using System.Xml;
using Koyashiro.PngChunkUtil;

namespace Acorisoft.Miga.Doc.Parts
{
    public class DataPartReader
    {
        private readonly XmlParser _parser;

        public DataPartReader()
        {
            _parser = XmlParser.GetParser(new []
            {
                typeof(Module),
                typeof(ColorProperty),
                typeof(GroupProperty),
                typeof(ImageProperty),
                typeof(NumberProperty),
                typeof(OptionProperty),
                typeof(ReferenceProperty),
                typeof(SequenceProperty),
                typeof(TextProperty),
                typeof(PageProperty),
                typeof(Value),
                typeof(DegreeProperty)
            });
        }

        public Compilation<Module> Read(string fileName)
        {
            var buffer = File.ReadAllText(fileName);
            var document = XDocument.Load(new StringReader(buffer), LoadOptions.SetLineInfo);
            var compilation = _parser.Parse<Module>(document);

            return compilation;
        }

        
        public async Task<Compilation<Module>> ReadAsync(string fileName)
        {
            var buffer = await File.ReadAllTextAsync(fileName);
            var document = XDocument.Load(new StringReader(buffer), LoadOptions.SetLineInfo);
            var compilation = _parser.Parse<Module>(document);

            return compilation;
        }
        
        public  Compilation<Module> ReadFrom(string fileName)
        {
            var dataPackets = File.ReadAllBytes(fileName);
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer1 = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();

            try
            {

                var reader = XmlReader.Create(new MemoryStream(buffer1));
                var document = XDocument.Load(reader, LoadOptions.SetLineInfo);
                var compilation = _parser.Parse<Module>(document);

                return compilation;
            }
            catch
            {
                return new Compilation<Module>
                {
                    IsFinished = false,
                    
                };
            }
        }
        
        public async Task<Compilation<Module>> ReadFromAsync(string fileName)
        {
            var dataPackets = await File.ReadAllBytesAsync(fileName);
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();
            var payload = Encoding.UTF8.GetString(buffer); 
            try
            {
                var document = XDocument.Load(new StringReader(payload), LoadOptions.SetLineInfo);
                var compilation = _parser.Parse<Module>(document);

                return compilation;
            }
            catch
            {
                return new Compilation<Module>
                {
                    IsFinished = false,
                };
            }
        }
        
        public Compilation<Module> ReadFromAsync(byte[] dataPackets)
        {
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();
            var payload = Encoding.UTF8.GetString(buffer); 
            try
            {
                var document = XDocument.Load(new StringReader(payload), LoadOptions.SetLineInfo);
                var compilation = _parser.Parse<Module>(document);

                return compilation;
            }
            catch
            {
                return new Compilation<Module>
                {
                    IsFinished = false,
                };
            }
        }
    }
}