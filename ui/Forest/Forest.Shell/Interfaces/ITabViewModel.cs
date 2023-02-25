
namespace Acorisoft.FutureGL.Forest.Interfaces
{
    
    public interface ITabViewModel :IRootViewModel
    {
        string Id { get; }
        bool Initialized { get; }
        string Title { get; }
    }
}