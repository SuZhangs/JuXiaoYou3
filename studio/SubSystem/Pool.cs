using System.Buffers;
using System.Security.Cryptography;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class Pool
    {
        public static readonly MD5 MD5 = MD5.Create();
    }
}