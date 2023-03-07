﻿using System;
using System.Windows.Markup;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Markup
{
    public class TextExtension : MarkupExtension
    {
        public TextExtension(){}

        public TextExtension(string id) => Id = id;
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return StringFromCode.GetText(Id);
        }
        
        public string Id { get; set; }
    }
}