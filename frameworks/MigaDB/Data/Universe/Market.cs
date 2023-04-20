namespace Acorisoft.FutureGL.MigaDB.Data.Universe
{
    public enum CommodityState
    {
        /* 有货（货少），有货（正常），有货（货多），无货（不能销售），无货（商品短缺）*/
        /// <summary>
        /// 缺货
        /// </summary>
        StockOut,

        /// <summary>
        /// 物资短缺
        /// </summary>
        Shortage,

        /// <summary>
        /// 供过于求
        /// </summary>
        Oversupply,
    }

    public sealed class MarketPrice : StorageUIObject
    {
        private decimal        _price;
        private string         _item;
        private CommodityState _state;

        /// <summary>
        /// 获取或设置 <see cref="State"/> 属性。
        /// </summary>
        public CommodityState State
        {
            get => _state;
            set => SetValue(ref _state, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Item"/> 属性。
        /// </summary>
        public string Item
        {
            get => _item;
            set => SetValue(ref _item, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Price"/> 属性。
        /// </summary>
        public decimal Price
        {
            get => _price;
            set => SetValue(ref _price, value);
        }
    }

    public class Market : StorageUIObject
    {
        
    }
}