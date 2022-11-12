namespace DigitalSignature.Utilities.Exceptions;

public class UnauthorizedException : HandledException
{
    public UnauthorizedException() : base(401, "Unauthorized")
    {
    }
}