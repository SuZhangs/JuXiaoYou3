﻿using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Acorisoft.FutureGL.MigaUtils
{
    public class Pool
    {
        private static readonly DefaultObjectPoolProvider ObjectPoolProvider;
        private static readonly ObjectPool<StringBuilder> StringBuilderPool;

        static Pool()
        {
            ObjectPoolProvider = new DefaultObjectPoolProvider();
            StringBuilderPool  = ObjectPoolProvider.CreateStringBuilderPool(32, 128);
        }

        public static StringBuilder GetStringBuilder() => StringBuilderPool.Get();
        public static void ReturnStringBuilder(StringBuilder sb) => StringBuilderPool.Return(sb);
    }
}