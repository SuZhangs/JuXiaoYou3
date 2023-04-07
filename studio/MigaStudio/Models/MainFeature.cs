using System;
using Acorisoft.FutureGL.Forest;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class MainFeature
    {
        public string GroupId { get; init; }
        public string NameId { get; init; }
        public bool IsGallery { get; init; }
        public bool IsDialog { get; init; }
        public Type Gallery { get; init; }
        public object[] Parameter { get; init; }
        public Type ViewModel { get; init; }
    }
}