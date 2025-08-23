public class DigitalSignature
{
    public bool VerifyHash(string originalHash, string newHash)
    {
        return originalHash == newHash;
    }

    public bool VerifyMessage(string originalMessage, string newMessage, string originalHash, string newHash)
    {
        return originalMessage == newMessage && VerifyHash(originalHash, newHash);
    }
}