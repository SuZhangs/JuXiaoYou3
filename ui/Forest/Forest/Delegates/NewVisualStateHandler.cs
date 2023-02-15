using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.Forest.Delegates
{
    public delegate void NewVisualStateHandler(bool init, VisualState last, VisualState now, VisualStateTrigger value);
}