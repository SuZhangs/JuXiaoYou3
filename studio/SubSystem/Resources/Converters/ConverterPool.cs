namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public static class ConverterPool
    {
        public static AvatarConverter Avatar { get; } = new AvatarConverter();
        public static AlbumConverter Album { get; } = new AlbumConverter();
        public static ImageConverter Image { get; } = new ImageConverter();
        public static ScaledImageConverter ScaledImage { get; } = new ScaledImageConverter();
    }
}