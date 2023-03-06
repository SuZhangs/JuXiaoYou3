﻿using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    /// <summary>
    /// 这是一个特殊用途的按钮
    /// </summary>
    public class DialogButton : ForestButtonBase
    {
        public static readonly DependencyProperty PurposeProperty = DependencyProperty.Register(
            nameof(Purpose),
            typeof(ButtonPurpose),
            typeof(DialogButton),
            new PropertyMetadata(ButtonPurpose.CallToAction));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(Geometry),
            typeof(DialogButton),
            new PropertyMetadata(default(Geometry)));


        public static readonly DependencyProperty IsFilledProperty = DependencyProperty.Register(
            nameof(IsFilled),
            typeof(bool),
            typeof(DialogButton),
            new PropertyMetadata(Boxing.False));


        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(DialogButton),
            new PropertyMetadata(17d));


        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        private const string PART_IconName    = "PART_Icon";

        private Border           _bd;
        private ContentPresenter _content;
        private Path             _icon;
        private Storyboard       _storyboard;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundHighlightBrush;
        private SolidColorBrush _foregroundHighlight2Brush;
        private SolidColorBrush _backgroundDisabledBrush;
        private SolidColorBrush _foregroundDisabledBrush;

        public DialogButton()
        {
            StateMachine.StateChangedHandler = OnStateChanged;
        }

        private void InvalidateState()
        {
            _backgroundBrush           = null;
            _foregroundBrush           = null;
            _backgroundHighlight1Brush = null;
            _foregroundHighlightBrush = null;
            _backgroundHighlight2Brush = null;
            _foregroundHighlight2Brush = null;
            _backgroundDisabledBrush   = null;
            _foregroundDisabledBrush   = null;
            InvalidateVisual();
        }

        private void OnStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var theme = ThemeSystem.Instance.Theme;

            // Stop Animation
            _storyboard?.Stop(_bd);

            if (!init)
            {
                HandleNormalState();
            }
            else
            {
                if (now == VisualState.Highlight1)
                {
                    HandleHighlight1State(theme.Duration.Medium);
                }
                else if (now == VisualState.Highlight2)
                {
                    HandleHighlight2State(theme.Duration.Medium);
                }
                else if (now == VisualState.Normal)
                {
                    HandleNormalState();
                }
                else if (now == VisualState.Inactive)
                {
                    HandleDisabledState();
                }
            }
        }

        private void SetForeground(Brush foreground)
        {
            _content.SetValue(TextElement.ForegroundProperty, foreground);

            if (IsFilled)
            {
                _icon.StrokeThickness = 0;
                _icon.Fill            = foreground;
            }
            else
            {
                _icon.StrokeThickness = 2;
                _icon.Stroke          = foreground;
            }
        }

        public static Color GetPurposeColor(ButtonPurpose purpose, int level)
        {
            var colors = ThemeSystem.Instance.Theme.Colors;

            return level switch
            {
                2 => purpose switch
                {

                    ButtonPurpose.CallToAction => colors[(int)ForestTheme.HighlightA4],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB4],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC4],
                    ButtonPurpose.CallToClose  => colors[(int)ForestTheme.Danger200],
                    ButtonPurpose.Close        => colors[(int)ForestTheme.SlateGray200],
                    ButtonPurpose.Warning      => colors[(int)ForestTheme.Warning200],
                    ButtonPurpose.Info         => colors[(int)ForestTheme.Info200],
                    ButtonPurpose.Success      => colors[(int)ForestTheme.Success200],
                    ButtonPurpose.Obsolete     => colors[(int)ForestTheme.Obsolete200],
                    _                          => colors[(int)ForestTheme.HighlightA4],
                },
                3 => purpose switch
                {

                    ButtonPurpose.CallToAction  => colors[(int)ForestTheme.HighlightA5],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB5],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC5],
                    ButtonPurpose.CallToClose   => colors[(int)ForestTheme.Danger300],
                    ButtonPurpose.Close         => colors[(int)ForestTheme.SlateGray300],
                    ButtonPurpose.Warning       => colors[(int)ForestTheme.Warning300],
                    ButtonPurpose.Info          => colors[(int)ForestTheme.Info300],
                    ButtonPurpose.Success       => colors[(int)ForestTheme.Success300],
                    ButtonPurpose.Obsolete      => colors[(int)ForestTheme.Obsolete300],
                    _                           => colors[(int)ForestTheme.HighlightA5],
                },
                4 => purpose switch
                {
                    ButtonPurpose.CallToAction  => colors[(int)ForestTheme.HighlightA1],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB1],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC1],
                    ButtonPurpose.CallToClose   => colors[(int)ForestTheme.DangerDisabled],
                    ButtonPurpose.Close         => colors[(int)ForestTheme.SlateGrayDisabled],
                    ButtonPurpose.Warning       => colors[(int)ForestTheme.WarningDisabled],
                    ButtonPurpose.Info          => colors[(int)ForestTheme.InfoDisabled],
                    ButtonPurpose.Success       => colors[(int)ForestTheme.SuccessDisabled],
                    ButtonPurpose.Obsolete      => colors[(int)ForestTheme.ObsoleteDisabled],
                    _                           => colors[(int)ForestTheme.HighlightA1],
                },
                _ => purpose switch
                {
                    ButtonPurpose.CallToAction  => colors[(int)ForestTheme.HighlightA3],
                    ButtonPurpose.CallToAction2 => colors[(int)ForestTheme.HighlightB3],
                    ButtonPurpose.CallToAction3 => colors[(int)ForestTheme.HighlightC3],
                    ButtonPurpose.CallToClose   => colors[(int)ForestTheme.Danger100],
                    ButtonPurpose.Close         => colors[(int)ForestTheme.SlateGray100],
                    ButtonPurpose.Warning       => colors[(int)ForestTheme.Warning100],
                    ButtonPurpose.Info          => colors[(int)ForestTheme.Info100],
                    ButtonPurpose.Success       => colors[(int)ForestTheme.Success100],
                    ButtonPurpose.Obsolete      => colors[(int)ForestTheme.Obsolete100],
                    _                           => colors[(int)ForestTheme.HighlightA3],
                }
            };
        }

        private void HandleNormalState()
        {
            if (!IsEnabled)
            {
                HandleDisabledState();
                return;
            }

            var purpose = Purpose;               
            var theme = ThemeSystem.Instance.Theme;

            if (purpose == ButtonPurpose.Close)
            {
                _backgroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Background]);
                _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGray100]);
            }
            else
            {
                _backgroundBrush ??= new SolidColorBrush(GetPurposeColor(purpose, 1));
                _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            }

            _bd.Background = _backgroundBrush;
            SetForeground(_foregroundBrush);
        }

        private void HandleHighlight1State(Duration duration)
        {
            //
            // Opacity 动画
            var purpose = Purpose;               
            var theme   = ThemeSystem.Instance.Theme;

            _foregroundHighlightBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight1Brush ??= new SolidColorBrush(GetPurposeColor(purpose, 2));

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundBrush.Color,
                To       = _backgroundHighlight1Brush.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlightBrush);
        }

        private void HandleHighlight2State(Duration duration)
        {
            //
            // Opacity 动画
            var purpose = Purpose;               
            var theme   = ThemeSystem.Instance.Theme;

            _foregroundHighlightBrush  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight2Brush ??= new SolidColorBrush(GetPurposeColor(purpose, 3));

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlightBrush);
        }


        private void HandleDisabledState()
        {
            var purpose = Purpose;               
            var theme   = ThemeSystem.Instance.Theme;

            if (purpose == ButtonPurpose.Close)
            {
                _backgroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Background]);
                _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGrayDisabled]);
            }
            else
            {
                _backgroundBrush ??= new SolidColorBrush(GetPurposeColor(purpose, 4));
                _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            }

            _bd.Background = _backgroundBrush;
            SetForeground(_foregroundBrush);
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x)
                  .Find<Path>(PART_IconName, x => _icon                   = x);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, Boxing.Box(value));
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public ButtonPurpose Purpose
        {
            get => (ButtonPurpose)GetValue(PurposeProperty);
            set => SetValue(PurposeProperty, value);
        }
    }
}