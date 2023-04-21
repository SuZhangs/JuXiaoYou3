using System.IO;
using System.Linq;
using TagLib;
using TagLib.Mpeg;
using File = TagLib.File;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class MusicUtilities
    {
        private static string CombineString(IEnumerable<string> source)
        {
            const string pattern = "{0},{1}";
            return source.Aggregate((a, b) => string.Format(pattern, a, b));
        }
        
        public static async Task AddMusic(IEnumerable<string> fileNames, MusicEngine engine, Action<Music> callback)
        {
            foreach (var fileName in fileNames)
            {
                Music music;
                var   fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var   fileNameWithExtension    = fileNameWithoutExtension + ".mp3";

                if (engine.HasFile(fileNameWithExtension))
                {
                    music = engine.GetFile(fileNameWithExtension);
                }
                else
                {
                    var file      = File.Create(fileName);
                    var musicFile = (AudioFile)file;
                    var tag       = musicFile.GetTag(TagTypes.Id3v2);
                    var cover     = Path.GetFileNameWithoutExtension(fileName) + ".png";

                    if (tag.Pictures is not null &&
                        tag.Pictures?.Length > 0)
                    {
                        var pic = tag.Pictures.First();
                        cover = Path.GetFileNameWithoutExtension(fileName) + ".png";
                        await engine.WriteAlbum(pic.Data.Data, cover);
                    }

                    var title      = string.IsNullOrEmpty(tag.Title) ? fileNameWithoutExtension : tag.Title;
                    var performers = CombineString(tag.Performers);
                    music = new Music
                    {
                        Id     = fileNameWithExtension,
                        Path   = fileName,
                        Name   = title,
                        Author = performers,
                        Cover  = cover
                    };

                    engine.AddFile(music);
                }

                //
                //
                callback(music);
            }
        }
    }
}