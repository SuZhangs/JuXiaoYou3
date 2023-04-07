using System.Globalization;

namespace Acorisoft.FutureGL.MigaUtils
{
    public interface ITextService
    {
        bool UseLanguageService();
        string GetTextSource();
    }
}