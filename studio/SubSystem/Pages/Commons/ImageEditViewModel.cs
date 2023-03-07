
using Acorisoft.FutureGL.Forest.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ImageEditViewModel : InputViewModel
    {
        private double _scale;
        private double _imageWidth;
        private double _imageHeight;
        private double _thumbSize;
        
        public override void Start()
        {
            base.Start();
        }

        protected override void OnStart(Parameter parameter)
        {
            if (parameter.Args[0] is Image<Rgba32> img)
            {
                BackendImage = img;
                ImageWidth   = img.Width;
                _imageHeight = img.Height;
            }
        }

        protected override void ReleaseManagedResources()
        {
            BackendImage?.Dispose();
            BackendImage = null;
            base.ReleaseManagedResources();
        }

        protected override bool IsCompleted()
        {
            throw new System.NotImplementedException();
        }

        protected override void Finish()
        {
            throw new System.NotImplementedException();
        }

        protected override string Failed()
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// 目标图片
        /// </summary>
        public Image<Rgba32> BackendImage { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="ThumbSize"/> 属性。
        /// </summary>
        public double ThumbSize
        {
            get => _thumbSize;
            set => SetValue(ref _thumbSize, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ImageHeight"/> 属性。
        /// </summary>
        public double ImageHeight
        {
            get => _imageHeight;
            set => SetValue(ref _imageHeight, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ImageWidth"/> 属性。
        /// </summary>
        public double ImageWidth
        {
            get => _imageWidth;
            set => SetValue(ref _imageWidth, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Scale"/> 属性。
        /// </summary>
        public double Scale
        {
            get => _scale;
            set => SetValue(ref _scale, value);
        }
    }
}