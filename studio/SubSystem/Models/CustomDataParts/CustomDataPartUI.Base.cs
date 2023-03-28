using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaStudio.Models.CustomDataParts
{
    public interface ICustomDataPartUI : INotifyPropertyChanged
    {
        
    }

    public abstract class CustomDataPartUI : ObservableObject, ICustomDataPartUI
    {
        
    }
}