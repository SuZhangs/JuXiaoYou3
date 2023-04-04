namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class KeyValueViewModel : ViewModelBase
    {
        public string this[string key, Dictionary<string, string> dictionary]
        {
            get
            {
                if (!dictionary.TryGetValue(key, out var value))
                {
                    value = string.Empty;
                    dictionary.TryAdd(key, value);
                }

                return value;
            }
            set
            {
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                }
                else
                {
                    dictionary.TryAdd(key, value);
                }
            }
        }
    }
}