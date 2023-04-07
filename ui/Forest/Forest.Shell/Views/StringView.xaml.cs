﻿using System.Threading.Tasks;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.Views
{
    public partial class StringView:ForestUserControl 
    {
        public StringView()
        {
            InitializeComponent();
        }
    }

    public sealed class StringViewModel : ImplicitDialogVM
    {
        private string     _text;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            Title = parameter.Args[0]?.ToString();
            base.OnStart(parameter);
        }

        protected override bool IsCompleted() => !string.IsNullOrEmpty(_text);

        protected override void Finish()
        {
            Result = _text;
        }

        protected override string Failed()
        {
            return "值为空";
        }


        /// <summary>
        /// 获取或设置 <see cref="Text"/> 属性。
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                SetValue(ref _text, value);
            }
        }
        
        public static Task<Op<string>> String(string title) => DialogService()
                                                       .Dialog<string>(new StringViewModel(), new RoutingEventArgs
                                                       {
                                                           Args = new []{ title }
                                                       });
    }
}