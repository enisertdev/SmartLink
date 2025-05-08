namespace SmartLinkClient.Interfaces
{
    public interface IVpnDetectorService
    {
        Task<bool> IsUsingVpn();
    }
}
