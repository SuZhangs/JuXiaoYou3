using System;
using System.Windows.Controls;

namespace ViewHost.Views
{
    [Connected(View = typeof(CharacterConstraintView), ViewModel = typeof(CharacterConstraintViewModel))]
    public partial class CharacterConstraintView : UserControl
    {
        public CharacterConstraintView()
        {
            InitializeComponent();
        }
    }
}