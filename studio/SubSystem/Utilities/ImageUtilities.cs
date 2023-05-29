using System.IO;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.IO;
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


    public static class ImageUtilities
    {
        public const string AvatarPattern            = "avatar_{0}.png";
        public const string ThumbnailWithSizePattern = "{0}_{1}_{2}";
        public const string ThumbnailPattern         = "thumb_{0}.png";
        public const string RawPattern         = "{0}.png";

        public static string GetAvatarName() => string.Format(AvatarPattern, ID.Get());

        public static async Task CropAllAvatar()
        {
            var dir = Studio.DatabaseManager()
                          .GetEngine<ImageEngine>()
                          .FullDirectory;

            var images = Directory.GetFiles(dir)
                                  .Where(x => x.Contains("avatar"))
                                  .ToArray();


            Image<Rgba32> img;

            await Task.Run(async () =>
            {
                var session = Xaml.Get<IBusyService>().CreateSession();
                session.Update(SubSystemString.Processing);

                foreach (var image in images)
                {
                    img = Image.Load<Rgba32>(image);
                    var w = img.Width;
                    var h = img.Height;

                    if (w > 256 && w == h)
                    {
                        var scale = 256d / w;
                        img.Mutate(x => { x.Resize(new Size((int)(w * scale), (int)(h * scale))); });
                        await img.SaveAsPngAsync(image);
                    }
                }

                session.Dispose();
            });
        }

        public static async Task<ImageOpResult> Avatar()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return new ImageOpResult { IsFinished = false };
            }

            var fileName = opendlg.FileName;

            //
            //
            var          session = Xaml.Get<IBusyService>().CreateSession();
            var          buffer  = await File.ReadAllBytesAsync(fileName);
            var          origin  = new MemoryStream(buffer);
            MemoryStream result;
            var          image = Image.Load<Rgba32>(buffer);
            var          h     = image.Height;
            var          w     = image.Width;


            if (w < 32 || h < 32)
            {
                image.Dispose();
                origin.Dispose();
                await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, SubSystemString.Notify, SubSystemString.ImageTooSmall);
                return new ImageOpResult { IsFinished = false };
            }

            if (w > 4096 ||
                h > 4096)
            {
                await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, SubSystemString.Notify, SubSystemString.ImageTooBig);
                return new ImageOpResult { IsFinished = false };
            }


            if (w != h)
            {
                var horizontal = w > 1920;

                if (horizontal || h > 1080)
                {
                    await Task.Run(async () =>
                    {
                        session.Update(SubSystemString.Processing);
                        await session.Await();
                        var scale = horizontal ? 1920d / w : 1080d / h;
                        h = (int)(h * scale);
                        w = (int)(w * scale);
                        image.Mutate(x => { x.Resize(new Size(w, h)); });

                        // rewrite
                        origin = new MemoryStream(buffer.Length);
                        image.SaveAsPng(origin);
                        origin.Seek(0, SeekOrigin.Begin);
                        session.Dispose();
                    });
                }

                var r = await Xaml.Get<IDialogService>()
                                  .Dialog<MemoryStream, ImageEditViewModel>(new Parameter
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
                origin.Dispose();
                image.Dispose();
                return new ImageOpResult
                {
                    IsFinished = true,
                    Buffer     = result,
                    FileName   = opendlg.FileName
                };
            }

            if (image.Width > 256 ||
                image.Width > 256)
            {
                await Task.Run(async () =>
                {
                    session.Update(SubSystemString.Processing);
                    await session.Await();
                    var scale = 256d / image.Width;
                    image.Mutate(x => { x.Resize(new Size((int)(w * scale), (int)(h * scale))); });

                    // rewrite
                    origin = new MemoryStream(buffer.Length);
                    image.SaveAsPng(origin);
                    origin.Seek(0, SeekOrigin.Begin);
                    session.Dispose();
                });

                return new ImageOpResult
                {
                    IsFinished = true,
                    Buffer     = origin,
                    FileName   = opendlg.FileName
                };
            }


            origin.Dispose();
            result = new MemoryStream(buffer.Length);
            await image.SaveAsPngAsync(result);
            result.Seek(0, SeekOrigin.Begin);
            session.Dispose();

            image.Dispose();
            return new ImageOpResult
            {
                IsFinished = true,
                Buffer     = result,
                FileName   = opendlg.FileName
            };
        }

        public static async Task<Op<Album>> Thumbnail(ImageEngine engine, string fileName)
        {
            var    buffer = await File.ReadAllBytesAsync(fileName);
            byte[] thumbnailBuffer;
            var    raw   = Pool.MD5.ComputeHash(buffer);
            var    md5   = Convert.ToBase64String(raw);
            var    image = Image.Load<Rgba32>(buffer);
            string thumbnail;

            if (engine.HasFile(md5))
            {
                var fr = engine.Records.FindById(md5);
                var src = fr.Uri
                            .Split('_');
                thumbnail = string.Format(ThumbnailPattern, src[0]);
                return Op<Album>.Success(new Album
                {
                    Id     = thumbnail,
                    Source = thumbnail,
                    Width  = int.Parse(src[1]),
                    Height = int.Parse(src[2])
                });
            }

            if (image.Width < 32 || image.Height < 32)
            {
                return Op<Album>.Failed("图片过小");
            }


            var w          = image.Width;
            var h          = image.Height;
            var horizontal = w > 1920;

            if (horizontal || h > 1080)
            {
                var scale = horizontal ? 1920d / image.Width : 1920d / image.Height;
                h = (int)(image.Height * scale);
                w = (int)(image.Width * scale);
                var ms = new MemoryStream(buffer.Length);
                image.Mutate(x => { x.Resize(new Size(w, h)); });
                image.SaveAsPng(ms);
                thumbnailBuffer = ms.GetBuffer();
            }
            else
            {
                thumbnailBuffer = new byte[buffer.Length];
                Array.Copy(buffer, thumbnailBuffer, buffer.Length);
            }

            var id = ID.Get();
            thumbnail = string.Format(ThumbnailPattern, id);
            engine.AddFile(new FileRecord
            {
                Id   = md5,
                Uri  = string.Format(ThumbnailWithSizePattern, id, w, h),
                Type = ResourceType.Image
            });

            engine.Write(thumbnail, thumbnailBuffer);
            return Op<Album>.Success(new Album
            {
                Id = thumbnail,
                Source = thumbnail,
                Width  = w,
                Height = h
            });
        }
        
        public enum ImageScale
        {
            Square,
            Horizontal,
            Vertical
        }
        
        public static async Task<Op<string>> Raw(ImageEngine engine, string fileName, ImageScale scale, Action<Album> callback)
        {
            var    buffer = await File.ReadAllBytesAsync(fileName);
            var    raw   = Pool.MD5.ComputeHash(buffer);
            var    md5   = Convert.ToBase64String(raw);
            var    image = Image.Load<Rgba32>(buffer);
            string thumbnail;


            var w = image.Width;
            var h = image.Height;

            if ((scale == ImageScale.Horizontal && w <= h) ||
                (scale == ImageScale.Vertical && w >= h) ||
                (scale == ImageScale.Square && w != h) )
            {
                return Op<string>.Failed("比例不对");
            }
            
            
            var scale1 = scale == ImageScale.Horizontal ? 1920d / image.Width : 1920d / image.Height;
            var h1     = (int)(image.Height * scale1);
            var w1     = (int)(image.Width * scale1);

            if (engine.HasFile(md5))
            {
                var fr = engine.Records.FindById(md5);
                thumbnail = fr.Uri;
                callback?.Invoke(new Album
                {
                    Id = thumbnail,
                    Source = thumbnail,
                    Width = w1,
                    Height = h1
                });
                return Op<string>.Success(thumbnail);
            }

            if (image.Width < 32 || image.Height < 32)
            {
                return Op<string>.Failed("图片过小");
            }

            var id       = ID.Get();
            var withSize = string.Format(ThumbnailWithSizePattern, id, w1, h1);
            thumbnail = string.Format(RawPattern, id);
            engine.AddFile(new FileRecord
            {
                Id   = md5,
                Uri  = withSize,
                Type = ResourceType.Image
            });

            engine.Write(thumbnail, buffer);
            callback?.Invoke(new Album
            {
                Id = thumbnail,
                Source = thumbnail,
                Width = w1,
                Height = h1
            });
            return Op<string>.Success(withSize);
        }
    }
}