
namespace Acorisoft.Miga.Doc.Parts
{
    public class DataPartCollection : List<DataPart>
    {
        public T GetPart<T>() where T : DataPart
        {
            var index = FindIndex(x => x is T);
            if (index < 0)
            {
                var part = Classes.CreateInstance<T>();
                Add(part);
                return part;
            }
            return (T)base[index];
        }
    }
}