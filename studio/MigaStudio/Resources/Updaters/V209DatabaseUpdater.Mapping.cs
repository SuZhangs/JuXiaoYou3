

using System.IO;
using System.Text;
using Acorisoft.FutureGL.MigaDB.Utils;
using Newtonsoft.Json;
using OldRepositoryProperty = Acorisoft.FutureGL.MigaStudio.Resources.Updaters.RepositoryProperty;
using NewRepositoryProperty = Acorisoft.FutureGL.MigaDB.Models.DatabaseProperty;
namespace Acorisoft.FutureGL.MigaStudio.Resources.Updaters
{
    partial class V209DatabaseUpdater
    {
        public static NewRepositoryProperty Mapping(OldRepositoryProperty property)
        {
            return new DatabaseProperty
            {
                Name        = property.Name,
                ForeignName = property.EnglishName,
                Author      = property.Author,
                Album       = new ObservableCollection<Album>(),
            };
        }
        
        
        public static T FromFile<T>(string fileName)
        {
            var rawJsonContent = File.ReadAllText(fileName, Encoding.UTF8);
            var instance       = JsonConvert.DeserializeObject<T>(rawJsonContent);
            return instance;
        }
    }
}