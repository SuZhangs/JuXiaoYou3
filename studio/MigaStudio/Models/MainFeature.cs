﻿using System;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class MainFeature
    {
        public static async Task Run(
            MainFeature feature, 
            Func<IDialogService> DialogService,
            TabController Controller,
            IObserver<ViewModelBase> onStart)
        {
            if (feature is null)
            {
                return;
            }

            if (feature.IsDialog)
            {
                await DialogService().Dialog(Xaml.GetViewModel<IDialogViewModel>(feature.ViewModel));
                return;
            }

            
            var vm = Controller.Start(feature.ViewModel, new Parameter
            {
                Args = feature.Parameter
            });
            feature.Cache ??= vm;
            onStart.OnNext(feature.Cache);
        }
        
        public string GroupId { get; init; }
        public string NameId { get; init; }
        public bool IsDialog { get; init; }
        public Type ViewModel { get; init; }
        public ViewModelBase Cache { get; set; }
        public object[] Parameter { get; init; }
    }
}