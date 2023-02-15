namespace Acorisoft.FutureGL.Forest
{
    public class Boxing
    {
        #region Boolean
        
        /// <summary>
        /// True的装箱值。
        /// </summary>
        public static readonly object True  = true;
        
        /// <summary>
        /// False的装箱值。
        /// </summary>
        public static readonly object False = false;

        /// <summary>
        /// 获取布尔类型的装箱值。
        /// </summary>
        /// <param name="value">指定要装箱的值</param>
        /// <remarks>使用该方法来避免过多的装箱值类型。</remarks>
        /// <returns>返回布尔类型的装箱值</returns>
        public static object Box(bool value) => value ? True : False;

        #endregion
    }
}