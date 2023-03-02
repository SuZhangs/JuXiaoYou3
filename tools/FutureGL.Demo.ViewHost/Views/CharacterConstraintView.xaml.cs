using System;
using System.Windows.Controls;
using Acorisoft.FutureGL.Demo.ViewHost.ViewModels;
using Acorisoft.FutureGL.Forest.Attributes;

namespace ViewHost.Views
{
    [Name("人物设定")]
    [Connected(View = typeof(CharacterConstraintView), ViewModel = typeof(CharacterConstraintViewModel))]
    public partial class CharacterConstraintView : UserControl
    {
        public CharacterConstraintView()
        {
            InitializeComponent();
        }
    }
}