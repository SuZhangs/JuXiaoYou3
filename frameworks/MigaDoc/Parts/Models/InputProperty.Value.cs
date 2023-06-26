﻿using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    public class Value :  ITextNode
    {
        private string _text;
        
        public void AddText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 获取或设置 <see cref="Text"/> 属性。
        /// </summary>
        public string Text { get; set; }
    }
}