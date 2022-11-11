namespace Digital.Infrastructure.Utilities.HSMServer
{
    public interface IByteSigner
    {
        byte[] Sign(byte[] input);
    }
}