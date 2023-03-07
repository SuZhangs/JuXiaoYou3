using System;
using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Interfaces;
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
            var buffer = await File.ReadAllBytesAsync(fileName);
            var ms     = new MemoryStream(buffer);
            var image  = Image.Load<Rgba32>(buffer);
            

            if (image.Width < 32 || image.Height < 32)
            {
                buffer = null;
                image.Dispose();
                ms.Dispose();
                await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, StringFromCode.Notify, StringFromCode.ImageTooSmall);
                return new ImageOpResult{ IsFinished = false};
            }

            if (image.Width > 1920 || image.Height > 1080)
            {
                buffer = null;
                image.Dispose();
                ms.Dispose();
                await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, StringFromCode.Notify, StringFromCode.ImageTooBig);
                return new ImageOpResult{ IsFinished = false};
            }

            if (image.Width != image.Height)
            {
                var ds = Xaml.Get<IDialogService>();
                var r = await ds.Dialog<MemoryStream, ImageEditViewModel>(new Parameter
                {
                    Args = new object[]
                    {
                        image,
                        ms
                    }
                });

                if (!r.IsFinished)
                {
                    return new ImageOpResult { IsFinished = false };
                }

                ms.Dispose();
                ms = r.Value;
            }
            else
            {
                ms.Dispose();
                ms = new MemoryStream();
                await image.SaveAsPngAsync(ms);
            }

            return new ImageOpResult
            {
                IsFinished = true,
                Buffer     = ms,
                FileName   = opendlg.FileName
            };
        }
    }
}