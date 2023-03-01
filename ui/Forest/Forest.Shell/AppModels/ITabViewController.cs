﻿using System.Reactive;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.AppModels
{
    public interface ITabViewController : IViewController, IRootViewModel
    {
        /// <summary>
        /// 启动指定的视图模型
        /// </summary>
        /// <param name="viewModel">指定的视图模型</param>
        void Start(ITabViewModel viewModel);
    }
}