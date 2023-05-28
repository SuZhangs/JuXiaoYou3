namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public static class ConverterPool
    {
        public static AvatarConverter Avatar { get; } = new AvatarConverter();
        public static AlbumConverter Album { get; } = new AlbumConverter();
        public static ImageConverter Image { get; } = new ImageConverter();
        public static ScaledImageConverter Image4x3 { get; } = new ScaledImageConverter(4, 3);
        public static ScaledImageConverter Image16x9 { get; } = new ScaledImageConverter(16, 9);
        public static ScaledImageConverter Image9x16 { get; } = new ScaledImageConverter(9, 16);
    }
}