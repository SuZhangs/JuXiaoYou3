using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.Forest
{
    public class Language
    {
        /// <summary>
        /// 指定文化
        /// </summary>
        /// <remarks>语言的切换必须重启。</remarks>
        public static CultureArea Culture { get; set; }
        
        /// <summary>
        /// 确定
        /// </summary>
        public static string ConfirmText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Ok",
                    
                    _ => "确定"
                };
            }
        }
        
        /// <summary>
        /// 拒绝
        /// </summary>
        public static string RejectText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Reject",
                    
                    _ => "拒绝"
                };
            }
        }
        
        /// <summary>
        /// 拒绝
        /// </summary>
        public static string CancelText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Cancel",
                    
                    _ => "放弃"
                };
            }
        }
    }
}