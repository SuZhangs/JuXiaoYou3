using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils;
using Microsoft.Win32;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image; 

namespace Acorisoft.FutureGL.MigaStudio.Utils
{
    public static class ImageHelper
    {
        /// <summary>
        /// 在异步可等待的
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="requireSizeConstraint"></param>
        public static async Task SetImage(Action<Result<Resource>> callback, bool requireSizeConstraint)
        {
            var opendlg = new OpenFileDialog
            {
                Filter = StringFromCode.GetImageFilter(),
            };

            if (opendlg.ShowDialog() != true)
            {
                callback?.Invoke(Result<Resource>.Failed(AppReason.CanceledByUser));
            }

            try
            {
                var fileName = opendlg.FileName;
                var buffer = await File.ReadAllBytesAsync(fileName);
                var ms = new MemoryStream(buffer);
                var image = Image.Load<Rgba32>(ms);
                var w = image.Width;
                var h = image.Height;
                var horizontalOrientation = w > h;
                var square = w == h;

                //
                // 要求图片处理器进行处理
                if (requireSizeConstraint && !square)
                {
                }

                // 240p 320x240
                // 720p 1280x720
                // 1080p 1920x1080
                ResourceProperty property;

                if (horizontalOrientation)
                {
                    var largeThan240 = w >= 320;
                    var largeThan720 = w >= 1280;
                    var largeThan1080 = w >= 1920;

                    property =
                        largeThan240
                            ? largeThan720
                                ? largeThan1080 ? ResourceProperty.Image_Horizontal_1080P : ResourceProperty.Image_Horizontal_720P
                                : ResourceProperty.Image_Horizontal_240P
                            : ResourceProperty.Image_Horizontal_MinSize;
                }
                else
                {
                    var largeThan240 = h >= 240;
                    var largeThan720 = h >= 720;
                    var largeThan1080 = h >= 1080;

                    property =
                        largeThan240
                            ? largeThan720
                                ? largeThan1080 ? ResourceProperty.Image_Horizontal_1080P : ResourceProperty.Image_Horizontal_720P
                                : ResourceProperty.Image_Horizontal_240P
                            : ResourceProperty.Image_Horizontal_MinSize;
                }

                var res = new Resource
                {
                    Property = property,
                    RelativePath = opendlg.FileName
                };
                
                //
                // 通知客户端
                callback?.Invoke(Result<Resource>.Successful(res));
            }
            catch (IOException)
            {
                callback?.Invoke(Result<Resource>.Failed(AppReason.Occupied));
            }
        }

        /// <summary>
        /// 图片处理程序
        /// </summary>
        public static Func<Image<Rgba32>, Task<Result<Stream>>> ImageProcessHandler { get; set; }
    }
}