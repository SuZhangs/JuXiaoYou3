using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels.CustomDataParts
{
    public interface ICustomDataPartUI : INotifyPropertyChanged
    {
        
    }

    public abstract class CustomDataPartUI : ObservableObject, ICustomDataPartUI
    {
        
    }
}