﻿namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfRel : PartOfDetailPlaceHolder
    {
        public PartOfRel()
        {
            Id = Constants.IdOfRelationship_CharacterPart;
        }


        public sealed override bool Removable => false;
    }
}