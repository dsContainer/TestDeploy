namespace DigitalSignature.Utilities.HSMServer
{
    public interface IByteSigner
    {
        byte[] Sign(byte[] input);
    }
}