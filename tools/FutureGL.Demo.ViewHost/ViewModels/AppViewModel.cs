﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Acorisoft.FutureGL.Demo.ViewHost.Models;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.Demo.ViewHost.ViewModels
{
    public class AppViewModel : AppViewModelBase
    {
        public AppViewModel()
        {
            var demos = GetType()
                .Assembly
                .GetTypes()
                .Where(x => x.IsClass && x.IsPublic && x.IsDefined(typeof(NameAttribute), false))
                .Select(x =>
                {
                    var na = x.GetCustomAttribute<NameAttribute>();
                    return new NameDemo
                    {
                        Name = na?.Name,
                        Type = x
                    };
                });
            Demos = new ObservableCollection<NameDemo>(demos);
        }

        private NameDemo _demo;
        private IViewModel   _viewModel;

        /// <summary>
        /// 获取或设置 <see cref="ViewModel"/> 属性。
        /// </summary>
        public IViewModel ViewModel
        {
            get => _viewModel;
            set => SetValue(ref _viewModel, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Demo"/> 属性。
        /// </summary>
        public NameDemo Demo
        {
            get => _demo;
            set
            {
                SetValue(ref _demo, value);
                if (value is null)
                {
                    return;
                }
                ViewModel = Xaml.GetViewModel<IViewModel>(value.Type);
            }
        }

        public ObservableCollection<NameDemo> Demos { get; }
    }
}