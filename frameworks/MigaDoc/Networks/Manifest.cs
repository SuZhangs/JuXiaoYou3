namespace Acorisoft.Miga.Doc.Networks
{
    public class Manifest
    {
        public List<BinaryDescription> Image { get; set; }
        public List<BinaryDescription> Modules { get; set; }
        public List<DirectlyDescription> Directly { get; set; }
        public List<DifferenceDescription> Difference { get; set; }

        private static List<BinaryTransaction> ComputeModulesDiff(Manifest source, Manifest target)
        {
            var binaryExceptDictionary = new HashSet<string>(13);
            //
            // build binary mapping
            foreach (var binary in source.Modules)
            {
                binaryExceptDictionary.Add(binary.FileName);
            }

            //
            // compare binary diff
            var binaryDiffCollection = target.Modules.Where(binary => !binaryExceptDictionary.Contains(binary.FileName)).Select(x => new BinaryTransaction
            {
                FileName = x.FileName
            }).ToList();

            return binaryDiffCollection;
        }

        private static List<BinaryTransaction> ComputeBinaryDiff(Manifest source, Manifest target)
        {
            var binaryExceptDictionary = new HashSet<string>(13);
            //
            // build binary mapping
            foreach (var binary in source.Image)
            {
                binaryExceptDictionary.Add(binary.FileName);
            }

            //
            // compare binary diff
            var binaryDiffCollection = target.Image.Where(binary => !binaryExceptDictionary.Contains(binary.FileName)).Select(x => new BinaryTransaction
            {
                FileName = x.FileName
            }).ToList();

            return binaryDiffCollection;
        }

        private static List<DirectlyTransaction> ComputeDirectlyDiff(Manifest source, Manifest target)
        {
            var directlyCompareDictionary = new Dictionary<string, DirectlyDescription>(13);
            var directlyDiffCollection = new List<DirectlyTransaction>(512); //
            // build directly mapping
            foreach (var directly in source.Directly)
            {
                directlyCompareDictionary.TryAdd(directly.Id, directly);
            }


            //
            // build directly diff
            foreach (var directly in target.Directly)
            {
                if (directlyCompareDictionary.ContainsKey(directly.Id))
                {
                    directlyCompareDictionary.Remove(directly.Id);
                }
                else
                {
                    directlyDiffCollection.Add(new DirectlyTransaction
                    {
                        Id    = directly.Id,
                        State = ObjectState.Removed
                    });
                }
            }

            directlyDiffCollection.AddRange(directlyCompareDictionary.Values.Select(directly => new DirectlyTransaction
            {
                Id    = directly.Id,
                State = ObjectState.Added
            }));

            return directlyDiffCollection;
        }

        private static List<DifferenceTransaction> ComputeComposeDiff(Manifest source, Manifest target)
        {
            var composeCompareDictionary = new Dictionary<string, DifferenceDescription>(13);
            var composeDiffCollection = new List<DifferenceTransaction>(512);

            //
            // build compose diff
            foreach (var description in source.Difference)
            {
                composeCompareDictionary.TryAdd(description.Id, description);
            }

            foreach (var description in target.Difference)
            {
                if (composeCompareDictionary.TryGetValue(description.Id, out var sourceDescription))
                {
                    ComputeComposeDiff(
                        description.Id,
                        sourceDescription.Draft,
                        description.Draft,
                        composeDiffCollection,
                        DifferencePosition.Draft);

                    ComputeComposeDiff(
                        description.Id,
                        sourceDescription.RecycleBin,
                        description.RecycleBin,
                        composeDiffCollection,
                        DifferencePosition.Recycle);

                    // xxxChanged = oldCount == newCount
                    //

                    if (description.CurrentHash != sourceDescription.CurrentHash)
                    {
                        composeDiffCollection.Add(new DifferenceTransaction
                        {
                            Id         = description.Id,
                            Position   = DifferencePosition.Current,
                            Difference = Networks.Difference.Replace
                        });
                    }

                    composeCompareDictionary.Remove(description.Id);
                }
                else
                {
                    composeDiffCollection.Add(new DifferenceTransaction
                    {
                        Id         = description.Id,
                        Difference = Networks.Difference.Removed,
                        Position   = DifferencePosition.Compose
                    });
                }
            }

            composeDiffCollection.AddRange(composeCompareDictionary.Values.Select(description => new DifferenceTransaction
            {
                Id         = description.Id,
                Difference = Networks.Difference.Added,
                Position   = DifferencePosition.Compose
            }));

            return composeDiffCollection;
        }

        private static void ComputeComposeDiff(
            string parentId,
            IEnumerable<Tuple<string, string>> source,
            IEnumerable<Tuple<string, string>> target,
            IList<DifferenceTransaction> composeDiffCollection,
            DifferencePosition position)
        {
            // var count = composeDiffCollection.Count;

            //
            // 比较数值
            var draftCompareDictionary = source.ToDictionary(draft => draft.Item1, draft => draft.Item2);
            //
            // full compare

            //
            // item1 == hash
            // item2 == id

            foreach (var draft in target.Where(draft => !draftCompareDictionary.ContainsKey(draft.Item1)))
            {
                composeDiffCollection.Add(new DifferenceTransaction
                {
                    Id         = parentId,
                    SubId      = draft.Item2,
                    Difference = Networks.Difference.Replace,
                    Position   = DifferencePosition.Draft
                });
                draftCompareDictionary.Remove(draft.Item1);
            }

            composeDiffCollection.AddRange(draftCompareDictionary.Values.Select(draft => new DifferenceTransaction
            {
                Id         = parentId,
                SubId      = draft,
                Difference = Networks.Difference.Replace,
                Position   = position
            }));

            foreach (var removed in draftCompareDictionary.Values)
            {
                composeDiffCollection.Add(new DifferenceTransaction
                {
                    Id         = parentId,
                    SubId      = removed,
                    Difference = Networks.Difference.Removed,
                    Position   = position
                });
            }

            // return count == composeDiffCollection.Count;
        }


        public static Task<TransactionManifest> GetDifferenceManifest(Manifest source, Manifest target)
        {
            return Task.Run(() => new TransactionManifest
            {
                Module     = ComputeModulesDiff(source, target),
                Image      = ComputeBinaryDiff(source, target),
                Difference = ComputeComposeDiff(source, target),
                Directly   = ComputeDirectlyDiff(source, target)
            });
        }

        public static Manifest Create() => new Manifest
        {
            Modules    = new List<BinaryDescription>(64),
            Image      = new List<BinaryDescription>(64),
            Difference = new List<DifferenceDescription>(512),
            Directly   = new List<DirectlyDescription>(512)
        };
    }
}