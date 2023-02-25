﻿using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Windows
{
    public abstract class TabBaseWindow<TViewModel> : ForestWindow where TViewModel : TabBased
    {
        private TViewModel _dc;

        protected override void OnApplyStyle()
        {
            Style ??= Application.Current.Resources["TabBaseWindow"] as Style;
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            _dc = Xaml.Get<TViewModel>();
            
            //
            // 设置上下文
            DataContext = _dc;

            //
            //
            Xaml.Get<ViewHostServiceAdapter>().Controller = _dc.Controller;
            
            //
            // 启动
            _dc.Start();
            
            //
            base.OnLoaded(sender, e);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            base.OnUnloaded(sender, e);
        }


        public TViewModel ViewModel => _dc;
    }
}