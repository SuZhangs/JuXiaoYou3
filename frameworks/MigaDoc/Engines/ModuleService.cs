// ReSharper disable ConvertToAutoPropertyWhenPossible

using Acorisoft.Miga.Doc.Networks;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ModuleService : DirectoryService, IBinaryDifferenceProvider
    {
        private static readonly DataPartReader _reader = new DataPartReader();
        private static readonly DataPartWriter _writer = new DataPartWriter();

        public ModuleService() : base(Constants.folder_modules)
        {
            Collection = new SourceList<ModuleIndex>();
        }

        public static async Task<Module> ReadFromAsync(string fileName)
        {
            var compilation = await _reader.ReadFromAsync(fileName);
            return compilation.Result;
        }
        
        public static Module ReadFromAsync(byte[] buffer)
        {
            var compilation = _reader.ReadFromAsync(buffer);
            return compilation.Result;
        }

        /// <summary>
        /// 重建所有模组
        /// </summary>
        public async Task Rebuilds()
        {
            var fileNames = System.IO.Directory.GetFiles(Directory, "*.png");
            Collection.Clear();
            Database.DeleteAll();
            DetailDatabase.DeleteAll();

            foreach(var fileName in fileNames)
            {
                var module = await ReadFromAsync(fileName);
                var info = new FileInfo(fileName);
                var overwritten = false;
                var index = new ModuleIndex
                {
                    Id = Guid.Parse(module.Id),
                    Author = module.Author,
                    Organization = module.Organization,
                    Name = module.Name,
                    FileName = info.Name,
                    Type = module.Type,
                    Version = module.Version
                };

                if (Database.Contains(index.Id))
                {
                    var oldOne = Database.FindById(index.Id);
                    if (oldOne.Version >= module.Version)
                    {
                        return;
                    }

                    overwritten = true;
                }

                if (!overwritten)
                {
                    Collection.Add(index);
                    Database.Insert(index);
                    var md = RebuildMeowMeowSpell(module);
                    DetailDatabase.Insert(md);
                    return;
                }

                Database.Update(index);
            }
        }

        #region MeowMeowSpell

        

        public static string GetMeowMeowSpell(IEnumerable<MetadataDetail> array)
        {
            var sb = new StringBuilder();
            foreach (var detail in array)
            {
                sb.Append(detail.Name);
                sb.Append('：');
                sb.Append(detail.Metadata);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public static IEnumerable<MetadataDetail> RebuildMeowMeowSpell(Module module)
        {
            if (module is null)
            {
                return null;
            }

            var array = new List<MetadataDetail>(16);

            foreach (var property in module.Items)
            {
                if (property is GroupProperty group)
                {
                    RebuildMeowMeowSpellFromGroupProperty(array, group);
                }
                else
                {
                    RebuildMeowMeowSpellFromProperty(array, property);
                }
            }

            return array;
        }
        
        private static void RebuildMeowMeowSpellFromGroupProperty(ICollection<MetadataDetail> md, GroupProperty property)
        {
            if (string.IsNullOrEmpty(property.Metadata))
            {
                return;
            }
            md.Add(new MetadataDetail
            {
                Metadata = property.Metadata,
                Name = property.Name
            });

            foreach (var prop in property.Values)
            {
                RebuildMeowMeowSpellFromProperty(md, prop);
            }
        }
        
        private static void RebuildMeowMeowSpellFromProperty(ICollection<MetadataDetail> md, InputProperty property)
        {
            if (string.IsNullOrEmpty(property.Metadata))
            {
                return;
            }
            md.Add(new MetadataDetail
            {
                Metadata = property.Metadata,
                Name     = property.Name
            });
        }


        #endregion
        
        /// <summary>
        /// 添加模组
        /// </summary>
        /// <param name="fileName">模组路径</param>
        public Task NewModules(string fileName, byte[] buffer)
        {
            return Task.Run(() =>
            {
                var module = ReadFromAsync(buffer);
                var info = new FileInfo(fileName);
                var overwritten = false;
                var index = new ModuleIndex
                {
                    Id           = Guid.Parse(module.Id),
                    Author       = module.Author,
                    Organization = module.Organization,
                    Name         = module.Name,
                    FileName     = info.Name,
                    Type         = module.Type,
                    Version      = module.Version
                };
                var dst = Path.Combine(Directory, index.FileName);

                if (Database.Contains(index.Id))
                {
                    var oldOne = Database.FindById(index.Id);
                    if (oldOne.Version >= module.Version)
                    {
                        return;
                    }

                    overwritten = true;
                }

                File.WriteAllBytes(dst, buffer);

                if (!overwritten)
                {
                    Collection.Add(index);
                    Database.Insert(index);
                    var md = RebuildMeowMeowSpell(module);
                    DetailDatabase.Insert(md);
                    return;
                }

                Database.Update(index);
            });
        }
        
        /// <summary>
        /// 添加模组
        /// </summary>
        /// <param name="fileName">模组路径</param>
        public Task NewModules(string fileName)
        {
            return Task.Run(async () =>
            {
                var module = await ReadFromAsync(fileName);
                var info = new FileInfo(fileName);
                var overwritten = false;
                var index = new ModuleIndex
                {
                    Id           = Guid.Parse(module.Id),
                    Author       = module.Author,
                    Organization = module.Organization,
                    Name         = module.Name,
                    FileName     = info.Name,
                    Type         = module.Type,
                    Version = module.Version
                };
                var dst = Path.Combine(Directory, index.FileName);

                if (Database.Contains(index.Id))
                {
                    var oldOne = Database.FindById(index.Id);
                    if (oldOne.Version >= module.Version)
                    {
                        return;
                    }

                    overwritten = true;
                }

                File.Copy(fileName, dst, true);

                if (!overwritten)
                {
                    Collection.Add(index);
                    Database.Insert(index);
                    var md = RebuildMeowMeowSpell(module);
                    DetailDatabase.Insert(md);
                    return;
                }

                Database.Update(index);
            });
        }

        /// <summary>
        /// 添加模组
        /// </summary>
        /// <param name="fileNames">模组路径</param>
        public async Task NewModules(string[] fileNames)
        {
            if (fileNames is null || fileNames.Length == 0)
            {
                return;
            }
            
            foreach (var fileName in fileNames)
            {
                await NewModules(fileName);
            }
        }

        /// <summary>
        /// 删除模组l
        /// </summary>
        /// <param name="index">模组索引</param>
        public Task RemoveModules(ModuleIndex index)
        {
            return Task.Run(() =>
            {
                if (index is null)
                {
                    return;
                }

                var dst = Path.Combine(Directory, index.FileName);
                File.Delete(dst);
                Database.Delete(index.Id);
                Collection.Remove(index);
            });
        }

        /// <summary>
        /// 清空模组
        /// </summary>
        public Task ClearModules()
        {
            return Task.Run(() =>
            {
                foreach (var file in System.IO.Directory.GetFiles(Directory))
                {
                    File.Delete(file);
                }

                Database.DeleteAll();
                Collection.Clear();
            });
        }
        public void Resolve(Transaction transaction)
        {
            if (transaction is not BinaryTransaction bt) return;
            var buffer = Convert.FromBase64String(bt.Base64);
            NewModules(Path.Combine(Directory,bt.FileName), buffer);
        }

        public void Process(Transaction transaction)
        {
            if (transaction is not BinaryTransaction bt) return;
            var buffer = File.ReadAllBytes(Path.Combine(Directory,bt.FileName));
            bt.Base64 = Convert.ToBase64String(buffer);
        }
        
        public List<BinaryDescription> GetDescriptions()
        {
            return System.IO.Directory.GetFiles(Directory).Select(x =>
            {
                var fi = new FileInfo(x);
                return new BinaryDescription { FileName = fi.Name };
            }).ToList();
        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            base.OnRepositoryOpening(context, property);
            Database       = context.Database.GetCollection<ModuleIndex>(Constants.cn_modules);
            DetailDatabase = context.Database.GetCollection<MetadataDetail>(Constants.cn_meow);
            Collection.AddRange(Database.FindAll());
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            base.OnRepositoryClosing(context);

            Collection.Clear();
            Database = null;
        }

        public DataPartReader Reader => _reader;
        public DataPartWriter Writer => _writer;
        public SourceList<ModuleIndex> Collection { get; }
        public ILiteCollection<ModuleIndex> Database { get; private set; }
        public ILiteCollection<MetadataDetail> DetailDatabase { get; private set; }
    }
}