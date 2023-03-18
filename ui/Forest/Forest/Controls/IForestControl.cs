namespace Acorisoft.FutureGL.Forest.Controls
{
    public interface IForestControl : ITextResourceAdapter
    {
        void InvalidateState();
        
        /// <summary>
        /// 调色板
        /// </summary>
        HighlightColorPalette Palette { get; }
    }
}