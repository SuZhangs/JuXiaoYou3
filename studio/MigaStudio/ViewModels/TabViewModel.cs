using System;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class TabViewModel : PageViewModel, IEquatable<TabViewModel>
    {
        public void Start(IUniquenessProtocol param)
        { 
            //
            // 第一个参数必须是ID
            Id = param.Id;
            
            OnStart(param);
        }
        
        /// <summary>
        /// 传递参数。
        /// </summary>
        /// <param name="protocol">指定要传递的参数。</param>
        protected abstract void OnStart(IUniquenessProtocol protocol);

        public bool Equals(TabViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Uniqueness ?
                other.GetType() == GetType() :
                Id == other.Id;
        }

        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TabViewModel)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// 用于表示当前的视图模型的唯一标识符。
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 用来表示当前的视图模型是否为唯一的。
        /// </summary>
        /// <remarks>
        /// <see cref="Uniqueness"/> 属性用来表示是否唯一，这个唯一是按照类型来算的。如果这个值为true，那么只能存在一个打开的类型。
        /// </remarks>
        public virtual bool Uniqueness => false;
    }
}