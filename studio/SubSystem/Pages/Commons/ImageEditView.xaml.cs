using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest.Adorners;
using NLog.Filters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using Pen = System.Windows.Media.Pen;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public partial class ImageEditView : UserControl
    {
        public ImageEditView()
        {
            InitializeComponent();
        }
    }
}