﻿namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations
{
    public class PresentationDegreeData : ObservableObject, IPresentationData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        public string ValueSourceID { get; init; }
        public bool IsMetadata { get; init; }
    }
}