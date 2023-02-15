using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ImTools;

namespace Acorisoft.FutureGL.MigaStudio.ModuleSystem
{
    public abstract class CounterPropertyDataView<TProperty> : PropertyDataView<TProperty, int>, IBindableClampSink where TProperty : ModuleProperty, IClampSink
    {
        protected CounterPropertyDataView(TProperty property) : base(property)
        {
            //
            // 请检查是否有需要绑定属性的赋值。
            // 
            // 注意:
            // Maximum 和 Minimum属性不需要绑定吧
        }

        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string GenericValueToString(int value) => value.ToString();

        /// <summary>
        /// 转成整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int StringToGenericValue(string value) => Math.Clamp(value.ToInt(Minimum), Minimum, Maximum);

        protected override int OnValueChanged(int oldValue, int newValue, out bool fallback)
        {
            var val2 = Math.Clamp(newValue, Minimum, Maximum);
            fallback = val2 != newValue;
            return val2;
        }

        /// <summary>
        /// 获取或设置 <see cref="Minimum"/> 属性。
        /// </summary>
        public int Minimum => 0;

        /// <summary>
        /// 获取或设置 <see cref="Maximum"/> 属性。
        /// </summary>
        public int Maximum => 10;
    }

    public abstract class CounterPropertyEditView<TProperty> : PropertyEditView<TProperty, int>, IClampSink where TProperty : ModuleProperty, IClampSink
    {
        private int _maximum;
        private int _minimum;

        protected CounterPropertyEditView(TProperty property) : base(property)
        {
            //
            // 请检查是否有Value的显式赋值
            // 请检查是否有需要绑定属性的赋值。
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string ConstructFallback(int value) => Math.Clamp(value, Minimum, Maximum).ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override int DeconstructFallback(string value) => Math.Clamp(value.ToInt(Minimum), Minimum, Maximum);


        /// <summary>
        /// 完成编辑并创建。
        /// </summary>
        /// <returns>返回一个完成编辑的新设定属性。</returns>
        protected override TProperty FinishState()
        {
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            TargetProperty.Fallback = ConstructFallback(Fallback);

            return TargetProperty;
        }

        /// <summary>
        /// 是否完成
        /// </summary>
        /// <returns></returns>
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name) && Fallback <= 10 && Fallback >= 0;


        /// <summary>
        /// 获取或设置 <see cref="Minimum"/> 属性。
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set => SetValue(ref _minimum, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Maximum"/> 属性。
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }
    }

    public class LikabilityPropertyDataView : CounterPropertyDataView<LikabilityProperty>
    {
        public LikabilityPropertyDataView(LikabilityProperty property) : base(property)
        {
        }
    }

    public class LikabilityPropertyEditView : CounterPropertyEditView<LikabilityProperty>
    {
        public LikabilityPropertyEditView(LikabilityProperty property) : base(property)
        {
        }


        /// <summary>
        /// 完成操作
        /// </summary>
        /// <returns>返回新的实例</returns>
        protected sealed override LikabilityProperty FinishState()
        {
            TargetProperty.Maximum = Maximum;
            TargetProperty.Minimum = Minimum;

            return base.FinishState();
        }
    }

    public class RatePropertyDataView : CounterPropertyDataView<RateProperty>
    {
        public RatePropertyDataView(RateProperty property) : base(property)
        {
        }
    }

    public class RatePropertyEditView : CounterPropertyEditView<RateProperty>
    {
        public RatePropertyEditView(RateProperty property) : base(property)
        {
        }


        /// <summary>
        /// 完成操作
        /// </summary>
        /// <returns>返回新的实例</returns>
        protected sealed override RateProperty FinishState()
        {
            TargetProperty.Maximum = Maximum;
            TargetProperty.Minimum = Minimum;

            return base.FinishState();
        }
    }

    public class GroupPropertyDataView : PropertyDataView
    {
        private readonly GroupProperty _target;

        private string _name;
        private string _metadata;

        public GroupPropertyDataView(GroupProperty property)
        {
            TargetProperty = property;
            Items          = new ObservableCollection<PropertyDataView>();

            if (property.Items is not null)
            {
                Items.AddRange(property.Items.Select(Create));
            }
        }


        /// <summary>
        /// 获取默认值或者回滚。
        /// </summary>
        /// <returns>返回默认值或者回滚</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string GetValueOrFallback() => string.IsNullOrEmpty(_target.Value) ? _target.Fallback : _target.Value;

        /// <summary>
        /// 获取或设置 <see cref="Metadata"/> 属性。
        /// </summary>
        public string Metadata
        {
            get => _metadata;
            private set => SetValue(ref _metadata, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            private set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 目标属性。
        /// </summary>
        public GroupProperty TargetProperty
        {
            get { return _target; }

            private init
            {
                _target   = value;
                _name     = _target.Name;
                _metadata = _target.Metadata;
            }
        }

        public ObservableCollection<PropertyDataView> Items { get; }
    }

    public class GroupPropertyEditView : PropertyEditView
    {
        private readonly GroupProperty _target;

        public GroupPropertyEditView(GroupProperty property)
        {
            TargetProperty = property;
            Items          = new ObservableCollection<ModuleProperty>();

            //
            //
            property.Items ??= Array.Empty<ModuleProperty>();

            //
            //
            if (property.Items is not null)
            {
                Items.AddRange(property.Items);
            }
        }


        /// <summary>
        /// 完善所有的属性
        /// </summary>
        /// <returns>返回一个完整的设定属性实例。</returns>
        public sealed override ModuleProperty Finish()
        {
            TargetProperty.Items    = Items.ToArray();
            TargetProperty.Name     = Name;
            TargetProperty.Metadata = Metadata;
            return TargetProperty;
        }

        /// <summary>
        /// 是否完善。
        /// </summary>
        /// <returns>返回一个值，true表示完善，否则为false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name);


        /// <summary>
        /// 目标属性。
        /// </summary>
        public GroupProperty TargetProperty
        {
            get { return _target; }

            private init
            {
                _target  = value;
                Name     = _target.Name;
                Metadata = _target.Metadata;
            }
        }

        public ObservableCollection<ModuleProperty> Items { get; }
    }
}