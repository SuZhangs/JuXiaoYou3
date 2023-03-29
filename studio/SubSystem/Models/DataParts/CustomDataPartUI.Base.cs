using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaStudio.Models.DataParts
{
    public interface ICustomDataPartUI : INotifyPropertyChanged
    {
        
    }

    public abstract class CustomDataPartUI : ObservableObject, ICustomDataPartUI
    {
        
    }
}