using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NLog.Filters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using Pen = System.Windows.Media.Pen;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;
using Rectangle = SixLabors.ImageSharp.Rectangle;

// ReSharper disable MethodHasAsyncOverload

namespace Acorisoft.FutureGL.MigaStudio.Windows
{
    public class ThumbAdorner : Adorner
    {
        private static readonly SolidColorBrush Brush = new SolidColorBrush(Colors.Teal);
        private static readonly Pen             Pen   = new Pen(Brush, 1);

        public ThumbAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        private void DrawCorner(DrawingContext drawingContext, Point pos)
        {
            drawingContext.DrawRectangle(Brush, null, new Rect(pos.X - 2, pos.Y - 2, 4, 4));
        }

        private void DrawAllCorner(DrawingContext drawingContext, double width, double height)
        {
            DrawCorner(drawingContext, new Point(0, 0));
            DrawCorner(drawingContext, new Point(width, 0));
            DrawCorner(drawingContext, new Point(0, height));
            DrawCorner(drawingContext, new Point(width, height));

            DrawCorner(drawingContext, new Point(width / 2, 0));
            DrawCorner(drawingContext, new Point(0, height / 2));
            DrawCorner(drawingContext, new Point(width, height / 2));
            DrawCorner(drawingContext, new Point(width / 2, height));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var actualHeight = AdornedElement.RenderSize.Height;
            var actualWidth = AdornedElement.RenderSize.Width;
            drawingContext.DrawRectangle(null, Pen, new Rect(0, 0, actualWidth, actualHeight));
            DrawAllCorner(drawingContext, actualWidth, actualHeight);
            base.OnRender(drawingContext);
        }
    }

    public class AvatarEditViewModel : ObservableObject
    {
        private double _scale;
        private double _imageWidth;
        private double _imageHeight;
        private double _thumbSize;

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

    public partial class ImageEditView
    {
        private const int ScaleFactor = 20;

        public ImageEditView()
        {
            InitializeComponent();
            InstallControls();
            ViewModel   = new AvatarEditViewModel();
            DataContext = ViewModel;
        }

        #region StartDragging

        private bool   _startDragging;
        private double _x;
        private double _y;

        private void InstallControls()
        {
            //
            // Thumb
            Thumb.Height               =  100;
            Thumb.Width                =  100;
            Thumb.MouseLeftButtonDown  += DraggingStart;
            Canvas.MouseLeftButtonDown += DraggingStartFromOriginal;
            Canvas.MouseLeave          += DraggingFinished;
            Canvas.MouseLeftButtonUp   += DraggingFinished;
        }

        private void Dragging(object sender, MouseEventArgs e)
        {
            if (!_startDragging) return;
            var pos = e.GetPosition(sender as IInputElement);
            _x = pos.X;
            _y = pos.Y;
            SetPositionAndImageCropped(pos);
            EnsureViewPort();
            e.Handled = true;
        }

        private void DraggingStartFromOriginal(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(sender as IInputElement);
            _x             = pos.X;
            _y             = pos.Y;
            _startDragging = true;
            SetPositionAndImageCropped(pos);
            EnsureViewPort();

            Canvas.CaptureMouse();
            Canvas.MouseMove += Dragging;
            e.Handled        =  true;
        }

        private void DraggingStart(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(sender as IInputElement);
            pos            = Thumb.PointToScreen(pos);
            pos            = Canvas.PointFromScreen(pos);
            _x             = pos.X;
            _y             = pos.Y;
            _startDragging = true;
            SetPositionAndImageCropped(pos);
            EnsureViewPort();

            Canvas.CaptureMouse();
            Canvas.MouseMove += Dragging;
            e.Handled        =  true;
        }

        private void DraggingFinished(object sender, MouseButtonEventArgs e)
        {
            _startDragging   =  false;
            e.Handled        =  true;
            Canvas.MouseMove -= Dragging;
            Canvas.ReleaseMouseCapture();
        }

        private void DraggingFinished(object sender, MouseEventArgs e)
        {
            _startDragging   =  false;
            e.Handled        =  true;
            Canvas.MouseMove -= Dragging;
            Canvas.ReleaseMouseCapture();
        }

        private void EnsureViewPort()
        {
            var verticalOffset = _y1 - ScrollViewer.ViewportHeight + ThumbSize;
            var horizontalOffset = _x1 - ScrollViewer.ViewportWidth + ThumbSize;

            // if (verticalOffset > 0)
            // {
            //     ScrollViewer.ScrollToVerticalOffset(verticalOffset - _thumbSize);
            // }
            //
            // if (horizontalOffset > 0)
            // {
            //     ScrollViewer.ScrollToHorizontalOffset(verticalOffset - _thumbSize);
            // }
        }

        #endregion

        #region ImageEditing

        private bool          _loading;
        private double        _minImageSize;
        private MemoryStream  _imageStream;
        private byte[]        _imageBuffer;
        private BitmapImage   _grayImage;
        private double        _thumbOriginalSize;
        private CroppedBitmap _cropped;
        private Image<Rgba32> _backendImage;
        private ImageBrush    _thumbImage;
        private ThumbAdorner  _adorner;
        private AdornerLayer  _layer;
        private int           _x1, _y1;

        private void ReleaseResource()
        {
            _layer?.Remove(_adorner);
            ScaleDown.IsEnabled = true;
            ScaleUp.IsEnabled   = true;
            Scale               = 100;
            _x                  = 0;
            _y                  = 0;
            ImageHeight         = 0;
            ImageWidth          = 0;
            ThumbSize           = 0;
            _thumbImage         = null;
            _loading            = false;
            //
            // release all resource
            _imageBuffer = null;
            _grayImage   = null;
            _cropped     = null;
            _imageStream?.Dispose();
            _backendImage?.Dispose();
        }

        public static BitmapImage FromStream(Stream stream)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption   = BitmapCacheOption.Default;
            bi.CreateOptions = BitmapCreateOptions.None;
            bi.StreamSource  = stream;
            bi.EndInit();
            bi.Freeze();
            return bi;
        }

        #endregion

        private void SetDefaultPositionAndData()
        {
            _adorner = new ThumbAdorner(Thumb);
            _layer   = AdornerLayer.GetAdornerLayer(Thumb);
            _layer?.Add(_adorner);


            Scale       = 100;
            ImageHeight = _backendImage.Height;
            ImageWidth  = _backendImage.Width;

            if (ImageWidth == 0 || ImageHeight == 0)
            {
                ReleaseResource();
                MessageBox.Show("图片大小异常，请使用其它的图片处理软件编辑！");
                return;
            }

            if (ImageWidth < 32 || ImageHeight < 32)
            {
                ReleaseResource();
                MessageBox.Show("图片大小太小，请使用其它的图片处理软件编辑！");
                return;
            }

            if (ImageWidth > 4096 || ImageHeight > 2560)
            {
                ReleaseResource();
                MessageBox.Show("图片大小过大，请使用其它的图片处理软件编辑！");
                return;
            }


            _minImageSize = Math.Min(ImageHeight, ImageWidth);

            _thumbOriginalSize =
                ThumbSize = Math.Clamp(_minImageSize, Math.Min(_minImageSize, 64), 100);

            //
            // Set Behavior
            ImageActualSize.Text = $"Actual Size : {ImageWidth},{ImageHeight}";
            ImageCropSize.Text   = $"Crop Size : {ThumbSize},{ThumbSize}";
            Thumb.Width          = ThumbSize;
            Thumb.Height         = ThumbSize;
            Canvas.SetLeft(Thumb, 0);
            Canvas.SetTop(Thumb, 0);

            //
            //
            _cropped           = new CroppedBitmap(_grayImage, new Int32Rect(0, 0, (int)ThumbSize, (int)ThumbSize));
            _thumbImage        = new ImageBrush { ImageSource = _cropped };
            ActualImage.Source = _grayImage;
            ActualImage.Width  = ImageWidth;
            ActualImage.Height = ImageHeight;
            Canvas.Width       = ImageWidth;
            Canvas.Height      = ImageHeight;

            //
            // Thumb And Preview
            Thumb.Background   = _thumbImage;
            Preview.Background = _thumbImage;
        }

        private void SetPositionAndImageCropped(Point pos)
        {
            if (!_loading) return;

            var thumbSize = ThumbSize / 2d;
            _x1 = (int)Math.Clamp(pos.X - thumbSize, 0, ImageWidth);
            _y1 = (int)Math.Clamp(pos.Y - thumbSize, 0, ImageHeight);

            //
            // 检查边界
            if (_x < 0 || _x + thumbSize > ImageWidth)
            {
                _x1 = _x1 <= 0 ? 0 : (int)(ImageWidth - ThumbSize);
                Canvas.SetLeft(Thumb, _x1);
            }

            if (_y < 0 || _y + thumbSize > ImageHeight)
            {
                _y1 = _y1 <= 0 ? 0 : (int)(ImageHeight - ThumbSize);
            }

            Canvas.SetLeft(Thumb, _x1);
            Canvas.SetTop(Thumb, _y1);

            _cropped                = new CroppedBitmap(_grayImage, new Int32Rect(_x1, _y1, (int)ThumbSize, (int)ThumbSize));
            _thumbImage.ImageSource = _cropped;
            Thumb.Background        = _thumbImage;
            Preview.Background      = _thumbImage;
        }

        private async void Button_OpenImage(object sender, RoutedEventArgs e)
        {
            // var opendlg = App.OpenDlg(SR.Filter_Image);
            // if (opendlg.ShowDialog() != true) return;
            //
            // var fileName = opendlg.FileName;
            //
            // try
            // {
            //     //
            //     // release all resource
            //     ReleaseResource();
            //
            //     //
            //     // loading
            //     _imageBuffer  = await File.ReadAllBytesAsync(fileName);
            //     _imageStream  = new MemoryStream(_imageBuffer);
            //     _backendImage = Image.Load<Rgba32>(_imageBuffer);
            //     _grayImage    = FromStream(_imageStream);
            //     _loading      = true;
            //
            //     //
            //     //
            //     SetDefaultPositionAndData();
            // }
            // catch (Exception exception)
            // {
            //     Console.WriteLine(exception);
            //     throw;
            // }
        }

        private void Button_SaveImage(object sender, RoutedEventArgs e)
        {
            // var savedlg = App.SaveDlg(SR.Filter_PNG);
            // if (savedlg.ShowDialog() != true) return;
            //
            // try
            // {
            //     _backendImage.Mutate(x => { x.Crop(new Rectangle(_x1, _y1, (int)ThumbSize, (int)ThumbSize)); });
            //     _backendImage.SaveAsPng(savedlg.FileName);
            //     MessageBox.Show("保存成功");
            // }
            // catch
            // {
            //     MessageBox.Show("保存失败");
            // }
        }

        private void Button_ScaleUp(object sender, RoutedEventArgs e)
        {
            if (!_loading) return;
            var newThumbSize = _thumbOriginalSize * ((Scale + ScaleFactor) / 100d);
            if (newThumbSize > _minImageSize)
            {
                return;
            }

            ScaleDown.IsEnabled =  true;
            Scale               += ScaleFactor;
            ThumbSize           =  _thumbOriginalSize * Scale / 100d;
            Thumb.Width         =  ThumbSize;
            Thumb.Height        =  ThumbSize;

            ImageCropSize.Text = $"Crop Size : {ThumbSize},{ThumbSize}";
            SetPositionAndImageCropped(new Point(_x, _y));

            //
            // enabled
            ScaleUp.IsEnabled = !(_thumbOriginalSize * ((Scale + ScaleFactor) / 100d) > _minImageSize);
        }

        private void Button_ScaleDown(object sender, RoutedEventArgs e)
        {
            if (!_loading) return;
            var minSize = Math.Min(32, _minImageSize);
            var newThumbSize = _thumbOriginalSize * ((Scale - ScaleFactor) / 100d);
            if (newThumbSize < minSize)
            {
                return;
            }

            ScaleUp.IsEnabled  =  true;
            Scale              -= ScaleFactor;
            ThumbSize          =  _thumbOriginalSize * Scale / 100d;
            Thumb.Width        =  ThumbSize;
            Thumb.Height       =  ThumbSize;
            ImageCropSize.Text =  $"Crop Size : {ThumbSize},{ThumbSize}";
            SetPositionAndImageCropped(new Point(_x, _y));

            //
            // enabled
            ScaleDown.IsEnabled = !(_thumbOriginalSize * ((Scale - ScaleFactor) / 100d) < minSize);
        }


        private void Button_FromClipboard(object sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsImage())
                return;

            try
            {
                var dataObject = Clipboard.GetDataObject();
                if (dataObject is null)
                {
                    return;
                }

                var bitmap = dataObject.GetData("System.Drawing.Bitmap") as Bitmap;
                //var b2 = dataObject.GetData("System.Windows.Media.Imaging.BitmapSource");

                if (bitmap is null)
                {
                    return;
                }

                ReleaseResource();

                var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                var imgPtr = bitmapData.Scan0;
                var length = Math.Abs(bitmapData.Stride) * bitmap.Height;

                _imageBuffer = new byte[length];
                _imageStream = new MemoryStream();

                //
                // 从像素数据转化为PNG
                Marshal.Copy(imgPtr, _imageBuffer, 0, length);
                bitmap.UnlockBits(bitmapData);
                var raw = Image.LoadPixelData<Bgr24>(_imageBuffer, bitmap.Width, bitmap.Height);
                _backendImage = raw.CloneAs<Rgba32>();
                _backendImage.SaveAsPng(_imageStream);
                _imageStream.Seek(0, SeekOrigin.Begin);

                _grayImage = FromStream(_imageStream);
                _loading   = true;

                //
                //
                SetDefaultPositionAndData();
            }
            catch
            {
            }
        }

        public AvatarEditViewModel ViewModel { get; }

        public double Scale
        {
            get => ViewModel.Scale;
            set => ViewModel.Scale = value;
        }

        public double ImageWidth
        {
            get => ViewModel.ImageWidth;
            set => ViewModel.ImageWidth = value;
        }

        public double ImageHeight
        {
            get => ViewModel.ImageHeight;
            set => ViewModel.ImageHeight = value;
        }

        public double ThumbSize
        {
            get => ViewModel.ThumbSize;
            set => ViewModel.ThumbSize = value;
        }
    }
}