using System.Runtime.InteropServices;
using Koyashiro.PngChunkUtil;

namespace Acorisoft.Miga.Doc.Utils
{
    public static class PNG
    {
        public static async Task WriteTo(string fileName, string payload, byte[] png)
        {
            var payloadBuffer = Encoding.UTF8.GetBytes(payload);
            await WriteTo(fileName, payloadBuffer, png);
        }
        
        public static async Task WriteTo(string fileName, byte[] payload, byte[] png)
        {
            var chunks = PngReader.ReadBytes(png);
            var chunks1 = new Chunk[chunks.Length];
            var IEND = Chunk.Create("IEND", payload);
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                chunks1[i] = chunks[i];
            }
            chunks1[^1] = IEND;
            var buffer = PngWriter.WriteBytes(CollectionsMarshal.AsSpan<Chunk>(chunks1.ToList()));
            await File.WriteAllBytesAsync(fileName, buffer);
        }
        
        public static async Task<string> ReadFrom(string fileName)
        {
            var dataPackets = await File.ReadAllBytesAsync(fileName);
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();

            try
            {
                var payload = Encoding.UTF8.GetString(buffer);
                return payload;
            }
            catch
            {
                return "{}";
            }
        }

        public static string ReadFrom(byte[] dataPackets)
        {
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();

            try
            {
                var payload = Encoding.UTF8.GetString(buffer);
                return payload;
            }
            catch
            {
                return "{}";
            }
        }

        public static async Task<byte[]> ReadOriginalImage(string fileName)
        {
            var dataPackets = await File.ReadAllBytesAsync(fileName);
            return ReadOriginalImage(dataPackets);
        }
        
        public static byte[] ReadOriginalImage(byte[] dataPackets)
        {
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = Chunk.Create("IEND", Array.Empty<byte>());
            chunks[^1] = IEND;
            return PngWriter.WriteBytes(chunks);
        }
    }
}