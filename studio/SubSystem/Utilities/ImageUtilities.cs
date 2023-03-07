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
using SixLabors.ImageSharp.Processing;
using IDialogService = Acorisoft.FutureGL.Forest.Interfaces.IDialogService;

// ReSharper disable MethodHasAsyncOverload
// ReSharper disable ConvertToUsingDeclaration

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
            using (var session = Xaml.Get<IBusyService>().CreateSession())
            {
                var buffer = await File.ReadAllBytesAsync(fileName);
                var ms     = new MemoryStream(buffer);
                var image  = Image.Load<Rgba32>(buffer);


                if (image.Width < 32 || image.Height < 32)
                {
                    image.Dispose();
                    ms.Dispose();
                    await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, StringFromCode.Notify, StringFromCode.ImageTooSmall);
                    return new ImageOpResult { IsFinished = false };
                }


                if (image.Width != image.Height)
                {
                    var ds         = Xaml.Get<IDialogService>();
                    var horizontal = image.Width > 1920;

                    if (horizontal || image.Height > 1080)
                    {
                        await Task.Run(() =>
                        {
                            session.Update(StringFromCode.ImageProcessing);
                            var scale = horizontal ? 1920d / image.Width : 1080d / image.Height;
                            image.Mutate(x => { x.Resize(new Size((int)(image.Width * scale), (int)(image.Height * scale))); });
                            var ms1 = new MemoryStream();
                            image.SaveAsPng(ms1);
                            ms1.Seek(0, SeekOrigin.Begin);
                            ms.Dispose();
                            ms = ms1;
                            session.Dispose();
                        });
                    }

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
                        ms.Dispose();
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
                    session.Dispose();
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
}