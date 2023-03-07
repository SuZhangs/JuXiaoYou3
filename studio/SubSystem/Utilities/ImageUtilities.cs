using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Ookii.Dialogs.Wpf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using IDialogService = Acorisoft.FutureGL.Forest.Interfaces.IDialogService;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public class ImageOpResult
    {
        public bool IsFinished { get; init; }
        public string FileName { get; init; }
        public MemoryStream Buffer { get; init; }
    }

    public class ImageUtilities
    {
        private static readonly byte[] __MemoryBuffer = new byte[8 * 1048576];
        private static readonly MD5    _md5           = MD5.Create();

        public static async Task<ImageOpResult> Avatar()
        {
            var opendlg = new VistaOpenFileDialog
            {
                Filter      = StringFromCode.ImageFilter,
                Multiselect = false
            };

            if (opendlg.ShowDialog() != true)
            {
                return new ImageOpResult { IsFinished = false };
            }

            var fileName = opendlg.FileName;

            //
            //
            var          image = Image.Load<Rgba32>(fileName);
            MemoryStream ms;

            if (image.Width != image.Height)
            {
                var ds = Xaml.Get<IDialogService>();
                var r = await ds.Dialog<MemoryStream, ImageEditViewModel>(new Parameter
                {
                    Args = new object[]
                    {
                        image
                    }
                });

                if (!r.IsFinished)
                {
                    return new ImageOpResult { IsFinished = false };
                }

                ms = r.Value;
            }
            else
            {
                ms = new MemoryStream(__MemoryBuffer);
                await image.SaveAsPngAsync(ms);
            }
            
            

            //
            // 计算HASH
            var originLength = (int)ms.Length;
            var originBuffer = ms.GetBuffer();
            var saltedStream = new MemoryStream(originBuffer, 0, originLength);
            saltedStream.Write(BitConverter.GetBytes(originLength));

            var raw = await _md5.ComputeHashAsync(saltedStream);
            var md5 = Convert.ToBase64String(raw);
            var ie  = Xaml.Get<ImageEngine>();
            var uri = Resource.ToUnifiedUri($"avatar_{ID.Get()}", ResourceType.Image);

            if (ie.HasFile(md5))
            {
                var fr = ie.Records.FindById(md5);
                return new ImageOpResult
                {
                    IsFinished = true,
                    Buffer     = ms,
                    FileName   = opendlg.FileName
                };
            }

            var record = new FileRecord
            {
                Id  = md5,
                Uri = uri,
            };

            ie.AddFile(record);

            return new ImageOpResult
            {
                IsFinished = true,
                Buffer     = ms,
                FileName   = opendlg.FileName
            };
        }
    }
}