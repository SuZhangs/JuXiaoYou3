using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils;
using Microsoft.Win32;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Acorisoft.FutureGL.MigaStudio.Utils
{
    public static class ImageHelper
    {
        public static async Task SetImage(Action<Result<string>> callback, bool requireSizeConstraint)
        {
            var opendlg = new OpenFileDialog
            {
                Filter      = GetImageFilter(),
            };

            if (opendlg.ShowDialog() != true)
            {
                callback?.Invoke(Result<string>.Failed(AppReason.CanceledByUser));
            }

            try
            {
                var fileName = opendlg.FileName;
                var buffer = await File.ReadAllBytesAsync(fileName);
                var ms = new MemoryStream(buffer);
                var image = Image.Load<Rgba32>(ms);

                //
                // 要求图片处理器进行处理
                if (requireSizeConstraint && image.Width != image.Height)
                {
                    var processTask = ImageHandler?.Invoke(image);

                    if (processTask is null)
                    {
                        callback?.Invoke(Result<string>.Failed(AppReason.MissingService));
                        return;
                    }
                    
                    var finalResultStream = processTask.
                }
                else
                {
                    
                }
            }
            catch (IOException)
            {
                callback?.Invoke(Result<string>.Failed(AppReason.Occupied));
            }
        }

        internal static string GetImageFilter()
        {
            // TODO: 翻译
            return Forest.Language.Culture switch
            {
                CultureArea.English => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.French => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _ => "图片文件|*.png;*.jpg;*.bmp;*.jpeg",
            };
        }
        
        /// <summary>
        /// 图片处理程序
        /// </summary>
        public static Func<Image<Rgba32>, Task<Result<Stream>>> ImageHandler { get; set; }
    }
}