using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

namespace Acorisoft.FutureGL.MigaDB.Interfaces
{
    public interface IValueEquatable
    {
        bool CompareValue(ModuleBlock block);
    }
    
    public interface ITemplateEquatable
    {
        bool CompareTemplate(ModuleBlock block);
    }
}