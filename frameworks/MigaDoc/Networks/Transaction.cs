
using Newtonsoft.Json;

namespace Acorisoft.Miga.Doc.Networks
{
    public abstract class Transaction
    {
        public string Base64 { get; set; }

        public byte[] FromTransaction()
        {
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
            var length = buffer.Length;
            var dataPackets = new byte[buffer.Length + 4];
            Array.Copy(BitConverter.GetBytes(length), 0 , dataPackets, 0 ,4);
            Array.Copy(buffer, 0 , dataPackets, 4, buffer.Length);
            return dataPackets;
        }

        public static Transaction ToTransaction(byte[] buffer)
        {
            var length = BitConverter.ToInt32(buffer, 0);
            var payload = Encoding.UTF8.GetString(buffer, 4, length);
            return (Transaction)JsonConvert.DeserializeObject(payload);
        }
    }
}