﻿using System.Windows.Media;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestSection : ForestHeaderedControlBase
    {
        

        public static readonly DependencyProperty HeaderThicknessProperty = DependencyProperty.Register(
            nameof(HeaderThickness),
            typeof(Thickness),
            typeof(ForestSection),
            new PropertyMetadata(default(Thickness)));


        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
            nameof(HeaderPadding),
            typeof(Thickness),
            typeof(ForestSection),
            new PropertyMetadata(default(Thickness)));


        /// <summary>
        /// 实现 <see cref="ToolBar"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarProperty = DependencyProperty.Register(
            nameof(ToolBar),
            typeof(object),
            typeof(ForestSection),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="ToolBarTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarTemplateProperty = DependencyProperty.Register(
            nameof(ToolBarTemplate),
            typeof(DataTemplate),
            typeof(ForestSection),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="ToolBarTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ToolBarTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(ForestSection),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="ToolBarStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarStringFormatProperty = DependencyProperty.Register(
            nameof(ToolBarStringFormat),
            typeof(string),
            typeof(ForestSection),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
            nameof(HeaderFontSize),
            typeof(double),
            typeof(ForestSection),
            new PropertyMetadata(default(double)));


        public static readonly DependencyProperty HeaderFontFamilyProperty = DependencyProperty.Register(
            nameof(HeaderFontFamily),
            typeof(FontFamily),
            typeof(ForestSection),
            new PropertyMetadata(default(FontFamily)));


        public static readonly DependencyProperty HeaderBrushProperty = DependencyProperty.Register(
            nameof(HeaderBrush),
            typeof(Brush),
            typeof(ForestSection),
            new PropertyMetadata(default(Brush)));

        public Brush HeaderBrush
        {
            get => (Brush)GetValue(HeaderBrushProperty);
            set => SetValue(HeaderBrushProperty, value);
        }

        static ForestSection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestSection), new FrameworkPropertyMetadata(typeof(ForestSection)));
        }
        

        public double HeaderFontSize
        {
            get => (double)GetValue(HeaderFontSizeProperty);
            set => SetValue(HeaderFontSizeProperty, value);
        }
        
        public FontFamily HeaderFontFamily
        {
            get => (FontFamily)GetValue(HeaderFontFamilyProperty);
            set => SetValue(HeaderFontFamilyProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBarStringFormat"/> 属性。
        /// </summary>
        public string ToolBarStringFormat
        {
            get => (string)GetValue(ToolBarStringFormatProperty);
            set => SetValue(ToolBarStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBarTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector ToolBarTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ToolBarTemplateSelectorProperty);
            set => SetValue(ToolBarTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBarTemplate"/> 属性。
        /// </summary>
        public DataTemplate ToolBarTemplate
        {
            get => (DataTemplate)GetValue(ToolBarTemplateProperty);
            set => SetValue(ToolBarTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBar"/> 属性。
        /// </summary>
        public object ToolBar
        {
            get => (object)GetValue(ToolBarProperty);
            set => SetValue(ToolBarProperty, value);
        }   
        
        public Thickness HeaderPadding
        {
            get => (Thickness)GetValue(HeaderPaddingProperty);
            set => SetValue(HeaderPaddingProperty, value);
        }

        
        public Thickness HeaderThickness
        {
            get => (Thickness)GetValue(HeaderThicknessProperty);
            set => SetValue(HeaderThicknessProperty, value);
        }
    }
}