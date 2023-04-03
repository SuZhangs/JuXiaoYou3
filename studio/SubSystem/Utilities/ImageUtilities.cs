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
        public const string AvatarPattern    = "avatar_{0}.png";
        public const string ImagePattern = "{0}.png";
        public const string ThumbnailPattern = "thumb_{0}";

        public static string GetAvatarName() => string.Format(AvatarPattern, ID.Get());
        
        public static async Task<ImageOpResult> Avatar()
        {
            var opendlg = new VistaOpenFileDialog
            {
                Filter      = SubSystemString.ImageFilter,
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
                var origin = new MemoryStream(buffer);
                var result = (MemoryStream)null;
                var image  = Image.Load<Rgba32>(buffer);


                if (image.Width < 32 || image.Height < 32)
                {
                    image.Dispose();
                    origin.Dispose();
                    await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, SubSystemString.Notify, SubSystemString.ImageTooSmall);
                    return new ImageOpResult { IsFinished = false };
                }


                if (image.Width != image.Height)
                {
                    var horizontal = image.Width > 1920;

                    if (horizontal || image.Height > 1080)
                    {
                        await Task.Run(() =>
                        {
                            session.Update(SubSystemString.ImageProcessing);
                            var scale = horizontal ? 1920d / image.Width : 1080d / image.Height;
                            image.Mutate(x => { x.Resize(new Size((int)(image.Width * scale), (int)(image.Height * scale))); });

                            // rewrite
                            origin = new MemoryStream();
                            image.SaveAsPng(origin);
                            origin.Seek(0, SeekOrigin.Begin);
                            session.Dispose();
                        });
                    }

                    var r = await Xaml.Get<IDialogService>()
                                      .Dialog<MemoryStream, ImageEditViewModel>(new RouteEventArgs
                                      {
                                          Args = new object[]
                                          {
                                              image,
                                              origin
                                          }
                                      });

                    if (!r.IsFinished)
                    {
                        origin.Dispose();
                        return new ImageOpResult { IsFinished = false };
                    }

                    result = r.Value;
                    result.Seek(0, SeekOrigin.Begin);
                }


                origin.Dispose();
                result = new MemoryStream();
                await image.SaveAsPngAsync(result);
                result.Seek(0, SeekOrigin.Begin);
                session.Dispose();

                return new ImageOpResult
                {
                    IsFinished = true,
                    Buffer     = result,
                    FileName   = opendlg.FileName
                };
            }
        }


        public static async Task<Op<Tuple<string, string>>> Thumbnail(ImageEngine engine, string fileName)
        {
            var    buffer = await File.ReadAllBytesAsync(fileName);
            byte[] thumbnailBuffer;
            var    raw   = Pool.MD5.ComputeHash(buffer);
            var    md5   = Convert.ToBase64String(raw);
            var    image = Image.Load<Rgba32>(buffer);
            string source;
            string thumbnail;

            if (engine.HasFile(md5))
            {
                var fr = engine.Records.FindById(md5);
                source    = string.Format(ImagePattern,fr.Uri);
                thumbnail = string.Format(ThumbnailPattern, source);
                return Op<Tuple<string, string>>.Success(new Tuple<string, string>(source, thumbnail));
            }

            if (image.Width < 32 || image.Height < 32)
            {
                buffer = null;
                return Op<Tuple<string, string>>.Failed("图片过小");
            }


            var horizontal = image.Width > 1280;

            if (horizontal || image.Height > 720)
            {
                var scale = horizontal ? 1280d / image.Width : 720d / image.Height;
                var ms    = new MemoryStream();
                image.Mutate(x => { x.Resize(new Size((int)(image.Width * scale), (int)(image.Height * scale))); });
                image.SaveAsPng(ms);
                thumbnailBuffer = ms.GetBuffer();
            }
            else
            {
                thumbnailBuffer = new byte[buffer.Length];
                Array.Copy(buffer, thumbnailBuffer, buffer.Length);
            }
            
            source    = string.Format(ImagePattern, ID.Get());
            thumbnail = string.Format(ThumbnailPattern, source);
            engine.AddFile(new FileRecord
            {
                Id = md5,
                Uri = source,
                Type = ResourceType.Image
            });

            engine.Write(source, buffer);
            engine.Write(thumbnail, thumbnailBuffer);
            return Op<Tuple<string, string>>.Success(new Tuple<string, string>(source, thumbnail));
        }
    }
}