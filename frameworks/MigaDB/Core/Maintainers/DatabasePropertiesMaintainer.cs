﻿using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
// ReSharper disable All

namespace Acorisoft.FutureGL.MigaDB.Core.Maintainers
{
    public class DatabasePropertiesMaintainer : IDatabaseMaintainer
    {
        public void Maintain(IDatabase database)
        {
            var timeOfBoth = DateTime.Now;

            database.Upsert<DatabaseProperty>(new DatabaseProperty
            {
                Author      = "Test",
                Name        = "Test",
                ForeignName = "Test"
            });

            database.Upsert<DoubleProperty>(new DoubleProperty
            {
                Value = new Dictionary<string, double>()
            });
            
            database.Upsert<BooleanProperty>(new BooleanProperty
            {
                Value = new Dictionary<string, bool>()
            });
            
            database.Upsert<Int32Property>(new Int32Property
            {
                Value = new Dictionary<string, int>()
            });
            
            database.Upsert<StringProperty>(new StringProperty
            {
                Value = new Dictionary<string, string>()
            });

            database.Upsert<ColorServiceProperty>(new ColorServiceProperty
            {
                Mappings = new List<ColorMapping>(),
            });
            
            database.Upsert<DatabaseVersion>(new DatabaseVersion
            {
                TimeOfModified = timeOfBoth,
                TimeOfCreated  = timeOfBoth,
                Version        = Constants.MinVersion
            });
        }
    }
}