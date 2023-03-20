namespace Acorisoft.FutureGL.Forest
{
    public  class XamlAssist : DependencyObject
    {
        #region EnableSizeScale

        
        public static readonly DependencyProperty EnableSizeScaleProperty = DependencyProperty.RegisterAttached(
            "EnableSizeScale", typeof(bool), typeof(XamlAssist), new PropertyMetadata(Boxing.False, OnHighSolutionChanged));

        public static readonly DependencyProperty OriginalSizeProperty = DependencyProperty.RegisterAttached(
            "OriginalSize", typeof(int), typeof(XamlAssist), new PropertyMetadata(-1));

        public static void SetOriginalSize(DependencyObject element, int value)
        {
            element.SetValue(OriginalSizeProperty, value);
        }

        public static int GetOriginalSize(DependencyObject element)
        {
            return (int)element.GetValue(OriginalSizeProperty);
        }

        private static void OnHighSolutionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Grid grid)
            {
                return;
            }
            
            grid.SizeChanged += OnSizeChanged;
            grid.Unloaded += (_,_) => grid.SizeChanged -= OnSizeChanged;
        }

        private static void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            var width  = grid.ActualWidth;
            var height = grid.ActualHeight;

            if (width <= 1920 && height <= 1080)
            {
                return;
            }
            
            var scaleY = height / 1080;
            var scaleX = width / 1920;
            int originalSize;

            foreach (var definition in grid.ColumnDefinitions)
            {
                if (definition.Width.IsAuto)
                {
                    continue;
                }

                originalSize = GetOriginalSize(definition);

                if (originalSize == -1)
                {
                    originalSize = (int)definition.Width.Value;
                    SetOriginalSize(definition, originalSize);
                }

                definition.Width = new GridLength(originalSize * scaleX);
            }
                
            foreach (var definition in grid.RowDefinitions)
            {
                if (definition.Height.IsAuto)
                {
                    continue;
                }
                
                originalSize = GetOriginalSize(definition);

                if (originalSize == -1)
                {
                    originalSize = (int)definition.Height.Value;
                    SetOriginalSize(definition, originalSize);
                }
                    
                definition.Height = new GridLength(originalSize * scaleY);
            }
        }

        public static void SetEnableSizeScale(Grid element, bool value)
        {
            element.SetValue(EnableSizeScaleProperty, value);
        }

        public static bool GetEnableSizeScale(Grid element)
        {
            return (bool)element.GetValue(EnableSizeScaleProperty);
        }
        
        #endregion
    }
}