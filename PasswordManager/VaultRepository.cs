using Microsoft.Data.Sqlite;

namespace PasswordManager
{
    public sealed class VaultRepository
    {
        private readonly string _connectionString;

        public VaultRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};Cache=Shared";
            Initialize();
        }

        private void Initialize()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var createKeyTable = @"
CREATE TABLE IF NOT EXISTS KeyDerivation (
    Id INTEGER PRIMARY KEY CHECK (Id = 1),
    Salt BLOB NOT NULL,
    Iterations INTEGER NOT NULL
);";

            var createVaultTable = @"
CREATE TABLE IF NOT EXISTS VaultItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Username TEXT,
    SecretType INTEGER NOT NULL,
    SecretCipher BLOB NOT NULL,
    SecretIV BLOB NOT NULL,
    CreatedUtc TEXT NOT NULL,
    UpdatedUtc TEXT NOT NULL
);";

            using var cmd1 = new SqliteCommand(createKeyTable, conn);
            cmd1.ExecuteNonQuery();

            using var cmd2 = new SqliteCommand(createVaultTable, conn);
            cmd2.ExecuteNonQuery();
        }

        public (byte[] salt, int iterations)? GetKeyDerivation()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            const string sql = "SELECT Salt, Iterations FROM KeyDerivation WHERE Id = 1;";
            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            var salt = (byte[])reader["Salt"];
            var iterations = Convert.ToInt32(reader["Iterations"]);
            return (salt, iterations);
        }

        public void InsertKeyDerivation(byte[] salt, int iterations)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            const string sql = @"
INSERT INTO KeyDerivation (Id, Salt, Iterations)
VALUES (1, @salt, @iterations)
ON CONFLICT(Id) DO UPDATE SET Salt = excluded.Salt, Iterations = excluded.Iterations;";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@iterations", iterations);
            cmd.ExecuteNonQuery();
        }

        public long InsertVaultItem(VaultItem item)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            const string sql = @"
INSERT INTO VaultItems
(Title, Username, SecretType, SecretCipher, SecretIV, CreatedUtc, UpdatedUtc)
VALUES (@Title, @Username, @SecretType, @SecretCipher, @SecretIV, @CreatedUtc, @UpdatedUtc);
SELECT last_insert_rowid();";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", item.Title);
            cmd.Parameters.AddWithValue("@Username", (object?)item.Username ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SecretType", (int)item.SecretType);
            cmd.Parameters.AddWithValue("@SecretCipher", item.SecretCipher);
            cmd.Parameters.AddWithValue("@SecretIV", item.SecretIv);
            cmd.Parameters.AddWithValue("@CreatedUtc", item.CreatedUtc.ToUniversalTime().ToString("O"));
            cmd.Parameters.AddWithValue("@UpdatedUtc", item.UpdatedUtc.ToUniversalTime().ToString("O"));

            var id = (long)(cmd.ExecuteScalar() ?? 0);
            return id;
        }

        public List<VaultItem> GetAllVaultItems()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            const string sql = @"
SELECT Id, Title, Username, SecretType, SecretCipher, SecretIV, CreatedUtc, UpdatedUtc
FROM VaultItems
ORDER BY Title ASC;";

            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            var list = new List<VaultItem>();
            while (reader.Read())
            {
                var item = new VaultItem
                {
                    Id = reader.GetInt64(0),
                    Title = reader.GetString(1),
                    Username = reader.IsDBNull(2) ? null : reader.GetString(2),
                    SecretType = (SecretType)reader.GetInt32(3),
                    SecretCipher = (byte[])reader[4],
                    SecretIv = (byte[])reader[5],
                    CreatedUtc = DateTime.Parse(reader.GetString(6)),
                    UpdatedUtc = DateTime.Parse(reader.GetString(7))
                };
                list.Add(item);
            }

            return list;
        }

        public VaultItem? GetVaultItem(long id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            const string sql = @"
SELECT Id, Title, Username, SecretType, SecretCipher, SecretIV, CreatedUtc, UpdatedUtc
FROM VaultItems
WHERE Id = @Id;";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new VaultItem
            {
                Id = reader.GetInt64(0),
                Title = reader.GetString(1),
                Username = reader.IsDBNull(2) ? null : reader.GetString(2),
                SecretType = (SecretType)reader.GetInt32(3),
                SecretCipher = (byte[])reader[4],
                SecretIv = (byte[])reader[5],
                CreatedUtc = DateTime.Parse(reader.GetString(6)),
                UpdatedUtc = DateTime.Parse(reader.GetString(7))
            };
        }

        public void UpdateVaultItem(VaultItem item)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            const string sql = @"
UPDATE VaultItems
SET Title = @Title,
    Username = @Username,
    SecretType = @SecretType,
    SecretCipher = @SecretCipher,
    SecretIV = @SecretIV,
    UpdatedUtc = @UpdatedUtc
WHERE Id = @Id;";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", item.Title);
            cmd.Parameters.AddWithValue("@Username", (object?)item.Username ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SecretType", (int)item.SecretType);
            cmd.Parameters.AddWithValue("@SecretCipher", item.SecretCipher);
            cmd.Parameters.AddWithValue("@SecretIV", item.SecretIv);
            cmd.Parameters.AddWithValue("@UpdatedUtc", item.UpdatedUtc.ToUniversalTime().ToString("O"));
            cmd.Parameters.AddWithValue("@Id", item.Id);

            cmd.ExecuteNonQuery();
        }


        public void DeleteVaultItem(long id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            const string sql = "DELETE FROM VaultItems WHERE Id = @Id;";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
