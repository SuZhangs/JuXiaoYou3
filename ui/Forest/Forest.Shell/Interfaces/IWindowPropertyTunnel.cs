namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IWindowPropertyTunnel
    {
        Action<WindowState> WindowStateTunnel { get; set; }
    }
}