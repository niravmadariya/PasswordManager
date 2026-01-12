namespace PasswordManager;

public enum SecretType
{
    Password = 0,
    Passcode = 1,
    Pin = 2
}

public class VaultItem
{
    public long Id { get; set; }
    public string Title { get; set; } = "";
    public string? Username { get; set; }
    public SecretType SecretType { get; set; }

    public byte[] SecretCipher { get; set; } = Array.Empty<byte>();
    public byte[] SecretIv { get; set; } = Array.Empty<byte>();

    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}
