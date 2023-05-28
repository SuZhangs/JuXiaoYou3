using DEDrake;

namespace Acorisoft.Miga.Doc
{
    public static class ShortGuidString
    {
        public static string GetId() => ShortGuid.NewGuid().ToString();
    }
}