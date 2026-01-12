namespace PasswordManager
{
    public sealed class VaultService
    {
        private readonly VaultRepository _repo;
        private readonly CryptoService _crypto;
        private byte[]? _key;

        public VaultService(VaultRepository repo, CryptoService crypto)
        {
            _repo = repo;
            _crypto = crypto;
        }

        public bool IsInitialized() => _repo.GetKeyDerivation() != null;

        public void InitializeNewVault(string masterPassword)
        {
            if (IsInitialized())
                throw new InvalidOperationException("Vault already initialized.");

            var salt = _crypto.GenerateRandomSalt();
            var key = _crypto.DeriveKey(masterPassword, salt, CryptoService.Iterations);

            _repo.InsertKeyDerivation(salt, CryptoService.Iterations);
            _key = key;
        }

        public bool OpenVault(string masterPassword)
        {
            var kd = _repo.GetKeyDerivation();
            if (kd is null) return false;

            var (salt, iterations) = kd.Value;
            var key = _crypto.DeriveKey(masterPassword, salt, iterations);
            _key = key;

            // Optionally, you can try decrypting one sample record or a fixed test string here
            return true;
        }

        private byte[] RequireKey()
        {
            if (_key == null)
                throw new InvalidOperationException("Vault not opened.");
            return _key;
        }

        public void UpdateItem(long id, string newTitle, string? newUsername, string newSecret)
        {
            var key = RequireKey();
            var (cipher, iv) = _crypto.EncryptString(newSecret, key);
            var now = DateTime.UtcNow;

            var existing = _repo.GetVaultItem(id);
            if (existing == null) throw new InvalidOperationException("Item not found.");

            existing.Title = newTitle;
            existing.Username = newUsername;
            existing.SecretCipher = cipher;
            existing.SecretIv = iv;
            existing.UpdatedUtc = now;

            _repo.UpdateVaultItem(existing);
        }


        public long AddItem(string title, string? username, SecretType type, string secret)
        {
            var key = RequireKey();

            var (cipher, iv) = _crypto.EncryptString(secret, key);
            var now = DateTime.UtcNow;

            var item = new VaultItem
            {
                Title = title,
                Username = username,
                SecretType = type,
                SecretCipher = cipher,
                SecretIv = iv,
                CreatedUtc = now,
                UpdatedUtc = now
            };

            return _repo.InsertVaultItem(item);
        }

        public IEnumerable<(VaultItem meta, string secret)> GetAllDecrypted()
        {
            var key = RequireKey();
            foreach (var item in _repo.GetAllVaultItems())
            {
                var secret = _crypto.DecryptString(item.SecretCipher, item.SecretIv, key);
                yield return (item, secret);
            }
        }

        public string? GetSecret(long id)
        {
            var key = RequireKey();
            var item = _repo.GetVaultItem(id);
            if (item == null) return null;
            return _crypto.DecryptString(item.SecretCipher, item.SecretIv, key);
        }

        public void DeleteItem(long id) => _repo.DeleteVaultItem(id);

        public void Lock()
        {
            if (_key != null)
                Array.Clear(_key, 0, _key.Length);
            _key = null;
        }
    }
}
